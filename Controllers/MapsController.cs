using Microsoft.AspNetCore.Mvc;

namespace Velzon.Controllers
{
    public class MapsController : Controller
    {

        [ActionName("Google")]
        public IActionResult Google()
        {
            return View();
        }

        [ActionName("Vector")]
        public IActionResult Vector()
        {
            return View();
        }

        [ActionName("Leaflet")]
        public IActionResult Leaflet()
        {
            return View();
        }

    }
}
