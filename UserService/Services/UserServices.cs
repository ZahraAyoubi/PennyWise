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

    public async Task<User> Login(string email, string password)
    {
        var user = await _userRepo.Login(email, password);

        return user;
    }
    public async Task<List<User>> GetAll()
    {
        var user = await _userRepo.GetAll();
        return user;
    }
    public async Task<bool> Register(User user)
    {
       if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var existingUser = await _userRepo.GetUserByEmail(user.Email.ToLower());
        if (existingUser != null)
        {
            return false;
        }

        user.Email = user.Email.ToLower();

        await _userRepo.Register(user);

        await _profileRepo.CreateProfile(user);

        return true;
    }
}
