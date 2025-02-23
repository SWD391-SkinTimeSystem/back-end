using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.BLL.Services.UserService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.Models;
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
            :base(mapper, emailUtil,tokenUtil)
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

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] AccountUpdateInformation user)
        {
            // Get user id from jwt token.
            string jwt = Request.Headers.Authorization.First()!;
            string user_id = _tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"];

            var userUpdate = _mapper.Map<User>(user);
            await _services.UpdateUser(user_id,userUpdate);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyCollection<AccountInformation>>> GetUserAccountList()
        {
            return await HandleServiceCall<IReadOnlyCollection<User>, IReadOnlyCollection<AccountInformation>>(_services.GetUsersAsReadOnly);
        }

        [HttpGet]
        public async Task<ActionResult<AccountInformation>> GetUserAccount()
        {
            // Get user id from jwt token.
            string jwt = Request.Headers.Authorization.First()!;
            string user_id = _tokenUtils.GetDataDictionaryFromJwt(jwt.Split()[1])["id"];

            return await HandleServiceCall<User, AccountInformation>(async () =>
            {
                return await _services.GetUser(user_id);
            });
        }

        /// <summary>
        ///     Register a customer account
        /// </summary>
        /// <param name="registrationInfo">The required fields that a customer need to fill on the register page.</param>
        /// <returns>The result of the operation, the data will be the newly created user id.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
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

                ApiResponse response = new ApiResponse(true, "Successfully created the user account", result.Data!.Id);

                return ServiceResult.Success(response);
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
            User userInformation = _mapper.Map<User>(registrationInfo);

            userInformation.Role = Enum.Parse<UserRole>(registrationInfo.Role);

            await _services.CreateUserAccount(userInformation);

            var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email_staff.html");
            content = content.Replace("[1]", registrationInfo.Username).Replace("[2]", registrationInfo.Password);
            
            await _emailUtils.SendGoogleEmailAsync(registrationInfo.Email, "SkinTime - New Registration Notice", content);

            ApiResponse<string> response = new(true, "Successfully created user account.");

            return Created((string) null!,response);
        }
    }
}
