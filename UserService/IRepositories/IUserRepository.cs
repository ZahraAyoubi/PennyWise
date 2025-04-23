using UserService.Models;

namespace UserService.IRepositories;

public interface IUserRepository
{
    Task<User> RegisterAsync(User user, CancellationToken cancellationToken = default);  
    Task<List<User>> GetAsync(CancellationToken cancellationToken = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
