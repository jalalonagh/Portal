using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JO.Data.Base;
using JO.Data.Base.ValueObjects.Types;

namespace Core.UserManager
{
    public class User : BaseIdentityUserEntity
    {
        public JODateTime Birthday { get; protected set; }
        public JONationalCode Identity { get; protected set; }
        public JOMobile Mobile { get; protected set; }
        public JOFullName? PersonalData { get; protected set; }
        public JOAvatar? Avatar { get; protected set; }
        public string? PhoneNumber { get; protected set; }
        public JOAddress? Address { get; protected set; }

        protected User() { }

        public User(string birthday, string identity, string mobile, string phoneNumber, string postCode)
        {
            Birthday = new JODateTime(birthday);
            Identity = new JONationalCode(identity);
            Mobile = new JOMobile(mobile);
            UserName = identity;
            Email = identity + "@temp.ir";
            ModifiedTime = DateTime.Now;
            ModifiedTimePersian = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            PhoneNumber = phoneNumber;
            Address = new JOAddress(postCode);

            Validate();
        }

        public User(string birthday, string identity, string mobile, string phoneNumber, JOAddress address)
        {
            Birthday = new JODateTime(birthday);
            Identity = new JONationalCode(identity);
            Mobile = new JOMobile(mobile);
            UserName = identity;
            Email = identity + "@temp.ir";
            ModifiedTime = DateTime.Now;
            ModifiedTimePersian = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            PhoneNumber = phoneNumber;
            Address = JOAddress.Copy(address);

            Validate();
        }

        public User(string birthday, string identity, string mobile, string postCode, string phoneNumber, string? firstName, string? lastName, string? avatar, string? root)
        {
            Birthday = new JODateTime(birthday);
            Identity = new JONationalCode(identity);
            Mobile = new JOMobile(mobile);
            PersonalData = new JOFullName(firstName, lastName);
            Avatar = new JOAvatar(avatar, root);
            ModifiedTime = DateTime.Now;
            ModifiedTimePersian = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            PhoneNumber = phoneNumber;
            Address = new JOAddress(postCode);

            Validate();
        }

        public void UpdatePersonalData(string? firstName, string? lastName, string? avatar, string? root)
        {
            PersonalData = new JOFullName(firstName, lastName);
            Avatar = new JOAvatar(avatar, root);
            ModifiedTime = DateTime.Now;
            ModifiedTimePersian = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            Validate();
        }

        public override void Validate()
        {
            Birthday.Validate();
            Identity.Validate();
            Mobile.Validate();
            PersonalData?.Validate();
            Address?.Validate();
            Avatar?.Validate();
        }
    }

    public class Configuration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), UserManagerConstants.TableSchema, t => t.IsTemporal());

            builder.OwnsOne(p => p.Birthday).ToTable($"{nameof(User)}_Birthday", UserManagerConstants.TableSchema, t => t.IsTemporal());
            builder.OwnsOne(p => p.Identity).ToTable($"{nameof(User)}_Identity", UserManagerConstants.TableSchema, t => t.IsTemporal());
            builder.OwnsOne(p => p.Mobile).ToTable($"{nameof(User)}_Mobile", UserManagerConstants.TableSchema, t => t.IsTemporal());
            builder.OwnsOne(p => p.PersonalData).ToTable($"{nameof(User)}_PersonalData", UserManagerConstants.TableSchema, t => t.IsTemporal());
            builder.OwnsOne(p => p.Avatar).ToTable($"{nameof(User)}_Avatar", UserManagerConstants.TableSchema, t => t.IsTemporal());
            builder.OwnsOne(p => p.Address).ToTable($"{nameof(User)}_Address", UserManagerConstants.TableSchema, t => t.IsTemporal());

            builder.Property(p => p.UserName).HasMaxLength(50);
            builder.Property(p => p.SecurityStamp).HasMaxLength(100);
            builder.Property(p => p.Email).HasMaxLength(100);
            builder.Property(p => p.NormalizedEmail).HasMaxLength(100);
            builder.Property(p => p.NormalizedUserName).HasMaxLength(50);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.SecurityStamp).HasMaxLength(100);
            builder.Property(p => p.PasswordHash).HasMaxLength(1000);

            builder.HasQueryFilter(e => e.IsDeleted != true);
        }
    }
}
