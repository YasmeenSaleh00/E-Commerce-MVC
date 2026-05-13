using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.DTOs;
using E_Commerce.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
    .Include(p => p.Images)
    .Include(p=>p.Category)
    .ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
     .Include(p => p.Images)
     .Include(p => p.Category)
     .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            Product product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                IsFeatured = dto.IsFeatured,
                ProductStatus = dto.ProductStatus,
                CategoryId = dto.CategoryId,
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                Images = new List<ProductImage>()
            };

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    string extension = Path.GetExtension(image.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Images",
                            "Only image files are allowed.");

                        return View(dto);
                    }
                    string imagePath =
            await ImageHelper.UploadFileAsync(image, "products");

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imagePath
                    });
                
            }
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product created successfully!";

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
       .Include(p => p.Images)
       .Include(p => p.Category)
       .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            CreateProductDto dto = new CreateProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Quantity = product.Quantity,
                IsFeatured = product.IsFeatured,
                ProductStatus = product.ProductStatus,
                ExistingImages = product.Images
    .Select(i => new ExistingImageDto
    {
        Id = i.Id,
        ImageUrl = i.ImageUrl
    })
    .ToList()
            };

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(dto);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProductDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.IsFeatured = dto.IsFeatured;
            product.ProductStatus = dto.ProductStatus;
            product.CategoryId = dto.CategoryId;
            product.ModificationDate = DateTime.Now;

            //delete images
            if (dto.DeletedImageIds != null && dto.DeletedImageIds.Any())
            {
                var imagesToDelete = product.Images
                    .Where(i => dto.DeletedImageIds.Contains(i.Id))
                    .ToList();

                foreach (var image in imagesToDelete)
                {
                    // Delete physical file
                    string fullPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        image.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    // Delete from database
                    _context.ProductImages.Remove(image);
                }
            }

            // Upload new images
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    string extension = Path.GetExtension(image.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Images",
                            "Only image files are allowed.");

                        return View(dto);
                    }
                    string imagePath =
            await ImageHelper.UploadFileAsync(image, "products");

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imagePath
                    });
                
            }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product Updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Images)
     .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Product Deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
