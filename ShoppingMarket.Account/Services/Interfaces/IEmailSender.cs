using System.Threading.Tasks;

namespace ShoppingMarket.Account.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
