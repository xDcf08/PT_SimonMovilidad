using SimonMovilidad.Application.Abstractions.PasswordHash;
using System.Security.Cryptography;

namespace SimonMovilidad.Persistence.Hasher
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 600000;
        private const char Delimiter = ';';
        public string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);

            return $"{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string storedHashString)
        {
            var parts = storedHashString.Split(Delimiter);
            if (parts.Length != 2)
            {
                throw new FormatException("Stored hash is in an invalid format.");
            }

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(HashSize);

            return CryptographicOperations.FixedTimeEquals(computed, storedHash);
        }
    }
}
