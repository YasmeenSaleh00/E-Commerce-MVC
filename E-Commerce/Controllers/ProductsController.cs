using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchString, int? categoryId)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(p => p.Name.Contains(searchString));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(await query.OrderByDescending(p => p.CreationDate).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            var related = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id)
                .Include(p => p.Images)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = related;
            return View(product);
        }

        [HttpGet("Products/FilterByCategory/{categoryId}")]
        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            var category = await _context.Categories.FindAsync(categoryId);
            ViewBag.CategoryName = category?.Name;
            return View("Index", products);
        }
    }
}
