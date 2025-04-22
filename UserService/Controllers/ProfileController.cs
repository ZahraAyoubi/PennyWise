using Microsoft.AspNetCore.Mvc;
using UserService.IServices;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [Route("{email}")]
    public async Task<IActionResult> GetProfileByEmail(string email)
    {
        var profile = await _profileService.GetProfileByEmail(email);
        return Ok(new { profile });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] Profile profile)
    {
        await _profileService.UpdateProfile(profile);
        return Ok(new { message = "Profile updated successfully" });
    }
}
