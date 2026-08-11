using System;
using System.Security.Cryptography;
using System.Text;

namespace Fit.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "Password cannot be empty.",
                    nameof(password));
            }

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize
            );

            return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            if (storedHash.StartsWith("PBKDF2$", StringComparison.Ordinal))
            {
                return VerifyPbkdf2(password, storedHash);
            }

            // Podrška za stare SHA-256 hash vrijednosti.
            return VerifyLegacySha256(password, storedHash);
        }

        public static bool NeedsRehash(string storedHash)
        {
            return string.IsNullOrWhiteSpace(storedHash) ||
                   !storedHash.StartsWith("PBKDF2$", StringComparison.Ordinal);
        }

        private static bool VerifyPbkdf2(
            string password,
            string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split('$');

                if (parts.Length != 4 ||
                    !string.Equals(
                        parts[0],
                        "PBKDF2",
                        StringComparison.Ordinal))
                {
                    return false;
                }

                if (!int.TryParse(parts[1], out int iterations) ||
                    iterations <= 0)
                {
                    return false;
                }

                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expectedHash =
                    Convert.FromBase64String(parts[3]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    expectedHash.Length
                );

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash
                );
            }
            catch
            {
                return false;
            }
        }

        private static bool VerifyLegacySha256(
            string password,
            string storedHash)
        {
            try
            {
                byte[] expectedHash =
                    Convert.FromBase64String(storedHash);

                byte[] passwordBytes =
                    Encoding.UTF8.GetBytes(password);

                byte[] actualHash =
                    SHA256.HashData(passwordBytes);

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash
                );
            }
            catch
            {
                return false;
            }
        }
    }
}