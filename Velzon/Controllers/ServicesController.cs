using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Poc.Common.StaticClasses;
using Poc.Infrastructure.DTOs.Requirement;
using Poc.Infrastructure.DTOs.Services;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.Interfaces.IServices.IRequirementServices;

namespace Velzon.Controllers
{
    public class ServicesController(IRequirementService service) : Controller
    {
        private readonly IRequirementService _service = service;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            if (loggedInUser == null)
            {
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            }

            var paymentGateways = await _service.GetPaymentGateways(loggedInUser.Id.Value);

            return View(paymentGateways);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ServicesRequestDTO input)
        {
            if (!ModelState.IsValid)
            {
                var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

                if (loggedInUser == null)
                {
                    return Unauthorized(new { success = false, message = "Session expired, please login again" });
                }

                var paymentGateways = await _service.GetPaymentGateways(loggedInUser.Id.Value);
                return View(paymentGateways);
            }

            Guid guidId = Guid.Parse(input.PaymentGateway);

            var request = await _service.GetInputData(guidId);
            request.Data = request.Data.OrderByDescending(x => x.IsSelected).ToList();
            return View("Fields", request.Data);
        }


        [HttpGet]
        public IActionResult Fields(ServicesRequestDTO input)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Fields(List<InputDataResponseDTO> input)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(input);
            //}

            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            if (loggedInUser == null)
            {
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            }
            var SelectedInput = input.Where(x => x.IsSelected).ToList();

            var request = await _service.AddCustomerFlow(loggedInUser.Id.Value, SelectedInput);

            // Filter selected inputs and order by OrderIndex
            //var selectedInputs = input.Where(x => x.IsSelected).OrderBy(x => x.OrderIndex).ToList();

            //var Json = System.Text.Json.JsonSerializer.Serialize(selectedInputs);

            //TempData["AlpacaJsonData"] = Json;

            return RedirectToAction("Flow");
        }

        [HttpGet]
        public async Task<IActionResult> Flow()
        {
            var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            if (loggedInUser == null)
            {
                return Unauthorized(new { success = false, message = "Session expired, please login again" });
            }

            var request = await _service.GetCustomerFlow(loggedInUser.Id.Value);

            var selectedInputs = request.Data;

            var Json = System.Text.Json.JsonSerializer.Serialize(selectedInputs);

            ViewData["AlpacaJsonData"] = Json;

            return View(Request);
        }

        public PartialViewResult FieldsPartialView()
        {
            //var loggedInUser = HttpContext.Session.GetObject<SiginDTO>("UserDto");

            //if (loggedInUser == null)
            //{
            //    return Unauthorized(new { success = false, message = "Session expired, please login again" });
            //}

            //var request = await _service.GetCustomerFlow(loggedInUser.Id.Value);

            //var selectedInputs = request.Data;

            //var Json = System.Text.Json.JsonSerializer.Serialize(selectedInputs);

            //ViewData["AlpacaJsonData"] = Json;

            return PartialView("_FieldsPartialView");
        }


        public PartialViewResult ResultPartialView()
        {
            return PartialView("_ResultPartialView");
        }
        public PartialViewResult ProgressPartialView()
        {
            return PartialView("_ProgressPartialView");
        }
        public PartialViewResult CheckoutPartialView()
        {
            return PartialView("_CheckoutPartialView");
        }

        public PartialViewResult PaymentMethodPartial()
        {
            return PartialView("_PaymentMethodPartial");
        }

        public PartialViewResult OptionsPartialView()
        {
            return PartialView("_OptionsPartialView");
        }
    }
}
