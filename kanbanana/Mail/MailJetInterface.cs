using Kanbanana.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Kanbanana.Mail
{
    public interface MailJetInterface
    {
        void AddOptions(MailJetOptions options);
        Task Send(string emailAddress, string subject, string body, MailJetOptions options);
    }
}
