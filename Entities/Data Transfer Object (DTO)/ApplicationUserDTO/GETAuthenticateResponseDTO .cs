using Newtonsoft.Json;
namespace Entities.Data_Transfer_Object__DTO_.ApplicationUserDTO
{
    public class GETAuthenticateResponseDTO
    {

        public string? Message { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public ICollection<string>? Roles { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? ExpiresOn { get; set; }
        // refresh token is returned in http only cookie
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
