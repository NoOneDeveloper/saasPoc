using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using Poc.Common.StaticClasses;
using Poc.Infrastructure.DTOs.Global;
using YourWebProject.Filters;
namespace Velzon.Controllers
{
   
    public class CustomersController(ICustomerService customerService) : Controller()
    {
        private readonly ICustomerService _customerService = customerService;
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Index()
        {
            var customersResponse = await _customerService.ListAsync();
            return View(customersResponse);
        }

        [SessionAuthorize("Admin")]
        public async Task<IActionResult> CustomerList()
        {
            var customersResponse = await _customerService.ApprovedCustomersListAsync();
            return View(customersResponse);
        }
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Details(Guid id)
        {
            var customerResponse = await _customerService.GetCustomerAdminDetailsAsync(id);

            // ✅ Agar data hi null hai ya success false hai
            if (customerResponse == null || !customerResponse.Success || customerResponse.Data == null)
            {
                return NotFound(new { success = false, message = customerResponse?.Message ?? "Customer not found" });
            }

            // ✅ Data ko View mein bhejna
            return View(customerResponse.Data);
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

        [HttpGet]
        public async Task<IActionResult> CustomerDetails()
        {
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            if (loggedInUser == null)
                return Unauthorized(new { success = false, message = "Session expired, please login again" });

            var guidId = Guid.Parse(loggedInUser.Id.ToString());

            var customerResponse = await _customerService.GetCustomerAdminDetailsAsync(guidId);

            if (!customerResponse.Success || customerResponse.Data == null)
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


        [HttpPost]
        public async Task<IActionResult> ToggleProofStatus([FromBody] StatusUpdateDTO input)
        {
            var proofId = Guid.Parse(input.Id);

            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");
            if (loggedInUser == null)
            {
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            }

            var result = await _customerService.UpdateProofStatus(proofId, input.Status, loggedInUser.Id.Value);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = "Status updated successfully" });
        }

        public IActionResult DropdownValues()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProofActivity([FromBody] ProofofBusinessActivityDTO input)
        {
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");
            if (loggedInUser == null)
                return Unauthorized(new { success = false, message = "Session expired" });

            var proof = await _customerService.GetProofById(input.Id);
            if (proof == null) return BadRequest(new { success = false, message = "Proof not found" });

            input.CustomerId = proof.CustomerId;
            input.Type = proof.Type;
            input.ModifiedBy = loggedInUser.Id.Value;
            input.CreatedDate = DateTime.UtcNow;

            await _customerService.AddProofActivity(input);
            await _customerService.UpdateProofStatus(proof.Id, input.Status.Value, loggedInUser.Id.Value);

            return Ok(new { success = true });
        }


    }
}
