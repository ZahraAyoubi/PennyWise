using UserService.Models;

namespace UserService.IRepositories;

public interface IUserRepository
{
    Task<bool> Register(User user);
    Task<bool> CreateProfile(User user);
    Task<List<User>> GetAll();
    Task<User> GetUserByEmail(string email);
    Task<User> Login(string email, string password);
}
