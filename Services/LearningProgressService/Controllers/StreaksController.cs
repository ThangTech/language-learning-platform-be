using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningProgressService.Controllers;

[ApiController]
[Route("api/streaks")]
[Authorize]
public class StreaksController : ControllerBase
{
    // TODO: Inject IStreakService
}
