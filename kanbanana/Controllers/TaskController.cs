using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Data;
using Kanbanana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Kanbanana.Controllers
{
    public class TaskController : Controller
    {
        private readonly KanbananaDbContext _context;

        public TaskController(
            KanbananaDbContext context)
        {
            _context = context;
        }



        public IActionResult Create(int? columnId, int boardId)
        {
            if (columnId == null)
                return NotFound();
            return View(new List<int>(){ (int)columnId, boardId});
        }

        [HttpPost]
        public async Task<IActionResult> Create(int columnId, int boardId, string title, string description)
        {
            await _context.Tasks.AddAsync(new Kanbanana.Models.Task {
                                            Title = title,
                                            Description = description,
                                            ColumnId = columnId,
                                            BoardId = boardId });
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Board", new { id = boardId });
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            var board = await _context.Boards
                .Include(e => e.Columns)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == task.BoardId);

            return View(new DisplayEditTask { Task = task, Columns = board.Columns });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, int columnId, int boardId, string title, string description)
        {
            try
            {
                _context.Tasks.Update(new Kanbanana.Models.Task {
                                        Id = id,
                                        ColumnId = columnId,
                                        BoardId = boardId,
                                        Title = title,
                                        Description = description});
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Board", new { id = boardId });
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var task = await _context.Tasks
                .Include(e => e.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
                return NotFound();
            return View(task);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfimed(int? id, int boardId)
        {
            if (id == null)
                return NotFound();
            var column = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(column);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Board", new { id = boardId });
        }
    }
}