using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Data;
using Kanbanana.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kanbanana.Controllers
{
    public class CommentController : Controller
    {
        private readonly KanbananaDbContext _context;

        public CommentController(
            KanbananaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string comment)
        {
            await _context.Comments.AddAsync(new Comment { TaskId = taskId, Text = comment });
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Task", new { id = taskId });
        }
    }
}