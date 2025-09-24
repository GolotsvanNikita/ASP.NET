using System.Security.Cryptography;
using System.Text;

namespace GuestBook.Services
{
    public class PasswordHash : IPasswordHash
    {
        public string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
