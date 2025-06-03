using Microsoft.AspNetCore.Mvc;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.API.Common;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public abstract class ApplicationController : ControllerBase
{
    protected new IActionResult Ok(object? result = null)
    {
        var envelope = Envelope.Ok(result);
        
        return base.Ok(envelope);
    }
    
    protected IActionResult BadRequest(Error? error)
    {
        var errorInfo = new ErrorInfo(error);
        var envelope = Envelope.Error(errorInfo);
       
        return base.BadRequest(envelope);
    }
}