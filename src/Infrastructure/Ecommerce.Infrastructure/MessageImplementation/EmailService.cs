using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.MessageImplementation
{
    public class EmailService : IEmailService
    {

        public EmailSettings EmailSettings { get; }

        public ILogger<EmailSettings> Logger { get; }

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailSettings> logger)
        {
            EmailSettings = emailSettings.Value;
            Logger = logger;
        }

        public async Task<bool> SendEmail(EmailMessage email, string token)
        {
            try
            {
                var client = new SendGridClient(EmailSettings.AppAPIKey);
                var from = new EmailAddress(EmailSettings.Email);
                var subject = email.Subject;
                var to = new EmailAddress(email.To, email.To);

                var plainTextContent = email.Body;
                var htmlContent = $"{email.Body} {EmailSettings.BaseUrlClient}/password/reset/{token}";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                Logger.LogError("The Email wasn´t sended, there are errors");
                return false;
            }
        }
    }
}
