using Microsoft.AspNetCore.Identity;
using UserService.IRepositories;
using UserService.IServices;
using UserService.Models;

namespace UserService.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepo;
    private readonly IProfileRepository _profileRepo;

    public UserServices(IUserRepository userRepo, IProfileRepository profileRepo)
    {
        _userRepo = userRepo;
        _profileRepo = profileRepo;

    }

    public async Task<ApplicationUser> Login(string email, string password)=>
        await _userRepo.LoginAsync(email, password);

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

    public async Task<ApplicationUser> FindUserByEmail(ApplicationUser user)
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

        return existingUser;
    }

    public Task<ApplicationUser> SendEmailAsync(ApplicationUser user, string password, string message)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateResetToken(ApplicationUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return _userRepo.GeneratePasswordResetTokenAsync(user.Email);
    }
}
