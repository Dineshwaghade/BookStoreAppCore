using BookStoreAppCore.Models;
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
            }
            return View();
        }
    }
}
