namespace ClinicWebApplication.Interfaces
{
    public interface IAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
