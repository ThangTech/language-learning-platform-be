using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrammarService.Controllers;

[ApiController]
[Route("api/grammar")]
[Authorize]
public class GrammarController : ControllerBase
{
    // TODO: Inject IGrammarService
}
