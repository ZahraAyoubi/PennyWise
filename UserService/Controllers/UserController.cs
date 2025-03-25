using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.IServices;
using UserService.Models;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserServices _userService;

    public UserController(IUserServices userService)
    {
        Console.WriteLine("🔹 UserController instantiated.");
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        await _userService.Register(user);
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.Login(request.Email, request.Password);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { user });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("user");
        return Ok(new { message = "Logged out successfully" });
    }
}
