using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<(IEnumerable<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search = null);
}
