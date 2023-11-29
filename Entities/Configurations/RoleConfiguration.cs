using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> EntityTypeBuilder)
        {
            EntityTypeBuilder.HasData(
                                new IdentityRole
                                {
                                    Name = "Manager",
                                    NormalizedName = "MANAGER"
                                },
                                new IdentityRole
                                {
                                    Name = "Adminstrator",
                                    NormalizedName = "ADMINSTRATOR"
                                }
                            );
        }
    }
}
