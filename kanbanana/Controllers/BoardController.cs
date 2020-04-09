using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Data;
using Kanbanana.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Kanbanana.Controllers
{
    [Authorize(Policy = "Employee")]
    public class BoardController : Controller
    {
        private readonly KanbananaDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public BoardController(
            KanbananaDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var boardIds = await _context.UserBoards
                .Where(x => x.UserId == user.Id)
                .Select(y => y.BoardId)
                .ToListAsync();

            var boards = await _context.Boards
                .Where(b => boardIds.Contains(b.Id))
                .ToListAsync();

            // !!! Move to less visited code
            var result = await _authorizationService.AuthorizeAsync(User, "Company");
            if (boards.Count == 0 && result.Succeeded)
                return RedirectToAction("CreateBoard", new { title = "First Board" });

            return View(boards);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string title)
        {
            await _context.Boards.AddAsync(new Board { Title = title });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateBoard(string title)
        {
            var board = new Board
            {
                Title = title
            };

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            return RedirectToAction(
                "CreateBoardConnection",
                "UserBoard",
                new { userId = user.Id, boardId = board.Id });
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var board = await _context.Boards
                .Include(e => e.Columns)
                .ThenInclude(f => f.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (board == null)
                return NotFound();
            return View(board);
        }
    }
}