using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly IUserAdminService _adminService;
    public AdminUsersController(IUserAdminService adminService) => _adminService = adminService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
        => Ok(await _adminService.GetAllUsersAsync(page, pageSize, search));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _adminService.GetUserByIdAsync(id));

    [HttpPut("{id:guid}/lock")]
    public async Task<IActionResult> Lock(Guid id)
        => Ok(await _adminService.LockUserAsync(id));

    [HttpPut("{id:guid}/unlock")]
    public async Task<IActionResult> Unlock(Guid id)
        => Ok(await _adminService.UnlockUserAsync(id));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _adminService.DeleteUserAsync(id));

    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleRequest request)
        => Ok(await _adminService.ChangeUserRoleAsync(id, request.Role));
}

public class ChangeRoleRequest { public string Role { get; set; } = string.Empty; }
