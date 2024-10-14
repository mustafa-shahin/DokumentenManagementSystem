using DMS.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net;
using System.Net.Mail;
namespace DMS.Service
{
    public class EmailService
    {
        private readonly DataContext _context;
        private readonly Dictionary<string, string> _userEmails;  // In-memory storage for email addresses
        private readonly Dictionary<string, (string Email, string VerificationCode)> _verificationCodes;

        public EmailService(DataContext context)
        {
            _context = context;
            _userEmails = new Dictionary<string, string>(); // Stores the username-email mapping
            _verificationCodes = new Dictionary<string, (string Email, string VerificationCode)>();
        }

        public void AddUserEmail(string username, string email)
        {
            if (!_userEmails.ContainsKey(username))
            {
                _userEmails.Add(username, email);
            }
        }

        public async Task<bool> SendVerificationCode(string username, string email)
        {
            // Check if the user exists
            AddUserEmail(username, email);
            var user = await _context.Benutzer.FirstOrDefaultAsync(b => b.Name == username);
            if (user == null || !_userEmails.ContainsKey(username) || _userEmails[username] != email)
            {
                return false;
            }

            var verificationCode = GenerateVerificationCode();

            // Store the verification code in memory
            if (_verificationCodes.ContainsKey(username))
            {
                _verificationCodes[username] = (email, verificationCode);
            }
            else
            {
                _verificationCodes.Add(username, (email, verificationCode));
            }
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
            {
                return true;
            }
            return false;
        }

        private string GenerateVerificationCode() => new Random().Next(100000, 999999).ToString();

        private static bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var configSmtpServer = ConfigurationManager.AppSettings["SMTPServer"];
                var configSmtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                var configSmtpUser = ConfigurationManager.AppSettings["SMTPUser"];
                var configSmtpPassword = ConfigurationManager.AppSettings["SMTPPassword"];
                var configsmtpEnableSSL = bool.Parse(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
                var configsmtpFromAddress = ConfigurationManager.AppSettings["SMTPFromAddress"];
                var configsmtpFromDisplayName = ConfigurationManager.AppSettings["SMTPFromDisplayName"];
                using MailMessage message = new();
                message.From = new MailAddress(configSmtpUser);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = subject;
                message.Body = body;
                var smtpClient = new SmtpClient(configSmtpServer)
                {
                    Port = configSmtpPort,
                    Credentials = new NetworkCredential(configSmtpUser, configSmtpPassword),
                    EnableSsl = configsmtpEnableSSL
                };

                // Send the email
                smtpClient.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
