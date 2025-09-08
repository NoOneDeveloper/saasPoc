using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.SinginUpDTO;
using Poc.Infrastructure.Interfaces.IServices.Customer;

namespace Velzon.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ICustomerService _customerService;
        public AuthenticationController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [ActionName("SignInBasic")]
        public IActionResult SignInBasic()
        {
            return View();
        }

        [ActionName("SignInCover")]
        public IActionResult SignInCover()
        {
            return View();
        }
        [ActionName("Waitingpage")]
        public IActionResult Waitingpage()
        {
            return View();
        }
        [ActionName("SignUpBasic")]
        public  async Task<IActionResult> SignUpBasic(SignUpRequestDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _customerService.CreateCustomerAsync(model);//ali
            return RedirectToAction("Success");
        }

        [ActionName("SignUpCover")]
        public IActionResult SignUpCover()
        {
            return View();
        }

        [ActionName("PasswordChangeBasic")]
        public IActionResult PasswordChangeBasic()
        {
            return View();
        }

        [ActionName("PasswordChangeCover")]
        public IActionResult PasswordChangeCover()
        {
            return View();
        }

        [ActionName("PasswordResetBasic")]
        public IActionResult PasswordResetBasic()
        {
            return View();
        }

        [ActionName("PasswordResetCover")]
        public IActionResult PasswordResetCover()
        {
            return View();
        }

        [ActionName("LockScreenBasic")]
        public IActionResult LockScreenBasic()
        {
            return View();
        }

        [ActionName("LockScreenCover")]
        public IActionResult LockScreenCover()
        {
            return View();
        }

        [ActionName("LogoutBasic")]
        public IActionResult LogoutBasic()
        {
            return View();
        }

        [ActionName("LogoutCover")]
        public IActionResult LogoutCover()
        {
            return View();
        }

        [ActionName("SuccessMessageBasic")]
        public IActionResult SuccessMessageBasic()
        {
            return View();
        }

        [ActionName("SuccessMessageCover")]
        public IActionResult SuccessMessageCover()
        {
            return View();
        }

        [ActionName("TwoStepVerificationBasic")]
        public IActionResult TwoStepVerificationBasic()
        {
            return View();
        }

        [ActionName("TwoStepVerificationCover")]
        public IActionResult TwoStepVerificationCover()
        {
            return View();
        }

        [ActionName("Errors404Basic")]
        public IActionResult Errors404Basic()
        {
            return View();
        }

        [ActionName("Errors404Cover")]
        public IActionResult Errors404Cover()
        {
            return View();
        }

        [ActionName("Errors404Alt")]
        public IActionResult Errors404Alt()
        {
            return View();
        }

        [ActionName("Errors500")]
        public IActionResult Errors500()
        {
            return View();
        }

        [ActionName("Offline")]
        public IActionResult Offline()
        {
            return View();
        }

    }
}
