using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Interfaces;

public interface IGrammarRepository : IGenericRepository<GrammarTopic>
{
    Task<(IEnumerable<GrammarTopic> Items, int TotalCount)> GetTopicsPagedAsync(int page, int pageSize, GrammarLevel? level = null, string? search = null);
}

public interface IUserGrammarRepository : IGenericRepository<UserGrammar>
{
    Task<IEnumerable<UserGrammar>> GetByUserIdAsync(Guid userId);
    Task<UserGrammar?> GetByUserAndTopicAsync(Guid userId, Guid topicId);
}
