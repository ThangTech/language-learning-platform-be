using Shared.Common.Models;
namespace GrammarService.Models;
public class GrammarTopic : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
