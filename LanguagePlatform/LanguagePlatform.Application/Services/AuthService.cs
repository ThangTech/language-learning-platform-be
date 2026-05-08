using AutoMapper;
using LanguagePlatform.Application.DTOs.Auth;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwt;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepo, IJwtService jwt, IMapper mapper)
    {
        _userRepo = userRepo;
        _jwt = jwt;
        _mapper = mapper;
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return ApiResponse<AuthResponse>.Fail("Email hoặc mật khẩu không đúng.");

        if (user.Status == UserStatus.Locked)
            return ApiResponse<AuthResponse>.Fail("Tài khoản đã bị khóa.");

        var token = _jwt.GenerateToken(user);
        return ApiResponse<AuthResponse>.Ok(new AuthResponse
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        }, "Đăng nhập thành công.");
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepo.EmailExistsAsync(request.Email))
            return ApiResponse<AuthResponse>.Fail("Email đã được sử dụng.");

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.User,
            Status = UserStatus.Active
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        // Create initial progress record
        var token = _jwt.GenerateToken(user);
        return ApiResponse<AuthResponse>.Ok(new AuthResponse
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        }, "Đăng ký thành công.");
    }

    public async Task<ApiResponse<UserDto>> GetProfileAsync(Guid userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return ApiResponse<UserDto>.Fail("Không tìm thấy người dùng.");
        return ApiResponse<UserDto>.Ok(_mapper.Map<UserDto>(user));
    }

    public async Task<ApiResponse<UserDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return ApiResponse<UserDto>.Fail("Không tìm thấy người dùng.");

        user.FullName = request.FullName;
        if (!string.IsNullOrWhiteSpace(request.AvatarUrl))
            user.AvatarUrl = request.AvatarUrl;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();
        return ApiResponse<UserDto>.Ok(_mapper.Map<UserDto>(user), "Cập nhật hồ sơ thành công.");
    }

    public async Task<ApiResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return ApiResponse<bool>.Fail("Không tìm thấy người dùng.");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return ApiResponse<bool>.Fail("Mật khẩu hiện tại không đúng.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đổi mật khẩu thành công.");
    }
}
