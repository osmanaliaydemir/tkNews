using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Common.Models;

namespace tkNews.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(BaseResponse<T> response)
    {
        if (response == null) return NotFound();
        
        if (response.IsSuccess && response.Data == null)
            return NotFound();
            
        if (response.IsSuccess && response.Data != null)
            return Ok(response);
            
        if (response.Errors.Any())
            return BadRequest(response);
            
        return BadRequest(response);
    }
    
    protected ActionResult<BaseResponse<T>> HandlePagedResult<T>(BaseResponse<T> response)
    {
        if (response == null) return NotFound(BaseResponse<T>.Failure("Not Found"));
        
        if (response.IsSuccess && response.Data == null)
            return NotFound(BaseResponse<T>.Failure("Not Found"));
            
        if (response.IsSuccess && response.Data != null)
            return Ok(response);
            
        return BadRequest(response);
    }
    
    protected ActionResult<BaseResponse<T>> Success<T>(T data, string message = "")
    {
        return Ok(BaseResponse<T>.Success(data, message));
    }
    
    protected ActionResult<BaseResponse<T>> Failure<T>(string message, List<string> errors = null)
    {
        return BadRequest(BaseResponse<T>.Failure(message, errors));
    }
} 