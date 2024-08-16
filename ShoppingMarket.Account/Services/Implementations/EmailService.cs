using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;
using ShoppingMarket.Account.Services.Interfaces;


namespace ShoppingMarket.Account.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await _emailSender.SendEmailAsync(email, subject, htmlMessage);
        }

        public string GenerateEmailConfirmationLink(string userId, string token, HttpRequest request)
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{request.Scheme}://{request.Host}{request.PathBase}/api/account/confirm-email?userId={userId}&token={encodedToken}";
            return confirmationLink;
        }

        public string GeneratePasswordResetLink(string email, string token, HttpRequest request)
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink = $"{request.Scheme}://{request.Host}{request.PathBase}/api/account/reset-password?token={encodedToken}";
            return resetLink;
        }
    }
}
