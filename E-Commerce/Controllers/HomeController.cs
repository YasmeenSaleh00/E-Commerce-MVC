using System.Diagnostics;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            var featured = await _context.Products
                .Where(p => p.IsFeatured && p.ProductStatus == ProductStatus.Available)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Take(8)
                .ToListAsync();

            var categories = await _context.Categories.Take(6).ToListAsync();

            ViewBag.Categories = categories;
            return View(featured);
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
    }
}
