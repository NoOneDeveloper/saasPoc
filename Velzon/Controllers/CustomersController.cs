using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using YourWebProject.Filters;

namespace Velzon.Controllers
{
    [SessionAuthorize("Admin")]
    public class CustomersController(ICustomerService customerService) : Controller()
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<IActionResult> Index()
        {
            var customersResponse = await _customerService.ListAsync();
            return View(customersResponse);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] StatusUpdateDTO input)
        {
            var guidId = Guid.Parse(input.Id);
            var customerResponse = await _customerService.GetCustomer(guidId);
            if (!customerResponse.Success)
            {
                TempData["ErrorMessage"] = customerResponse.Message;
                return RedirectToAction("Index");
            }
            return View(customerResponse.Data);
        }
    }
}
