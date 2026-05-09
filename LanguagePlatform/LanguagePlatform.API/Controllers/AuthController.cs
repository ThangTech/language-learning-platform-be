using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.Application.DTOs.Auth;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<UpdateProfileRequest> _updateProfileValidator;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

    public AuthController(
        IAuthService authService,
        IValidator<LoginRequest> loginValidator,
        IValidator<RegisterRequest> registerValidator,
        IValidator<UpdateProfileRequest> updateProfileValidator,
        IValidator<ChangePasswordRequest> changePasswordValidator)
    {
        _authService = authService;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _updateProfileValidator = updateProfileValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Kiểm tra dữ liệu đầu vào
        var ketQua = await _loginValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var ketQua = await _registerValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        Guid userId = GetUserId();
        var result = await _authService.GetProfileAsync(userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var ketQua = await _updateProfileValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        Guid userId = GetUserId();
        var result = await _authService.UpdateProfileAsync(userId, request);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var ketQua = await _changePasswordValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        Guid userId = GetUserId();
        var result = await _authService.ChangePasswordAsync(userId, request);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }
}
