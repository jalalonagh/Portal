using Api.Middlewares;
using ApplicationService.Extensions;
using ApplicationService.UserManager;
using Core;
using Core.UserManager;
using Infrastructure.DB;
using Infrastructure.DB.Context.EFCore;
using Infrastructure.Extensions;
using JO.AutoMapper;
using JO.Data.Base.Interfaces;
using JO.Shared;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using SharedWebApi.Middlewares;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

internal static class Program
{
    private static readonly string cors = "_myAllowSpecificOrigins";
    private static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("fa");
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

            builder.Services.AddRequestTimeouts();

            builder.Services.AddJODbContext(_configuration);
            builder.Services.AddScoped<IUnitOfWork<MembershipEFCoreContext>, UnitOfWork>();

            builder.Services.AddIdentity<User, Role>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._@";
            })
                .AddEntityFrameworkStores<MembershipEFCoreContext>()
                .AddDefaultTokenProviders();

            var addresses = _configuration.GetSection("Cors").Get<List<string>>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: cors,
                    builder =>
                    {
                        builder
                        .WithOrigins(addresses.ToArray())
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");
                })
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                })
                .AddCertificate();

            builder.Services.AddMvcCore()
                            .AddNewtonsoftJson(x =>
                            {
                                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                                x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                            })
                            .AddApiExplorer()
                            .AddAuthorization()
                            .AddFormatterMappings()
                            .AddDataAnnotations()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                options.JsonSerializerOptions.WriteIndented = false;
                                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "سامانه بازارگاه", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "توکن معتبر خود را وارد کنید",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddSharedServices();
            builder.Services.AddJORepositories();
            builder.Services.AddJODomainServices();
            builder.Services.AddJOServices();
            builder.Services.AddJOAutoMapperServices(typeof(UserManagerProfile).Assembly);

            var app = builder.Build();

            app.UseRequestTimeouts();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    ci,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    ci,
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<JWTMiddleware>();
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            app.UseMiddleware<TransactionMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(cors);
            app.MapControllers();

            app.Run();
        }
        catch (Exception e)
        {
            var text = DateTime.Now.ToString() + Environment.NewLine + "--------------------------------------" + Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(e);
            if (!File.Exists("start_error.txt"))
            {
                File.Create("start_error.txt").Close();
            }

            File.WriteAllText("start_error.txt", text);

            throw;
        }
    }
}