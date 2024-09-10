using API.Helper.Interfaces;

namespace API.Helper
{
    public class BcryptHelper : IBcryptHelper
    {
        public string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            return hash;
        }

        public bool VerifyPassword(string password, string hash)
        {
            var verify = BCrypt.Net.BCrypt.Verify(password, hash);
            return verify;
        }
    }
}
