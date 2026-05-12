using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class Pages : Controller
    {
        public IActionResult About()
        {
            ViewData["Title"] = "About Us";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy & Policy";
            return View();
        }
    }
}
