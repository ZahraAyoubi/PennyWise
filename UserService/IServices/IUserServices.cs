using UserService.Models;

namespace UserService.IServices;

public interface IUserServices
{
    Task<ApplicationUser> Register(ApplicationUser user, string password);
    Task<List<ApplicationUser>> GetAll();
    Task<ApplicationUser> Login(string email, string password);
    Task<ApplicationUser> FindUserByEmail(ApplicationUser user);
    Task<string> GenerateResetToken(ApplicationUser user);
    Task<ApplicationUser> SendEmailAsync(ApplicationUser user, string password, string message);
}
