namespace API.Helper.Interfaces
{
    public interface IBcryptHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);

        string GenerateSalt();
    }
}
