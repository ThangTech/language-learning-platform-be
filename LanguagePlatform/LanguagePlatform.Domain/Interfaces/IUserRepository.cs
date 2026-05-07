using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<(IEnumerable<User> Items, int TotalCount)> GetUsersPagedAsync(int page, int size, string? search = null);
}
