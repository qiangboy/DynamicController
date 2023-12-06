using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Service;

//[Route("")]
//[ApiController]
[Produces("application/json")]
public class ApplicationService : /*ControllerBase,*/ IDynamicController, IApiBehaviorMetadata
{
}