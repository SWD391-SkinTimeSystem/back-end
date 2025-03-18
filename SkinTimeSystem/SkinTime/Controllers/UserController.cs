using API.Model;
using AutoMapper;
using Azure;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Repositories;
using Services.Commons;
using Services.Commons.DTOs.User;
using Services.Commons.DTOs.Users;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using System.IO;
using System.Text;

namespace SkinTime.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/account")]
    public class UserController : BaseController
    {
        private readonly IUserService _services;

        public UserController(IUserService services, IMapper mapper, IEmailUtilities emailUtil, ITokenUtilities tokenUtil)
            : base(mapper, emailUtil, tokenUtil)
        {
            _services = services;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AccountInformation>> DeleteUser(Guid id)
        {
            ServiceResult result = await _services.UpdateUserStatus(id, UserStatus.Deleted);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///  Update user information
        /// </summary>
        /// <param name="user">some required fields</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] AccountUpdateInformation user)
        {
            ServiceResult result = await _services.UpdateUserInformation(Guid.Parse(GetUserIdFromJwt()), user);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///   Update user password. Requires to be authenticated to use
        /// </summary>
        /// <param name="password">old and new password</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] PasswordUpdate password)
        {
            ServiceResult result = await _services.UpdateUserPassword(Guid.Parse(GetUserIdFromJwt()), password.OldPassword, password.NewPassword);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Get account information for all user in the system. (This should be limited to admin)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType<ApiResponse<PaginationResult<AccountInformation>>>(StatusCodes.Status200OK)]
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyCollection<AccountInformation>>> GetUserAccountList(int page = 1, int page_size = 10)
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Success",
                Data = await _services.GetAllUser(page, page_size),
            });
        }

        /// <summary>
        ///     Return the currently authenticated user information.
        /// </summary>
        /// <returns>The user account information</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType<ApiResponse<AccountInformation>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountInformation>> GetUserAccount()
        {
            ServiceResult result = await _services.GetUserById(Guid.Parse(GetUserIdFromJwt()));

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Register a customer account
        /// </summary>
        /// <param name="registrationInfo">The required fields that a customer need to fill on the register page.</param>
        /// <returns>The result of the operation, the data will be the newly created user id.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> RegisterCustomerAccount([FromBody] CustomerRegistration registrationInfo)
        {
            ServiceResult result = await _services.CreateCustomerAccount(registrationInfo);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Create a new user account of any role. 
        /// </summary>
        /// <param name="registrationInfo">The user account registration information</param>
        /// <remarks>Only the admin may use this endpoint
        /// </remarks>
        /// <returns>200Ok response if successfully create an user account, else 400BadRequest</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("account")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRegistration registrationInfo)
        {
            ServiceResult result = await _services.CreateAccount(registrationInfo);

            return HandleServiceCall(result);
        }
    }
}
