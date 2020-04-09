using Kanbanana.Data;
using Kanbanana.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Controllers
{
    public class UserCompanyController : Controller
    {
        private readonly KanbananaDbContext _context;

        public UserCompanyController(KanbananaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateCompanyConnection(string userId, string companyName)
        {
            _context.UserCompanies.Add(new UserCompany { UserId = userId, CompanyName = companyName });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Company");
        }
    }
}
