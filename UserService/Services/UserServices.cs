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

    public async Task<User> Login(string email, string password)=>
        await _userRepo.LoginAsync(email, password);

    public async Task<List<User>> GetAll() =>
        await _userRepo.GetAsync();

    public async Task<User> Register(User user)
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

        await _userRepo.RegisterAsync(user);

        await _profileRepo.CreateProfileAsync(user);

        return user;
    }
}
