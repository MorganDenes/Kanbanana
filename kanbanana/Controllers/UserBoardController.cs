using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kanbanana.Models;
using Kanbanana.Data;
using Microsoft.AspNetCore.Authorization;

namespace Kanbanana.Controllers
{
    [Authorize(Policy = "Company")]
    public class UserBoardController : Controller
    {
        private readonly KanbananaDbContext _context;

        public UserBoardController(KanbananaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateBoardConnection(string userId, int boardId)
        {
            _context.UserBoards.Add(new UserBoards { UserId = userId, BoardId = boardId });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Board");
        }
    }
}