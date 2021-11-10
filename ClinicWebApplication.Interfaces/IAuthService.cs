namespace ClinicWebApplication.Interfaces
{
    public interface IAuthService<T>
    {
        IAccount Authenticate(string email, string password);
    }
}
