using API.Model;
using AutoMapper;
using Azure;
using BusinessObject.Entities;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Services.Commons;
using Services.Commons.DTOs.User;
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
        public async Task<ActionResult<AccountInformation>> DeleteUser(string id)
        {
            return await HandleServiceCall<User, AccountInformation>(async () =>
            {
                return await _services.DeleteUser(id);
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] AccountUpdateInformation user)
        {
            return await HandleServiceCall(async () =>
            {
                // Get user id from jwt token.
                return await _services.UpdateUser(base.GetUserIdFromJwt(), _mapper.Map<User>(user));
            });

        }

        /// <summary>
        ///     Get account information for all user in the system. (This should be limited to admin)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyCollection<AccountInformation>>> GetUserAccountList()
        {
            return await HandleServiceCall<IReadOnlyCollection<User>, IReadOnlyCollection<AccountInformation>>(_services.GetUsersAsReadOnly);
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
            return await HandleServiceCall<User, AccountInformation>(async () =>
            {
                return await _services.GetUser(base.GetUserIdFromJwt());
            });
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
            return await HandleServiceCall(async () =>
            {
                ServiceResult<User> result = await _services.CreateUserAccount(_mapper.Map<User>(registrationInfo));

                if (result.IsFailed)
                {
                    return result;
                }

                var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email.html");
                await _emailUtils.SendGoogleEmailAsync(registrationInfo.Email, "SkinTime - New Registration Notice", content.Replace("[0]", registrationInfo.Fullname));

                return ServiceResult.Success(result.Data!.Id);
            });
        }

        /// <summary>
        ///     Create a new user account of any role. 
        /// </summary>
        /// <param name="registrationInfo">The user account registration information</param>
        /// <remarks>Only the admin may use this endpoint 
        ///     <para>
        ///         <b>Note:</b> This endpoint is not updated to use the latest 
        ///         <see cref="BaseController.HandleServiceCall(Func{Task{ServiceResult}})"/> to return 
        ///         a <see cref="ApiResponse"/> result to the client.
        ///     </para>
        /// </remarks>
        /// <returns>200Ok response if successfully create an user account, else 400BadRequest</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("account")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRegistration registrationInfo)
        {

            return await HandleServiceCall(async () =>
            {
                User userInformation = _mapper.Map<User>(registrationInfo);

                userInformation.Role = Enum.Parse<UserRole>(registrationInfo.Role);
                var result = await _services.CreateUserAccount(userInformation);

                if (result.IsSuccess)
                {
                    var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email_staff.html");
                    content = content.Replace("[1]", registrationInfo.Username).Replace("[2]", registrationInfo.Password);

                    await _emailUtils.SendGoogleEmailAsync(registrationInfo.Email, "SkinTime - New Registration Notice", content);
                }

                return ServiceResult.Success();
            });
        }
    }
}
