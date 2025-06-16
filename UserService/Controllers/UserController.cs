using Microsoft.AspNetCore.Identity;
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
    [Route("Register/{password}")]
    public async Task<IActionResult> Register([FromBody] ApplicationUser user, string password)
    {
        await _userService.Register(user, password);
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

    [HttpPost("send-reset-link")]
    public async Task<IActionResult> SendResetLink([FromBody] ApplicationUser request)
    {
        var user = await _userService.FindUserByEmail(request);
        if (user == null)
            return Ok(); // Don't reveal user existence

        var token = await _userService.GenerateResetToken(request);
        var resetLink = Url.Action("ResetPassword", "Account", new
        {
            token,
            email = request.Email
        }, Request.Scheme);

        var message = $"Click the link to reset your password: {resetLink}";
        await _userService.SendEmailAsync(request, "Reset Your Password", message);

        return Ok();
    }
}
