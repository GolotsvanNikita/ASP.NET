using System.Security.Cryptography;
using System.Text;

namespace MusicPortal.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }
    }
}