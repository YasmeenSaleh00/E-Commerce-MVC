using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestimonialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string status)
        {
            var data = _context.Testimonials.Include(x => x.User).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "pending")
                    data = data.Where(x => x.Status == TestimonialStatus.Pending);

                if (status == "approved")
                    data = data.Where(x => x.Status == TestimonialStatus.Approved);

                if (status == "rejected")
                    data = data.Where(x => x.Status == TestimonialStatus.Rejected);
            }

            return View(data.ToList());
        }

        public async Task<IActionResult> Approve(int id)
        {
            var item = await _context.Testimonials.FindAsync(id);
            item.Status = TestimonialStatus.Approved;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var item = await _context.Testimonials.FindAsync(id);
            item.Status = TestimonialStatus.Rejected;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
