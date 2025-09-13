using System.Security.Cryptography;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;

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


        public static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }


        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
      

                return pbkdf2.GetBytes(HashSize);
            }
        }

        // Verify password
        public static bool VerifyPassword(string password, byte[] salt, byte[] hash)
        {
            var computedHash = HashPassword(password, salt);

            for (int i = 0; i < HashSize; i++)
            {
                if (computedHash[i] != hash[i])
                    return false;
            }
            return true;
        }
    }
    #region Email
    public static class EmailHelper
    {
        private static readonly string _smtpServer = "smtp.gmail.com";
        private static readonly int _port = 587;
        private static readonly string _fromEmail = "alihassanaslam2000@gmail.com"; // aapka Gmail
        private static readonly string _fromPassword = "pdynqfgeihccskna"; // Gmail App Password

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("POC", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = "This is a plain text fallback for clients that don't support HTML."
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_fromEmail, _fromPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
    #endregion



    #region EmailTemplates
    public static class EmailTemplates
    {
        public static string Welcome(string firstName) =>
            $"<h3>Hello {firstName}</h3><p>Thanks for signing up!</p><h2>Welcome to POC</h2>";

        public static string AccountCreated(string firstName) =>
            $"<h3>Hello {firstName}</h3><p>Your account has been created successfully 🎉</p>";

        public static string PasswordReset(string firstName, string resetLink) =>
            $"<h3>Hello {firstName}</h3><p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
    }
    #endregion

}
