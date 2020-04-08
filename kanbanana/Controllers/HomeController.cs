using Kanbanana.Data;
using Kanbanana.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Models;

namespace Kanbanana.Controllers
{
    public class HomeController : Controller
    {
        private readonly KanbananaDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOptions<MailJetOptions> _mailOptions;
        private readonly MailJetInterface _email;

        public HomeController(
            KanbananaDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<MailJetOptions> mailOptions, MailJetInterface email)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailOptions = mailOptions;
            _email = email;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        [Authorize(Policy = "Company")]
        public IActionResult CompanySecret()
        {
            return View("Secret");
        }

        [Authorize(Policy = "Employee")]
        public IActionResult EmployeeSecret()
        {
            return View("Secret");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {           
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            string email,
            string company,
            string password,
            [FromServices] IConfiguration config)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _context.UserCompanies.Add(new UserCompany { UserId = user.Id, CompanyName = company });
                _context.SaveChanges();

                await _userManager.AddToRoleAsync(user, "Company");

                // !!! Refactor into MailJet
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
                if (config.GetSection("EmailAuthentication").Value != "False")
                {
                    var confirmedEmail = "MorganADenes@gmail.com";
                    var subject = "Kanbanana - Verify Email";
                    await _email.Send(confirmedEmail, subject, $"<a href=\"{link}\">Click Here</a>", _mailOptions.Value);

                    return RedirectToAction("EmailVerification");
                }
                else
                {
                    return RedirectToAction("VerifyEmail", new { userId = user.Id, code });
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code, [FromServices] IAuthorizationService authService)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult EmailVerification()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
