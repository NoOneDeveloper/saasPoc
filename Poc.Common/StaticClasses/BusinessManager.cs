using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Common.StaticClasses
{
    public static class BusinessManager
    {


    }
    public static class PasswordHasher
    {
        private const int SaltSize = 16;   // 128 bits
        private const int HashSize = 32;   // 256 bits
        private const int Iterations = 100000;

        // ✅ Generate salt
        public static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // ✅ Hash password with provided salt
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        // ✅ Verify password
        public static bool VerifyPassword(string password, byte[] salt, byte[] hash)
        {
            var computedHash = HashPassword(password, salt);

            // Compare byte by byte
            for (int i = 0; i < HashSize; i++)
            {
                if (computedHash[i] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
