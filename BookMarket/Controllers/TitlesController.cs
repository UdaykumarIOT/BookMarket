using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookMarket.Data;
using BookMarket.Models;

namespace BookMarket.Controllers
{
    public class TitlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TitlesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Titles
        public async Task<IActionResult> Index()
        {
            var titles = _context.Titles.Include(t => t.Author).Include(t => t.Publisher);
            return View(await titles.ToListAsync());
        }

        // GET: Titles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var title = await _context.Titles
                .Include(t => t.Author)
                .Include(t => t.Publisher)
                .FirstOrDefaultAsync(m => m.TitleId == id);
            if (title == null) return NotFound();

            return View(title);
        }

        // GET: Titles/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Titles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,type,AuthId,Notes,ImageUrl,Price,PubId,PubDate")] Title title)
        {
            var uploadedFiles = HttpContext.Request.Form.Files;
            if (uploadedFiles.Count > 0)
            {
                var extension = Path.GetExtension(uploadedFiles[0].FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageUrl", "Invalid image format. Allowed: jpg, jpeg, png, gif.");
                    PopulateDropdowns(title.AuthId, title.PubId);
                    return View(title);
                }

                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "book");
                Directory.CreateDirectory(uploadPath);
                var newFileName = $"{Guid.NewGuid()}{extension}";

                using var fileStream = new FileStream(Path.Combine(uploadPath, newFileName), FileMode.Create);
                await uploadedFiles[0].CopyToAsync(fileStream);
                title.ImageUrl = $"/image/book/{newFileName}";
            }

            if (ModelState.IsValid)
            {
                title.TitleId = Guid.NewGuid();
                _context.Add(title);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(title.AuthId, title.PubId);
            return View(title);
        }

        // GET: Titles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var title = await _context.Titles.FindAsync(id);
            if (title == null) return NotFound();

            PopulateDropdowns(title.AuthId, title.PubId);
            return View(title);
        }

        // POST: Titles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TitleId,Name,type,AuthId,Notes,ImageUrl,Price,PubId,PubDate")] Title title)
        {
            var uploadedFiles = HttpContext.Request.Form.Files;

            if (uploadedFiles.Count > 0)
            {
                var extension = Path.GetExtension(uploadedFiles[0].FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageUrl", "Invalid image format. Allowed: jpg, jpeg, png, gif.");
                    PopulateDropdowns(title.AuthId, title.PubId);
                    return View(title);
                }

                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "book");
                Directory.CreateDirectory(uploadPath);
                var newFileName = $"{Guid.NewGuid()}{extension}";

                var oldImage = await _context.Titles.AsNoTracking().FirstOrDefaultAsync(x => x.TitleId == id);
                if (oldImage != null && !string.IsNullOrEmpty(oldImage.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldImage.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                using var fileStream = new FileStream(Path.Combine(uploadPath, newFileName), FileMode.Create);
                await uploadedFiles[0].CopyToAsync(fileStream);
                title.ImageUrl = $"/image/book/{newFileName}";
            }
            else
            {
                var oldImage = await _context.Titles.AsNoTracking().FirstOrDefaultAsync(x => x.TitleId == id);
                if (oldImage != null)
                {
                    title.ImageUrl = oldImage.ImageUrl;
                }
            }

            if (id != title.TitleId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(title);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitleExists(title.TitleId)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(title.AuthId, title.PubId);
            return View(title);
        }

        // GET: Titles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var title = await _context.Titles
                .Include(t => t.Author)
                .Include(t => t.Publisher)
                .FirstOrDefaultAsync(m => m.TitleId == id);
            if (title == null) return NotFound();

            return View(title);
        }

        // POST: Titles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var title = await _context.Titles.FindAsync(id);
            if (title != null)
            {
                if (!string.IsNullOrEmpty(title.ImageUrl))
                {
                    var imgPath = Path.Combine(_webHostEnvironment.WebRootPath, title.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(imgPath))
                        System.IO.File.Delete(imgPath);
                }
                _context.Titles.Remove(title);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TitleExists(Guid id)
        {
            return _context.Titles.Any(e => e.TitleId == id);
        }

        private void PopulateDropdowns(object? selectedAuth = null, object? selectedPub = null)
        {
            ViewData["AuthId"] = new SelectList(_context.Authors, "AuthId", "AuthName", selectedAuth);
            ViewData["PubId"] = new SelectList(_context.Publishers, "PubId", "PubName", selectedPub);
        }
    }
}
