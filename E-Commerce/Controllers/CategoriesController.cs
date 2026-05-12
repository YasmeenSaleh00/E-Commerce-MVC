using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Helpers;

namespace E_Commerce.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var categoryQuery = _context.Categories
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                categoryQuery = categoryQuery.Where(p =>
                    p.Name.Contains(searchTerm) 
                    );
            }

            return View(await categoryQuery.ToListAsync());
        }


        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

      
      

      

    
    }
}
