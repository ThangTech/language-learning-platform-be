using System.Security.Claims;
using LanguagePlatform.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Không thể xác thực danh tính người dùng hoặc token không hợp lệ.");
        }
        return userId;
    }

    protected IActionResult HandleResult<T>(ApiResponse<T> result)
    {
        if (result == null)
        {
            return StatusCode(500, ApiErrorResponse.Fail("Đã có lỗi hệ thống xảy ra."));
        }

        if (!result.Success)
        {
            // Determine status code based on common failure message patterns
            var message = result.Message.ToLower();
            if (message.Contains("không tìm thấy") || message.Contains("không tồn tại") || message.Contains("not found"))
            {
                return NotFound(result);
            }
            if (message.Contains("đã tồn tại") || message.Contains("yêu cầu") || message.Contains("không hợp lệ") || message.Contains("sai") || message.Contains("trùng"))
            {
                return BadRequest(result);
            }
            if (message.Contains("chưa đăng nhập") || message.Contains("không thể xác thực") || message.Contains("unauthorized"))
            {
                return Unauthorized(result);
            }
            if (message.Contains("không có quyền") || message.Contains("cấm") || message.Contains("forbidden"))
            {
                return Forbid();
            }
            return BadRequest(result);
        }

        return Ok(result);
    }
}
