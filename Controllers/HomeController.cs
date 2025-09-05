using Microsoft.AspNetCore.Mvc;

namespace Velzon.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult LandingPage()
        {
            return View();
        }
    }
}
