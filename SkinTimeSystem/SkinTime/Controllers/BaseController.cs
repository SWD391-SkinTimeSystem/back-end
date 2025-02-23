using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.Helpers;
using SkinTime.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

[ApiController]
public class BaseController : ControllerBase
{
    private const string ERORR_MESSAGE = "An error has occurred:";
    protected readonly IMapper _mapper;
    protected readonly ITokenUtilities _tokenUtils;
    protected readonly IEmailUtilities _emailUtils;

    public BaseController(IMapper mapper, IEmailUtilities emailUtilities, ITokenUtilities tokenUtilities)
    {
        _mapper = mapper;
        _emailUtils = emailUtilities;
        _tokenUtils = tokenUtilities;
    }

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

    protected async Task<ActionResult> HandleServiceCall(Func<Task<ServiceResult>> func)
    {
        var result = await func();

        var error = HandleError((ServiceResult)result);

        if (error != null)
        {
            return error;
        }

        return Ok(new ApiResponse(true, "Success", result.Data));
    }

    protected async Task<ActionResult> HandleServiceCall<T>(Func<Task<ServiceResult>> func)
    {
        var result = await func();

        var error = HandleError((ServiceResult)result);

        if (error != null)
        {
            return error;
        }

        return Ok(new ApiResponse(true, "Success", _mapper.Map<T>(result.Data)));
    }

    protected async Task<ActionResult<TResult>> HandleServiceCall<T, TResult>(Func<Task<ServiceResult<T>>> func)
    {
        var result = await func();

        var error = HandleError(result);

        if (error != null)
        {
            return error;
        }

        return Ok(new ApiResponse(true, "Success",_mapper.Map<TResult>(result.Data)));
    }

    private ActionResult? HandleError(ServiceResult result)
    {
        if (result.IsFailed)
        {
            ApiResponse errorResponse = new ApiResponse(false, result.Error.Description!, result.Error.Information, result.Error.Code);

            switch (result.Error.Code)
            {
                case ServiceError._ValidationFailed:
                    return StatusCode(400, errorResponse);
                case ServiceError._NotExisted:
                    return StatusCode(400, errorResponse);
                case ServiceError._Unauthorized:
                    return StatusCode(401, errorResponse);
                case ServiceError._NotFound:
                    return StatusCode(404, errorResponse);
                case ServiceError._AlreadyExisted:
                    return StatusCode(409, errorResponse);
                default:
                    return StatusCode(500, errorResponse);
            }
        }
        return null;
    }
}
