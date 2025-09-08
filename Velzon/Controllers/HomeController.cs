using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs;

namespace Velzon.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult KycForm()
        {
            var model = new CustomerDTO();
            return View(model);
        }
    }
}
