using BookStoreAppCore.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BookStoreAppCore.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly SMTPConfigModel _smtpConfig;
        private const string templatePath = @"EmailTemplates/{0}.html";

        public EmailServices(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }
        public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholder("Hi {{Username}}, this is email confirmation ", userEmailOptions.Placeholder);
            userEmailOptions.Body = UpdatePlaceholder(GetEmailBody("EmailVerification"), userEmailOptions.Placeholder);
            await sendEmail(userEmailOptions);
        }
        private async Task sendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                IsBodyHtml = _smtpConfig.isHTMLBody
            };
            foreach (var email in userEmailOptions.toEmails)
            {
                mail.To.Add(email);
            }
            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                UseDefaultCredentials = _smtpConfig.UserDefaultCredentials,
                EnableSsl = _smtpConfig.EnabledSSL,
                Credentials = networkCredential
            };
            mail.BodyEncoding = System.Text.Encoding.Default;
            await smtpClient.SendMailAsync(mail);
        }
        private string UpdatePlaceholder(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }
        public async Task SendEmailForResetPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholder("Hi {{Username}}, reset password ", userEmailOptions.Placeholder);
            userEmailOptions.Body = UpdatePlaceholder(GetEmailBody("ResetPassword"), userEmailOptions.Placeholder);
            await sendEmail(userEmailOptions);
        }
    }
}
