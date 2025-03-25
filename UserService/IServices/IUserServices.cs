using UserService.Models;

namespace UserService.IServices;

public interface IUserServices
{
    Task<bool> Register(User user);
    Task<List<User>> GetAll();
    Task<User> Login(string email, string password);
}
