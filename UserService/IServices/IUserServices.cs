using UserService.Models;

namespace UserService.IServices;

public interface IUserServices
{
    Task<User> Register(User user);
    Task<List<User>> GetAll();
    Task<User> Login(string email, string password);
}
