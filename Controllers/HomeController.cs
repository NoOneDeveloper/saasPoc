using Microsoft.AspNetCore.Mvc;

namespace Velzon.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult KycForm()
        {
            return View();
        }
    }
}
