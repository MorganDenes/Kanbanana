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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kanbanana.Controllers
{
    [Authorize(Policy = "Company")]
    public class CompanyController : Controller
    {
        private readonly KanbananaDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IOptions<MailJetOptions> _mailOptions;
        private readonly MailJetInterface _email;

        public CompanyController(
            KanbananaDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IOptions<MailJetOptions> mailOptions, MailJetInterface email)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mailOptions = mailOptions;
            _email = email;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult ViewUser()
        {
            var users = _userManager.Users.ToList();
            IEnumerable<string> convertedUsers = users.Select(x => new string(x.Email));
            return View(new DisplayViewModel { Users = convertedUsers });
        }



        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var companyUser = await _userManager.GetUserAsync(HttpContext.User);
                var company = _context.UserCompanies
                                        .Where(x => x.UserId == companyUser.Id)
                                        .Select(s => s.CompanyName)
                                        .ToList()[0];

                _context.UserCompanies.Add(new UserCompany { UserId = user.Id, CompanyName = company });
                _context.SaveChanges();

                await _userManager.AddToRoleAsync(user, "Employee");

                // !!! Refactor into MailJet
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(Kanbanana.Controllers.HomeController.VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
                if (_configuration.GetSection("EmailAuthentication").Value != "False")
                {
                    var confirmedEmail = "MorganADenes@gmail.com";
                    var subject = "Kanbanana - Verify Email";
                    await _email.Send(confirmedEmail, subject, $"<a href=\"{link}\">Click Here</a>", _mailOptions.Value);

                    return RedirectToAction("EmailVerification");
                }
                else
                {
                    return RedirectToAction("VerifyEmail", "Home", new { userId = user.Id, code });
                }
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> AddUserToBoard()
        {
            var companyUser = await _userManager.GetUserAsync(HttpContext.User);
            var company = _context.UserCompanies
                                    .Where(x => x.UserId == companyUser.Id)
                                    .Select(s => s.CompanyName)
                                    .ToList()[0];

            var boardIds = _context.UserBoards.Where(x => x.UserId == companyUser.Id).Select(s => s.BoardId).ToList();
            var boards = _context.Boards.Where(x => boardIds.Contains(x.Id)).ToList();

            var userIds = _context.UserCompanies.Where(x => x.CompanyName == company).Select(s => s.UserId).ToList();
            var users = _context.Users.Where(x => userIds.Contains(x.Id)).ToList();

            //ViewBag.UserList = new SelectList(users, "Id", "UserName");
            //ViewBag.BoardList = new SelectList(boards, "Id", "Title");

            return View(new DisplayUserCompaniesModel { Users = users, Boards = boards });
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToBoard(string userId, int boardId)
        {
            _context.UserBoards.Add(new UserBoards { UserId = userId, BoardId = boardId });
            await _context.SaveChangesAsync();
            return RedirectToAction("AddUserToBoard");
        }
    }

    public class DisplayUserCompaniesModel
    {
        public List<IdentityUser> Users { get; set; }
        public List<Board> Boards { get; set; }
    }


    public class DisplayViewModel
    {
        public IEnumerable<string> Users { get; set; }
    }
}