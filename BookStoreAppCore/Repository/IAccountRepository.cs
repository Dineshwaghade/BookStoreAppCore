using BookStoreAppCore.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStoreAppCore.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel model);
        Task<SignInResult> PasswordAsync(LoginModel model);
        Task SignOutAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task SendEmail_EmailConfirmation(ApplicationUser user, string token);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task<IdentityResult> ConfirmResetPasswordAsync(ResetPasswordModel model);
        Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
        Task SendEmail_ResetPassword(ApplicationUser user, string token);
    }
}