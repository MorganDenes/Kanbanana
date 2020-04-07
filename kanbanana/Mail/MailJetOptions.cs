using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Mail
{
    public class MailJetOptions
    {
        public string Host { get; set; }
        public string APIKEY { get; set; }
        public string APIKeySecret { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
    }
}
