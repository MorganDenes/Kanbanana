using Kanbanana.Mail;
using Microsoft.AspNetCore.Identity;
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
        //private readonly UserManager<IdentityUser> _userManager;
        private MailJetOptions _options;

        //public MailJet(UserManager<IdentityUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public void AddOptions(MailJetOptions options)
        {
            _options = options;
        }

        //public async void ComposeVerificationEmail(IdentityUser user)
        //{
        //    await _userManager.AddToRoleAsync(user, "Employee");
        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var link = Url.Action(nameof(Controllers.HomeController.VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
        //    if (_configuration.GetSection("EmailAuthentication").Value != "False")
        //    {
        //        var confirmedEmail = "MorganADenes@gmail.com";
        //        var subject = "Kanbanana - Verify Email";
        //        await _email.Send(confirmedEmail, subject, $"<a href=\"{link}\">Click Here</a>", _mailOptions.Value);

        //        return RedirectToAction("EmailVerification");
        //    }
        //    else
        //    {
        //        return RedirectToAction("VerifyEmail", "Home", new { userId = user.Id, code });
        //    }
        //}

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
