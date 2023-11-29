using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> EntityTypeBuilder)
        {
            EntityTypeBuilder.HasData(
                                new Company
                                {
                                    Id = new Guid("92f04823-3b8f-48c4-a37f-32529f8724f5"),
                                    Name = "Nasa",
                                    Adress = "any adress at Cairo",
                                    Country = "Egypt"
                                },
                                new Company
                                {
                                    Id = new Guid("455cdf8b-9469-45e4-aa05-0c24b3541503"),
                                    Name = "Tesla",
                                    Adress = "any adress at Maiami",
                                    Country = "USA"
                                }
                            );
        }
    }


}
