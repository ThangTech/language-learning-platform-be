using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VocabularyService.Controllers;

[ApiController]
[Route("api/words")]
[Authorize]
public class WordsController : ControllerBase
{
    // TODO: Inject IWordService
}
