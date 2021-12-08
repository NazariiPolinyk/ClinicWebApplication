using ClinicWebApplication.Interfaces;

namespace ClinicWebApplication.DataLayer.Models
{
    class Admin : IEntity, IAccount
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
