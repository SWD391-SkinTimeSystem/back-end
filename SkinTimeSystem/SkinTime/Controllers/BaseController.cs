using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
public class BaseController : ControllerBase
{
    private const string ERORR_MESSAGE = "An error has occurred:";
    protected async Task<IActionResult> HandleApiCallAsync<T>(Func<Task<T>> func)
    {
        try
        {
            var result = await func();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ERORR_MESSAGE, error = ex.Message });
        }
    }
}
