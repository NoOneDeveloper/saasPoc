using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Common.StaticClasses;
namespace Velzon.Controllers
{
    public class HomeController(ICustomerService customer) : Controller
    {
        private readonly ICustomerService _customer = customer;

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> KycForm()
        {
            var user = HttpContext.Session.GetObject<SiginDTO>("UserDto");
            if (user == null)
                return RedirectToAction("SignIn", "Authentication");


            var response = await _customer.GetCustomer(user.Id.Value);

            return View(response.Data);

        }
        [HttpPost]
        public async Task<IActionResult> KycForm(CustomerKycDTO request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var user = HttpContext.Session.GetObject<SiginDTO>("UserDto");
            if (user == null)
                return RedirectToAction("SignIn", "Authentication");

            // 👇 link KYC with logged-in user
            request.UserId = user.Id.Value;
            var reponse = await _customer.AddCustomer(request);
            return Json(new { success = true, message = "Registered Successfuly" });
        }
    }
}
