using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.Requirement;
using Poc.Infrastructure.DTOs.Services;
using Poc.Infrastructure.Interfaces.IServices.IRequirementServices;

namespace Velzon.Controllers
{
    public class ServicesController(IRequirementService service) : Controller
    {
        private readonly IRequirementService _service = service;

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var paymentGateways = await _service.GetPaymentGateways();
            return View(paymentGateways);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ServicesRequestDTO input)
        {
            if (!ModelState.IsValid)
            {
                var paymentGateways = await _service.GetPaymentGateways();
                return View(paymentGateways);
            }

            Guid guidId = Guid.Parse(input.PaymentGateway);

            var request = await _service.GetInputData(guidId);

            return View("Fields", request.Data);
        }


        [HttpGet]
        public IActionResult Fields(ServicesRequestDTO input)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Fields(List<InputDataResponseDTO> input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            // Filter selected inputs and order by OrderIndex
            var selectedInputs = input.Where(x => x.IsSelected).OrderBy(x => x.OrderIndex).ToList();

            TempData["AlpacaJsonData"] = System.Text.Json.JsonSerializer.Serialize(selectedInputs);

            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
