using Core.UserManager;
using Infrastructure.Extensions;
using JO.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.DB.Context.EFCore
{
    public class MembershipEFCoreContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MembershipEFCoreContext(DbContextOptions<MembershipEFCoreContext> options, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (modelBuilder == null)
                return;

            modelBuilder.RegisterAllEntities<IJOEntity>(typeof(User).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(User).Assembly);
            modelBuilder.RegisterEntityTypeConfiguration(typeof(User).Assembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.EnableDetailedErrors();
        }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                {
                    continue;
                }
                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));
                foreach (var property in properties)
                {
                    var val = property.GetValue(item.Entity, null);
                    if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    {
                        var newVal = (val?.ToString() ?? "").CleanString().FixPersianChars();
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}
