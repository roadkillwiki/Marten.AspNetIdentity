using System.Threading.Tasks;

namespace Marten.AspNetIdentity.Example.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
