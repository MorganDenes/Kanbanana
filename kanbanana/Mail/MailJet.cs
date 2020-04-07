using Kanbanana.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Kanbanana.Mail
{
    public class MailJet : MailJetInterface
    {
        private MailJetOptions _options;
        public void AddOptions(MailJetOptions options)
        {
            _options = options;
        }

        public async Task Send(string emailAddress, string subject, string body, MailJetOptions options)
        {
            var client = new SmtpClient();

            client.Host = options.Host;
            client.Credentials = new NetworkCredential(options.APIKEY, options.APIKeySecret);
            client.Port = options.Port;

            var message = new MailMessage(options.SenderEmail, emailAddress);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }

    }
}
