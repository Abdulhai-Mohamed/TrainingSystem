using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}
