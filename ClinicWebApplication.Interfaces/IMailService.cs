using System.Threading.Tasks;

namespace ClinicWebApplication.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(IMailRequest mailRequest);
    }
}
