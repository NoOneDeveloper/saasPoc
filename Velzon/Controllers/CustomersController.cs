using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.Interfaces.IServices.Customer;

namespace Velzon.Controllers
{
    public class CustomersController(ICustomerService customerService) : Controller()
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<IActionResult> Index()
        {
            var customersResponse = await _customerService.ListAsync();
            return View(customersResponse);
        }
    }
}
