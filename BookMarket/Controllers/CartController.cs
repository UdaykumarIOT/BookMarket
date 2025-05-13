using System.Security.Principal;
using BookMarket.Data;
using BookMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BookMarket.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static readonly List<CartItem> Cart = new();
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "user")]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var titleIds = Cart.Where(c => c.UserId == userId).Select(c => c.TitleId).ToList();
            var titles = _context.Titles
                .Where(t => titleIds.Contains(t.TitleId))
                .ToDictionary(t => t.TitleId);

            foreach (var item in Cart)
            {
                if (titles.TryGetValue(item.TitleId, out var title))
                    item.Title = title;
            }

            return View(Cart.Where(c => c.UserId == userId).ToList());

        }
        [Authorize(Roles = "user")]
        public IActionResult Add(Guid id)
        {
            var userId = _userManager.GetUserId(User);

            var item = Cart.FirstOrDefault(c => c.TitleId == id && c.UserId == userId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                var title = _context.Titles.Find(id);
                if (title != null)
                {
                    Cart.Add(new CartItem
                    {
                        TitleId = id,
                        Quantity = 1,
                        UserId = userId,
                        Title = title
                    });
                }
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "user")]
        public IActionResult Remove(Guid id)
        {
            var userId = _userManager.GetUserId(User);

            var item = Cart.FirstOrDefault(c => c.TitleId == id && c.UserId == userId);
            if (item != null)
            {
                Cart.Remove(item);
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "user")]
        public IActionResult Checkout()
        {
            var userId = _userManager.GetUserId(User);

            var titleIds = Cart.Where(c => c.UserId == userId).Select(c => c.TitleId).ToList();
            var titles = _context.Titles
                .Where(t => titleIds.Contains(t.TitleId))
                .ToDictionary(t => t.TitleId, t => t.Price);

            var total = Cart
                .Where(c => c.UserId == userId)
                .Sum(c => c.Quantity * (titles.TryGetValue(c.TitleId, out var price) ? price : 0));

            return View(total);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CheckoutConfirmAsync()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userCartItems = Cart
                .Where(c => c.UserId == userId && c.Quantity > 0 && c.TitleId != Guid.Empty)
                .ToList();

            if (!userCartItems.Any())
                return BadRequest("Cart is empty or invalid.");

            var titlePrices = _context.Titles
                .Where(t => userCartItems.Select(c => c.TitleId).Contains(t.TitleId))
                .ToDictionary(t => t.TitleId, t => t.Price);

            var total = userCartItems
                .Sum(c => c.Quantity * (titlePrices.TryGetValue(c.TitleId, out var price) ? price : 0));

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = total,
                Items = userCartItems.Select(c => new OrderItem
                {
                    TitleId = c.TitleId,
                    Quantity = c.Quantity,
                    UserId = userId
                }).ToList()
            };

            var sales = userCartItems.Select(c => new Sale
            {
                Id = Guid.NewGuid(),
                TitleId = c.TitleId,
                UserId = userId,
                Quantity = c.Quantity,
                TotalPrice = c.Quantity * (titlePrices.TryGetValue(c.TitleId, out var price) ? price : 0),
                SaleDate = DateTime.UtcNow
            }).ToList();

            try
            {
                _context.Orders.Add(order);
                _context.Sales.AddRange(sales);
                await _context.SaveChangesAsync();

                Cart.RemoveAll(c => c.UserId == userId);
                return View("OrderSuccess", order);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while saving the order and sales: " + ex.Message);
            }
        }

    }
}