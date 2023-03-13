using System.ComponentModel.DataAnnotations;

namespace ClinicWebApplication.BusinessLayer.Services.AuthenticationService
{
    public class AuthenticateModel
    {
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
