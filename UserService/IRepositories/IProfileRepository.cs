using UserService.Models;

namespace UserService.IRepositories;

public interface IProfileRepository
{
    Task<bool> CreateProfile(User user);
    Task<Profile> GetProfileByEmail(string email);
    Task<Profile> GetProfileByyId(Guid id);
    Task<bool> UpdateProfile(Profile profile);
}
