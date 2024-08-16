using Microsoft.AspNetCore.Identity;

namespace ShoppingMarket.Account.Models
{
    public class ApplicationUser:IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
