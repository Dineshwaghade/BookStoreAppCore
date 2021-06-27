using BookStoreAppCore.Models;
using BookStoreAppCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserServices _userServices;
        private readonly IEmailServices _emailServices;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
            IUserServices userServices,IEmailServices emailServices,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userServices = userServices;
            _emailServices = emailServices;
            _configuration = configuration;
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel model)
        {
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if(!string.IsNullOrEmpty(token))
                {
                    await SendEmail_EmailConfirmation(user,token);
                }
            }
            return result;
        }
        public async Task<SignInResult> PasswordAsync(LoginModel model)
        {
            var result=await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            return result;
        }
        public async Task SignOutAsync()
        {
           await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userServices.GetUserId();
            var user =await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }
        public async Task SendEmail_EmailConfirmation(ApplicationUser user, string token)
        {
            string AppDomain = _configuration["Application:AppDomain"];
            string link = _configuration["Application:EmailConfirm"];
            UserEmailOptions options = new UserEmailOptions()
            {
                toEmails = new List<string>() { user.Email },
                Placeholder = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{Username}}",user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(AppDomain+link,user.Id,token))
                }
            };
            await _emailServices.SendEmailForEmailConfirmation(options);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string uid,string token)
        {
           return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
            
        }
        public async Task<IdentityResult> ConfirmResetPasswordAsync(ResetPasswordModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.uid), model.token, model.NewPassword);
        }

        public async Task SendEmail_ResetPassword(ApplicationUser user, string token)
        {
            string AppDomain = _configuration["Application:AppDomain"];
            string link = _configuration["Application:ResetPassword"];
            UserEmailOptions options = new UserEmailOptions()
            {
                toEmails = new List<string>() { user.Email },
                Placeholder = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{Username}}",user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(AppDomain+link,user.Id,token))
                }
            };
            await _emailServices.SendEmailForResetPassword(options);
        }
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            ApplicationUser user = new ApplicationUser();
            user=await _userManager.FindByEmailAsync(model.Email);
            if(user!=null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await SendEmail_ResetPassword(user, token);
                return true;
            }
            return false;
        }
    }
}
