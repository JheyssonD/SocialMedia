using Microsoft.Extensions.Options;
using SocialMedia.Infrastucture.Interfaces;
using SocialMedia.Infrastucture.Options;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace SocialMedia.Infrastucture.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions Options;

        public PasswordService(IOptions<PasswordOptions> options)
        {
            Options = options.Value;
        }

        public bool Check(string hash, string password)
        {
            string[] parts = hash.Split(".", 3);
            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected Hash format");
            }
            int iterations = Convert.ToInt32(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] key = Convert.FromBase64String(parts[2]);

            using (Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA512
            ))
            {
                byte[] keyToCheck = algorithm.GetBytes(Options.SaltSize);

                return keyToCheck.SequenceEqual(key);
            }
        }

        public string Hash(string password)
        {
            //PBKDF2 implementation
            using (Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
                password,
                Options.SaltSize,
                Options.Iterations,
                HashAlgorithmName.SHA512
            ))
            {
                string key = Convert.ToBase64String(algorithm.GetBytes(Options.SaltSize));
                string salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Options.Iterations}.{salt}.{key}";
            }
        }
    }
}
