using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Data;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Test.Repositories;

public class ProfileRepositoryTests
{
    private readonly UserDbContext _context;
    private readonly ProfileRepository _profileRepo;
    private readonly UserRepository _userRepo;

    public ProfileRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new UserDbContext(options);

        // Setup mock UserManager
        var store = new Mock<IUserStore<ApplicationUser>>();
        var mockUserManager = new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );

        mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => new ApplicationUser { Email = email });

        var passwordHasher = new PasswordHasher<User>();

        _userRepo = new UserRepository(_context, passwordHasher, mockUserManager.Object);
        _profileRepo = new ProfileRepository(_context);
    }


    [Fact]
    public async Task CreateProfile_ShouldAddProfile()
    {
        string password = "123";
        var user = new ApplicationUser { Email = "new@user.com" };
        await _userRepo.RegisterAsync(user, password); // add user

        var result = await _profileRepo.CreateProfileAsync(user);

        var profile = await _profileRepo.GetProfileByEmailAsync("new@user.com");

        Assert.True(result);
        Assert.NotNull(profile);
    }
}
