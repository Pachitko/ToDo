using Core.Application.Abstractions;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class EmailSender : IEmailSender, IEmailConfirmationLinkSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _httpContext;
        private readonly UserManager<AppUser> _userManager;

        // todo: Use SendGrid NuGet Package
        public EmailSender(ILogger<EmailSender> logger, IHttpContextAccessor accessor, LinkGenerator generator,
            UserManager<AppUser> userManager)
        {
            _logger = logger;
            _httpContext = accessor.HttpContext;
            _linkGenerator = generator;
            _userManager = userManager;
        }

        public async Task SendConfirmationCodeAsync(AppUser user)
        {
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));
            var callbackUrl = _linkGenerator.GetUriByAction(_httpContext, "ConfirmEmail", values: new { userId = user.Id, code });
            await SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl ?? "")}'>clicking here</a>.");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogWarning("NotImplementedException");
            return Task.CompletedTask;
            // todo Receive settings from options
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

            client.SendAsync(emailMessage, null);

            _logger.LogWarning($"Email has been sent: \nto: {email}\nSubject: {subject}\nHtmlMesage: {htmlMessage};");
            return Task.CompletedTask;
        }
    }
}