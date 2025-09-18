using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using Poc.Common.StaticClasses;
using Poc.Infrastructure.DTOs.Global;
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

        public async Task<IActionResult> Details()
        {
            var customersResponse = await _customerService.ListAsync();
            return View(customersResponse);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] StatusUpdateDTO input)
        {
            var guidId = Guid.Parse(input.Id);

            // ✅ Pehle customer verify kar lo
            var customerResponse = await _customerService.GetCustomer(guidId);
            if (!customerResponse.Success)
            {
                return BadRequest(new { success = false, message = customerResponse.Message });
            }

            // ✅ Logged-in user (Session se uthao)
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");
            if (loggedInUser == null)
            {
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            }

            // ✅ Status update call
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

        [HttpGet]
        public async Task<IActionResult> CustomerDetails()
        {
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            if (loggedInUser == null)
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            
            var guidId = Guid.Parse(loggedInUser.Id.ToString());

            var customerResponse = await _customerService.GetCustomerDetailsAsync(guidId);
            if (!customerResponse.Success)
            {
                return RedirectToAction("CustomerList");
            }
            return View(customerResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CustomerDetails(ChangeFileDTO input)
        {
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            if (loggedInUser == null)
                return Unauthorized(new { success = false, message = "Session expired, please login again" });

            var response = await _customerService.changeFileAsync(loggedInUser.Id.Value, input);
            if (!response.Success)
            {
                var model = new Result<CustomerDetailDTO>();
                return View(model);
            }
            return Json(new { success = true, message = "Document updated successfully" });
        }

    }
}
