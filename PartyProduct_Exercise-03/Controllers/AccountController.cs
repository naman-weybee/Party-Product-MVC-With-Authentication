using Microsoft.AspNetCore.Mvc;
using PartyProduct_Exercise_03.Models;
using PartyProduct_Exercise_03.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository = null;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
            }
            ModelState.Clear();
            return RedirectToAction("Login");
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.LoginAsync(signInModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Party", "Party");
                }
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return View(signInModel);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.LogoutAsync();
            return RedirectToAction("Party", "Party");
        }

        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangeYourPassword(model);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var errorMessage in result.Errors)
                {
                    ModelState.AddModelError("", errorMessage.Description);
                }
            }
            return View(model);
        }
    }
}
