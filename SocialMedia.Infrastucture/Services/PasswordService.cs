using Microsoft.Extensions.Options;
using SocialMedia.Infrastucture.Interfaces;
using SocialMedia.Infrastucture.Options;
using System;
using System.Security.Cryptography;

namespace SocialMedia.Infrastucture.Services
{
    class PasswordService : IPasswordHasher
    {
        private readonly PasswordOptions Options;

        public PasswordService(IOptions<PasswordOptions> options)
        {
            Options = options.Value;
        }

        public bool check(string hash, string password)
        {
            throw new NotImplementedException();
        }

        public string hash(string password)
        {
            //PBKDF2 implementation
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                Options.SaltSize,
                Options.Iterations,
                HashAlgorithmName.SHA512
            ))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(Options.SaltSize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Options.Iterations}.{salt}.{key}";
            }
        }
    }
}
