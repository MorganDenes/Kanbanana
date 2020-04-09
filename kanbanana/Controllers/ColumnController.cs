using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Data;
using Kanbanana.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace Kanbanana.Controllers
{
    public class ColumnController : Controller
    {
        private readonly KanbananaDbContext _context;

        public ColumnController(
            KanbananaDbContext context)
        {
            _context = context;
        }



        public IActionResult Create(int? boardId)
        {
            if (boardId == null)
                return NotFound();
            return View(boardId);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int boardId, string title)
        {
            await _context.Columns.AddAsync(new Column { Title = title, BoardId = boardId });
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Board", new { id = boardId });
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var column = await _context.Columns.FindAsync(id);
            if (column == null)
                return NotFound();
            return View(column);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, int boardId, string title)
        {
            try
            {
                _context.Columns.Update(new Column { Id = id, BoardId = boardId, Title = title });
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Board", new { id =boardId });
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var column = await _context.Columns.FindAsync(id);
            if (column == null)
                return NotFound();
            return View(column);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfimed(int? id, int boardId)
        {
            if (id == null)
                return NotFound();
            var column = await _context.Columns.FindAsync(id);
            _context.Columns.Remove(column);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Board", new { id = boardId });
        }
    }
}