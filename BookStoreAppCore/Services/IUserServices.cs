namespace BookStoreAppCore.Services
{
    public interface IUserServices
    {
        string GetUserId();
        bool isAuthenticated();
    }
}