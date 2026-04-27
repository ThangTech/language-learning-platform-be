using Shared.Common.Models;
namespace VocabularyService.Models;
public class Word : AuditableEntity
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
}
