using System.Security.Cryptography;
using System.Text;
using Library.Models;
using StocksApi.Interfaces;
using Library.Interfaces;

namespace StocksApi.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly string _salt;

        public PasswordHasher(IAppConfiguration financeConfiguration)
        {
            var variables = financeConfiguration.Get<Variables>(ConfigurationKeys.AppVariablesSection);
            _salt = variables.PASSWORD_SALT;
        }

        public string HashPassword(string password)
        {
            string saltedPassword = password + _salt;
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));

            var builder = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
                builder.Append(hashBytes[i].ToString("x2"));

            return builder.ToString();
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedProvidedPassword = HashPassword(password);

            return hashedProvidedPassword.Equals(hashedPassword);
        }
    }
}