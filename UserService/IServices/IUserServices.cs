using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using UserService.Models;

namespace UserService.IServices;

public interface IUserServices
{
    Task<ApplicationUser> Register(ApplicationUser user, string password);
    Task<List<ApplicationUser>> GetAll();
    Task<ApplicationUser> Login(LoginRequest request);
    Task<ApplicationUser> FindUserByEmail(string user);
    Task<string> GenerateResetToken(ApplicationUser user);
    Task SendEmailAsync(ApplicationUser user, string subjcet, string message);
    Task<IdentityResult> ResetPassword(ApplicationUser user, string token, string newPassword);
}
