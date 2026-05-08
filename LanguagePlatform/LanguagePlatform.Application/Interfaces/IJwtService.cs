using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    Guid? ValidateToken(string token);
}
