using System.Diagnostics;
using BookMarket.Data;
using BookMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string search_text, string author, string publisher)
        {
            var titlesQuery = _context.Titles
                .AsNoTracking()
                .Include(t => t.Author)
                .Include(t => t.Publisher)
                .AsQueryable();

            // Case-insensitive search on title
            if (!string.IsNullOrWhiteSpace(search_text))
            {
                search_text = search_text.Trim();
                var lowered = search_text.ToLower();
                titlesQuery = titlesQuery.Where(t =>
                    EF.Functions.Like(t.Name.ToLower(), $"%{lowered}%"));
            }

            // Case-insensitive filter for author
            if (!string.IsNullOrWhiteSpace(author))
            {
                var loweredAuthor = author.ToLower();
                titlesQuery = titlesQuery.Where(t =>
                    t.Author.AuthName.ToLower() == loweredAuthor);
            }

            // Case-insensitive filter for publisher
            if (!string.IsNullOrWhiteSpace(publisher))
            {
                var loweredPublisher = publisher.ToLower();
                titlesQuery = titlesQuery.Where(t =>
                    t.Publisher.PubName.ToLower() == loweredPublisher);
            }

            var titles = await titlesQuery.ToListAsync();

            await PopulateDropdownsAsync();

            return View(titles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task PopulateDropdownsAsync(object? selectedAuth = null, object? selectedPub = null)
        {
            var authors = await _context.Authors.ToListAsync();
            var publishers = await _context.Publishers.ToListAsync();

            ViewData["AuthId"] = new SelectList(authors, "AuthId", "AuthName", selectedAuth);
            ViewData["PubId"] = new SelectList(publishers, "PubId", "PubName", selectedPub);

            ViewBag.AuthorNames = authors.Select(a => a.AuthName).ToList();
            ViewBag.PublisherNames = publishers.Select(p => p.PubName).ToList();
        }
    }
}
