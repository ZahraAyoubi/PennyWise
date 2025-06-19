using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using UserService.IRepositories;
using UserService.IServices;
using UserService.Models;

namespace UserService.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepo;
    private readonly IProfileRepository _profileRepo;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserServices> _logger;

    public UserServices(IUserRepository userRepo,
        IProfileRepository profileRepo, 
        IEmailService emailService,
        ILogger<UserServices> logger)
    {
        _userRepo = userRepo;
        _profileRepo = profileRepo;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<ApplicationUser> Login(LoginRequest request) =>
        await _userRepo.LoginAsync(request);

    public async Task<List<ApplicationUser>> GetAll() =>
        await _userRepo.GetAsync();

    public async Task<ApplicationUser> Register(ApplicationUser user, string password)
    {
       if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var existingUser = await _userRepo.GetUserByEmailAsync(user.Email.ToLower());
        if (existingUser != null)
        {
            return null;
        }

        user.Email = user.Email.ToLower();

        await _userRepo.RegisterAsync(user, password);

        await _profileRepo.CreateProfileAsync(user);

        return user;
    }

    public async Task<ApplicationUser> FindUserByEmail(string email)
    {
        if (email == null)
        {
            throw new ArgumentNullException(nameof(email));
        }

        var existingUser = await _userRepo.GetUserByEmailAsync(email.ToLower());
        if (existingUser == null)
        {
            return null;
        }

        return existingUser;
    }

    public async Task SendEmailAsync(ApplicationUser user, string subject, string message)
    {
        try
        {
            await _emailService.SendEmailAsync(user.Email, "Reset Your Password", message);
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "SMTP error while sending email.");
            throw  ex.InnerException;
        }
    }

    public Task<string> GenerateResetToken(ApplicationUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return _userRepo.GenerateResetTokenAsync(user);
    }

    public async Task<IdentityResult> ResetPassword(ApplicationUser user, string token, string newPassword)
    {    
       return  await _userRepo.ResetPasswordAsync(user, token, newPassword);
    }
}
