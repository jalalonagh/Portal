using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JO.Data.Base;

namespace Core.UserManager
{
    public class Role : BaseIdentityRoleEntity
    {
        public string Title { get; protected set; }

        protected Role() { }

        public Role(string title, string name)
        {
            Title = title;
            Name = name;
        }

        public override void Validate()
        {

        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role), UserManagerConstants.TableSchema, t => t.IsTemporal());

            builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.ConcurrencyStamp).HasMaxLength(200);
            builder.Property(p => p.NormalizedName).HasMaxLength(50);

            builder.HasQueryFilter(e => e.IsDeleted != true);
        }
    }
}
