using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        // todo: Use SendGrid NuGet Package
        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogWarning("throw new System.NotImplementedException()");
            return Task.CompletedTask;
            // Receive settings from options
            MailAddress from = new("", "ToDo");
            MailAddress to = new(email);

            MailMessage emailMessage = new(from, to)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            using var client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("", ""),
                EnableSsl = true
            };

            client.Send(emailMessage);

            _logger.LogWarning($"Email has been sent: \nto: {email}\nSubject: {subject}\nHtmlMesage: {htmlMessage};");
            return Task.CompletedTask;
        }
    }
}