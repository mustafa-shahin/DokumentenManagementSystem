using DMS.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net;
using System.Net.Mail;
namespace DMS.Service
{
    public class EmailService(DataContext context)
    {
        private readonly DataContext _context = context;
        private readonly Dictionary<string, (string Email, string VerificationCode)> _verificationCodes = [];

 
        public async Task<bool> SendVerificationCode(string username, string email)
        {
            var user = await _context.Benutzer.FirstOrDefaultAsync(b => b.Name == username);
            var verificationCode = new Random().Next(100000, 999999).ToString();

            // Store the verification code
            _verificationCodes[username] = (email, verificationCode);

            string emailBody = $"Hallo {username},\n\n" +
                   $"Sie haben eine Anfrage zum Zurücksetzen Ihres Passworts gestellt.\n" +
                   $"Ihr Bestätigungscode lautet: {verificationCode}\n\n" +
                   "Bitte verwenden Sie diesen Code, um Ihr Passwort zurückzusetzen.\n\n" +
                   "Wenn Sie diese Anfrage nicht gestellt haben, ignorieren Sie bitte diese E-Mail.\n\n" +
                   "Ihr Support-Team";
            return SendEmail(email, "Passwort-Wiederherstellungscode", emailBody);
        }

        public bool VerifyCode(string username, string code)
        {
            if (_verificationCodes.TryGetValue(username, out (string Email, string VerificationCode) value) && value.VerificationCode == code)
                return true;

            return false;
        }

        private static bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var configSmtpUser = ConfigurationManager.AppSettings["SMTPUser"];
                var configSmtpPassword = ConfigurationManager.AppSettings["SMTPPassword"];
                var configsmtpFromAddress = ConfigurationManager.AppSettings["SMTPFromAddress"];
                var configsmtpFromDisplayName = ConfigurationManager.AppSettings["SMTPFromDisplayName"] ?? "Support";

                if(string.IsNullOrEmpty(configSmtpUser) && string.IsNullOrEmpty(configSmtpPassword) && string.IsNullOrEmpty(configsmtpFromAddress))
                    return false;

                using MailMessage message = new();
                message.From = new MailAddress(configsmtpFromAddress);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = subject;
                message.Body = body;
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(configSmtpUser, configSmtpPassword),
                    EnableSsl = true
                };

                // Send the email
                smtpClient.Send(message);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
