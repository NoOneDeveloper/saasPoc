using Microsoft.AspNetCore.Mvc;

namespace Velzon.Controllers
{
    public class WidgetsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
