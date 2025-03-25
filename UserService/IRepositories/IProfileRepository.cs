using UserService.Models;

namespace UserService.IRepositories;

public interface IProfileRepository
{
    Task<Profile> GetProfileByEmail(string email);
}
