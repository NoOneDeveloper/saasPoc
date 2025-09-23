using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourWebProject.Filters;

namespace Velzon.Controllers
{

    [SessionAuthorize("Customer")]
    public class DashBoardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
      

    }
}
