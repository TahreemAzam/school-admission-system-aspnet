using System.Threading.Tasks;

namespace SchoolWebsite1.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
