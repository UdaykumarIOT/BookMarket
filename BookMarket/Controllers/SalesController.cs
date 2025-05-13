using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookMarket.Data;
using BookMarket.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookMarket.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sales.Include(s => s.Title).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var sale = await _context.Sales
                .Include(s => s.Title)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sale == null) return NotFound();

            return View(sale);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var sale = await _context.Sales
                .Include(s => s.Title)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sale == null) return NotFound();

            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }

}
