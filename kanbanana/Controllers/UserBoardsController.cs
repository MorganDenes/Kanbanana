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
    public class UserBoardsController : Controller
    {
        private readonly KanbananaDbContext _context;

        public UserBoardsController(KanbananaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateBoardConnection(string userId, int boardId)
        {
            UserBoards board = new UserBoards
            {
                UserId = userId,
                BoardId = boardId
            };

            // Security issues; could require company ID and check for it
            _context.UserBoards.Add(board);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Board");
        }
    }
}