using UserService.Models;

namespace UserService.IRepositories;

public interface IProfileRepository
{
    Task<bool> CreateProfileAsync(User user, CancellationToken cancellationToken = default);
    Task<Profile> GetProfileByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Profile> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UpdateProfileAsync(Profile profile, CancellationToken cancellationToken = default);
}
