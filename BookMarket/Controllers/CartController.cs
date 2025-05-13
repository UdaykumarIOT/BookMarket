using BookMarket.Data;
using BookMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    // For demo/testing only. Replace with session-based cart in production.
    private static readonly List<CartItem> Cart = new();

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var titleIds = Cart.Select(c => c.TitleId).ToList();
        var titles = _context.Titles
            .Where(t => titleIds.Contains(t.TitleId))
            .ToDictionary(t => t.TitleId);

        foreach (var item in Cart)
        {
            if (titles.TryGetValue(item.TitleId, out var title))
                item.Title = title;
        }

        return View(Cart);
    }

    public IActionResult Add(Guid id)
    {
        var item = Cart.FirstOrDefault(c => c.TitleId == id);
        if (item != null)
        {
            item.Quantity++;
        }
        else
        {
            var title = _context.Titles.Find(id);
            if (title != null)
            {
                Cart.Add(new CartItem { TitleId = id, Quantity = 1, Title = title });
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult Remove(Guid id)
    {
        var item = Cart.FirstOrDefault(c => c.TitleId == id);
        if (item != null)
        {
            Cart.Remove(item);
        }

        return RedirectToAction("Index");
    }

    public IActionResult Checkout()
    {
        var titleIds = Cart.Select(c => c.TitleId).ToList();
        var titles = _context.Titles
            .Where(t => titleIds.Contains(t.TitleId))
            .ToDictionary(t => t.TitleId, t => t.Price);

        var total = Cart.Sum(c => c.Quantity * (titles.TryGetValue(c.TitleId, out var price) ? price : 0));
        return View(total);
    }

    [HttpPost]
    public IActionResult CheckoutConfirm()
    {
        var titleIds = Cart.Select(c => c.TitleId).ToList();
        var titles = _context.Titles
            .Where(t => titleIds.Contains(t.TitleId))
            .ToDictionary(t => t.TitleId, t => t.Price);

        var total = Cart.Sum(c => c.Quantity * (titles.TryGetValue(c.TitleId, out var price) ? price : 0));

        var order = new Order
        {
            Items = Cart.Select(c => new CartItem
            {
                TitleId = c.TitleId,
                Quantity = c.Quantity
            }).ToList(),
            TotalAmount = total,
            OrderDate = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        Cart.Clear();
        return View("OrderSuccess", order);
    }
}
