using Shared.Common.Models;
namespace LearningProgressService.Models;
public class UserProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public int TotalScore { get; set; }
    public int CurrentStreak { get; set; }
}
