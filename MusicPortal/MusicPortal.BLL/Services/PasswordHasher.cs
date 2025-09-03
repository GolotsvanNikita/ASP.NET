using System.Security.Cryptography;
using System.Text;

namespace MusicPortal.BLL.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = ComputeHash(password, salt);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] computedHash = ComputeHash(password, salt);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != computedHash[i])
                    return false;
            }
            return true;
        }

        private byte[] ComputeHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];
                Array.Copy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);
                return sha256.ComputeHash(saltedPassword);
            }
        }
    }
}