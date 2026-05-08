using LanguagePlatform.Application.DTOs.Auth;
using LanguagePlatform.Application.DTOs.Common;

namespace LanguagePlatform.Application.Interfaces;

public interface IUserAdminService
{
    Task<ApiResponse<PagedResult<UserDto>>> GetAllUsersAsync(int page, int pageSize, string? search = null);
    Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id);
    Task<ApiResponse<bool>> LockUserAsync(Guid id);
    Task<ApiResponse<bool>> UnlockUserAsync(Guid id);
    Task<ApiResponse<bool>> DeleteUserAsync(Guid id);
    Task<ApiResponse<bool>> ChangeUserRoleAsync(Guid id, string role);
}
