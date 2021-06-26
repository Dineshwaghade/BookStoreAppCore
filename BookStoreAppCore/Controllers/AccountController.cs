﻿using BookStoreAppCore.Models;
using BookStoreAppCore.Repository;
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
        public IActionResult SignUpUser()
        {
            return View();
        }


        [HttpPost]
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
        [HttpGet,Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost,Route("login")]
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
        [HttpGet,Route("changePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost,Route("changePassword")]
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

    }
}
