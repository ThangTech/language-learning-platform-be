using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

// Controller quản lý người dùng - chỉ Admin mới có quyền truy cập
[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ApiControllerBase
{
    private readonly IUserAdminService _adminService;

    public AdminUsersController(IUserAdminService adminService)
    {
        _adminService = adminService;
    }

    // Lấy danh sách tất cả người dùng (có phân trang và tìm kiếm)
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        var result = await _adminService.GetAllUsersAsync(page, pageSize, search);
        return HandleResult(result);
    }

    // Lấy thông tin chi tiết một người dùng theo ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _adminService.GetUserByIdAsync(id);
        return HandleResult(result);
    }

    // Khóa tài khoản người dùng
    [HttpPut("{id:guid}/lock")]
    public async Task<IActionResult> Lock(Guid id)
    {
        var result = await _adminService.LockUserAsync(id);
        return HandleResult(result);
    }

    // Mở khóa tài khoản người dùng
    [HttpPut("{id:guid}/unlock")]
    public async Task<IActionResult> Unlock(Guid id)
    {
        var result = await _adminService.UnlockUserAsync(id);
        return HandleResult(result);
    }

    // Xóa tài khoản người dùng
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _adminService.DeleteUserAsync(id);
        return HandleResult(result);
    }

    // Thay đổi vai trò người dùng (ví dụ: User → Admin)
    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleRequest request)
    {
        var result = await _adminService.ChangeUserRoleAsync(id, request.Role);
        return HandleResult(result);
    }
}

// DTO nhỏ dùng cho endpoint đổi vai trò
public class ChangeRoleRequest
{
    public string Role { get; set; } = string.Empty;
}
