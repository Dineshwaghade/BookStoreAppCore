using BookStoreAppCore.Models;
using BookStoreAppCore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult SignUpUser()
        {
            return View();
        }

        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> SignUpUser(SignUpUserModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.isSuccess = true;
                }
                else
                {
                    foreach(var ErrMessage in result.Errors)
                    {
                        ModelState.AddModelError("", ErrMessage.Description);
                    }
                    return View(model);
                }
            }
            return View();
        }
        [HttpGet,Route("login"),AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost,Route("login"),AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordAsync(model);
                if(result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Invalid credentials");
            }
            return View(model);
        }
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet,Route("changePassword"),Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost,Route("changePassword"),Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    ViewBag.isSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }
        [HttpGet,Route("Email-Confirm"),AllowAnonymous]
        public async Task<IActionResult> EmailConfirm(string uid,string token)
        {
            token = token.Replace(' ', '+');
            if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                var result=await _accountRepository.ConfirmEmailAsync(uid, token);
                if(result.Succeeded)
                {
                    ViewBag.isSuccess = true;
                }
            }
            return View();
        }
        
        [HttpGet,Route("forgotpassword"),AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost,Route("forgotpassword"),AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            bool result=_accountRepository.ForgotPasswordAsync(model).Result;
            if(result==true)
            {
                ModelState.Clear();
                ViewBag.isSuccess = true;
            }
            return View();
        }
        [HttpGet, Route("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                uid = uid,
                token = token
            };
            return View(model);
        }
        [HttpPost,Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                model.token = model.token.Replace(' ', '+');
                if (!string.IsNullOrEmpty(model.uid) && !string.IsNullOrEmpty(model.token))
                {
                    var result = await _accountRepository.ConfirmResetPasswordAsync(model);
                    if (result.Succeeded)
                    {
                        ModelState.Clear();
                        ViewBag.isSuccess = true;
                    }
                }
            }
            return View();
        }

    }
}
