using Shared.Common.Models;
namespace QuizService.Models;
public class Quiz : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
}
