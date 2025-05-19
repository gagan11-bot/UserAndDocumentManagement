using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UserAndDocumentManagement.Models;


[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {

        return Ok("Welcome, Admin!");
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _userManager.Users.Select(u => new { u.Id, u.Email, u.UserName }).ToList();
        return Ok(users);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RoleUpdateModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound("User not found");

        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Role));
        }
        var result = await _userManager.AddToRoleAsync(user, model.Role);
        return result.Succeeded ? Ok("Role assigned") : BadRequest(result.Errors);
    }

    [HttpPost("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] RoleUpdateModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound("User not found");

        var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
        return result.Succeeded ? Ok("Role removed") : BadRequest(result.Errors);
    }

    [HttpGet("roles/{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded ? Ok("User deleted") : BadRequest(result.Errors);
    }
}