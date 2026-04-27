using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrammarService.Controllers;

[ApiController]
[Route("api/user-grammar")]
[Authorize]
public class UserGrammarController : ControllerBase
{
    // TODO: Inject IUserGrammarService
}
