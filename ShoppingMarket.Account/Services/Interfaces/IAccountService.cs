using Microsoft.AspNetCore.Http;
using ShoppingMarket.Account.Models;

namespace ShoppingMarket.Account.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model, HttpRequest request);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<AuthModel> ConfirmEmailAsync(string userId, string token);
        Task<string> GeneratePasswordResetTokenAsync(string email, HttpRequest request);
        Task<AuthModel> ResetPasswordAsync(ResetPasswordModel model);
        Task<AuthModel> LoginAsync(LoginModel loginModel);
        Task<AuthModel> LogoutAsync();
        Task<string> AddRoleAsync(AddRoleModel Rolemodel);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

    }
}
