using System.Text.Json.Serialization;

namespace ShoppingMarket.Account.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public bool IsExpired { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }


    }
}
