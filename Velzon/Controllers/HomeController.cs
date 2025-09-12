using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.Interfaces.IServices.Customer;

namespace Velzon.Controllers
{
    public class HomeController(ICustomerService customer) : Controller
    {
        private readonly ICustomerService _customer = customer;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> KycForm()
        {
            var response = await _customer.GetCustomer(Guid.Parse("A1B085A9-D7AD-4D51-9EE9-A06058C2355D"));
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> KycForm(CustomerKycDTO request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var reponse = await _customer.AddCustomer(request);
            return Json(new { success = true, message = "Registered Successfuly" });
        }
    }
}
