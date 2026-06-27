using System;
using System.Security.Cryptography;

namespace DVLD.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                byte[] salt = algorithm.Salt;
                byte[] key = algorithm.GetBytes(KeySize);

                byte[] result = new byte[SaltSize + KeySize];
                Array.Copy(salt, 0, result, 0, SaltSize);
                Array.Copy(key, 0, result, SaltSize, KeySize);

                return Convert.ToBase64String(result);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] bytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(bytes, 0, salt, 0, SaltSize);

            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                byte[] key = algorithm.GetBytes(KeySize);

                for (int i = 0; i < KeySize; i++)
                {
                    if (bytes[i + SaltSize] != key[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}