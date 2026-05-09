using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.API.Helpers;
using LanguagePlatform.Application.DTOs.Auth;
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
        var invalid = await ValidationHelper.ValidateAsync<LoginRequest, object>(_loginValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _authService.LoginAsync(request));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<RegisterRequest, object>(_registerValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _authService.RegisterAsync(request));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
        => Ok(await _authService.GetProfileAsync(GetUserId()));

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<UpdateProfileRequest, object>(_updateProfileValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _authService.UpdateProfileAsync(GetUserId(), request));
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<ChangePasswordRequest, object>(_changePasswordValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _authService.ChangePasswordAsync(GetUserId(), request));
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
