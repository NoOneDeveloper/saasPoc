using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poc.Infrastructure.DTOs.SigninDTO;
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


        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View(new SiginDTO()); 
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SignIn")]
        public async  Task<IActionResult> SignIn(SiginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _customerService.ValidateCustomerAsync(model);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid email or password";
                return View(model);
            }
         


            HttpContext.Session.SetString("UserEmail", user.Email);
            string userType = user.Email.EndsWith("@pcipal.com") ? "Admin" : "Customer";
            HttpContext.Session.SetString("UserType", userType);
            TempData["LoginSuccess"] = "Welcome! You have successfully logged in.";
            return RedirectToAction("Index", "Dashboard");
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
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUpBasic()
        {
            return View(new SignUpRequestDTO());
        }
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SignUpBasic")]
        public  async Task<IActionResult> SignUpBasic(SignUpRequestDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _customerService.CreateCustomerAsync(model);
            TempData["LoginSuccess"] = $"Welcome {model.Email}! You have successfully signed in.";
            return RedirectToAction("SignIn", "Authentication");

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
