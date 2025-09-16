using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using Poc.Common.StaticClasses;
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


        public async Task<IActionResult> CustomerList()
        {
            var customersResponse = await _customerService.ListAsync();
            return View(customersResponse);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var customerDetail = await _customerService.GetCustomerDetailsAsync(id);
            if (customerDetail == null)
            {
                return NotFound();
            }
            return View(customerDetail);
        }

      


        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] StatusUpdateDTO input)
        {
            var guidId = Guid.Parse(input.Id);

       
            var customerResponse = await _customerService.GetCustomer(guidId);
            if (!customerResponse.Success)
            {
                return BadRequest(new { success = false, message = customerResponse.Message });
            }

            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");
            if (loggedInUser == null)
            {
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            }

            var updateResult = await _customerService.UpdateCustomerStatus(
                guidId,
                input.Status,
                loggedInUser.Id.Value
            );
            if (!updateResult.Success)
            {
                return BadRequest(new { success = false, message = updateResult.Message });
            }

            return Ok(new { success = true, message = "Status updated successfully" });
        }

    }
}
