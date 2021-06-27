using BookStoreAppCore.Models;
using System.Threading.Tasks;

namespace BookStoreAppCore.Services
{
    public interface IEmailServices
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);
        Task SendEmailForResetPassword(UserEmailOptions userEmailOptions);
    }
}