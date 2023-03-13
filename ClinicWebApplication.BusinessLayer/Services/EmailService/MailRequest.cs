using ClinicWebApplication.Interfaces;

namespace ClinicWebApplication.BusinessLayer.Services.EmailService
{
    public class MailRequest : IMailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public object Attachments { get; internal set; }
    }
}
