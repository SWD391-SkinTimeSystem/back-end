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

        /// <summary>
        ///     Delete user password (by changing the user status)
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
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
        [HttpPost("password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] PasswordUpdate password)
        {
            ServiceResult result = await _services.UpdateUserPassword(Guid.Parse(GetUserIdFromJwt()), password.OldPassword, password.NewPassword);

            return HandleServiceCall(result);
        }

        [HttpPost("status")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] AccountStatusUpdate update)
        {
            ServiceResult result = await _services.UpdateUserStatus(update.UserId, update.Status);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Get account information for all user in the system. (This should be limited to admin)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType<ApiResponse<PaginationResult<AccountInformation>>>(StatusCodes.Status200OK)]
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyCollection<AccountInformation>>> GetUserAccountList(int page = 1, int page_size = 10, UserRole? role = null, UserStatus? status = null)
        {
            return Ok(new ApiResponse<PaginationResult<AccountInformation>>
            {
                Success = true,
                Message = "Success",
                Data = await _services.GetUserByRole(page, page_size, role, status),
            });
        }


        /// <summary>
        ///     Return the currently authenticated user information.
        /// </summary>
        /// <returns>The user account information</returns>
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
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("account")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRegistration registrationInfo)
        {
            ServiceResult result;

            if (User.IsInRole("Admin"))
            {
                result = await _services.CreateUserAsAdmin(registrationInfo);
            }
            else
            {
                result = await _services.CreateAccount(registrationInfo);
            }
            
            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Request a password reset through email.
        /// </summary>
        /// <param name="email">The user email used to register an account</param>
        /// <param name="call_url">The site to redirect user when the email is sent to their inbox</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("password/forgot/request")]
        public async Task<IActionResult> RequestChangePassword([FromQuery] string email, string call_url)
        {
            ServiceResult result = await _services.RequestForgetPassword(email, call_url);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     Update the user password with a new one.
        /// </summary>
        /// <remarks>Callback when the user is redirected to reset password site</remarks>
        /// <param name="user_id">User id inside the system</param>
        /// <param name="new_password">user new password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("password/forgot")]
        public async Task<IActionResult> ChangeForgotPassword(Guid user_id, [FromBody] string new_password)
        {
            ServiceResult result = await _services.UpdateForgetPassword(user_id, new_password);

            return HandleServiceCall(result);
        }
    }
}
