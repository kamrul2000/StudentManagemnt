using StudentManagemnt.Models;

namespace StudentManagemnt.Interface
{
    public interface IAuthService
    {
        Task<User> Register(User user, string password);
        Task<string?> Login(string username, string password);
    }
}
