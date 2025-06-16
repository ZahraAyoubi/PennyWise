using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Data;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Test.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly UserRepository _repo;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UserDbContext(options);

        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
    .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => new ApplicationUser { Email = email });


        _passwordHasher = new PasswordHasher<User>();

        _repo = new UserRepository(context, _passwordHasher, _mockUserManager.Object);
    }

    [Fact]
    public async Task Register_ShouldAddUserWithHashedPassword()
    {
        //Arrange
        var user = new ApplicationUser { Email = "test@example.com" };

        string password = "MyPass123";

        //Act
        var result = await _repo.RegisterAsync(user, password);
        var savedUser = await _repo.GetUserByEmailAsync("test@example.com");

        //Assert
        Assert.NotNull(result);
        Assert.NotEqual("MyPass123", savedUser.PasswordHash); // ensure password was hashed
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        //Arrange
        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Email = "test1@example.com" },
            new ApplicationUser { Email = "test2@example.com" }
        }.AsQueryable();

        _mockUserManager.Setup(m => m.Users).Returns(users);

        //Act
        var result = await _repo.GetAsync();

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnCorrectUser()
    {
        //Arrange
        string password = "123";
        await _repo.RegisterAsync(new ApplicationUser { Email = "email@host.com" }, password);

        //Act
        var user = await _repo.GetUserByEmailAsync("email@host.com");

        //Assert
        Assert.NotNull(user);
        Assert.Equal("email@host.com", user.Email);
    }
}
