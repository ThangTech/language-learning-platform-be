using AutoMapper;
using LanguagePlatform.Application.DTOs.Auth;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class UserAdminService : IUserAdminService
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public UserAdminService(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<UserDto>>> GetAllUsersAsync(int page, int pageSize, string? search = null)
    {
        var (items, total) = await _userRepo.GetPagedAsync(page, pageSize, search);
        return ApiResponse<PagedResult<UserDto>>.Ok(new PagedResult<UserDto>
        {
            Items = _mapper.Map<List<UserDto>>(items),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return ApiResponse<UserDto>.Fail("Không tìm thấy người dùng.");
        return ApiResponse<UserDto>.Ok(_mapper.Map<UserDto>(user));
    }

    public async Task<ApiResponse<bool>> LockUserAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return ApiResponse<bool>.Fail("Không tìm thấy người dùng.");
        user.Status = UserStatus.Locked;
        user.UpdatedAt = DateTime.UtcNow;
        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã khóa tài khoản.");
    }

    public async Task<ApiResponse<bool>> UnlockUserAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return ApiResponse<bool>.Fail("Không tìm thấy người dùng.");
        user.Status = UserStatus.Active;
        user.UpdatedAt = DateTime.UtcNow;
        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã mở khóa tài khoản.");
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return ApiResponse<bool>.Fail("Không tìm thấy người dùng.");
        _userRepo.Remove(user);
        await _userRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa người dùng.");
    }

    public async Task<ApiResponse<bool>> ChangeUserRoleAsync(Guid id, string role)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return ApiResponse<bool>.Fail("Không tìm thấy người dùng.");
        if (!Enum.TryParse<UserRole>(role, true, out var newRole))
            return ApiResponse<bool>.Fail("Role không hợp lệ.");
        user.Role = newRole;
        user.UpdatedAt = DateTime.UtcNow;
        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, $"Đã đổi role thành {role}.");
    }
}
