using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    [Owned]
    public class RefreshToken
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsRevoked => RevokedOn != null;






    }
}
