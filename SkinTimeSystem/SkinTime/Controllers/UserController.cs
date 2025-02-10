using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using SkinTime.BLL.Services.UserService;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Enum;
using SkinTime.Helpers;
using SkinTime.Models;
using System.IO;
using System.Text;

namespace SkinTime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class userController : Controller
    {
        private readonly IUserService _services;
        private readonly IMapper _mapper;
        private readonly IEmailUtilities _emailUtilities;
        private readonly ITokenUtilities _tokenUtilities;

        public userController(IUserService services, IMapper mapper, IEmailUtilities emailUtil, ITokenUtilities tokenUtil)
        {
            _services = services;
            _mapper = mapper;
            _emailUtilities = emailUtil;
            _tokenUtilities = tokenUtil;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _services.GetUser(id);

            if (user == null)
            {
                return NotFound("Resource Not Found");
            }
            return Ok(_mapper.Map<AccountInformation>(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _services.DeleteUser(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpadteUser(string id,[FromBody] UserAdd user)
        {           
            var userUpdate = _mapper.Map<User>(user);
            await _services.UpdateUser(id,userUpdate);
            return Ok();
        }

        [HttpGet("account")]
        public async Task<IActionResult> GetUserAccount()
        {
            IReadOnlyCollection<AccountInformation> result = _mapper.Map<IReadOnlyCollection<User>,IReadOnlyCollection<AccountInformation>>(await _services.GetUsersAsReadOnly());

            Console.WriteLine(result.Count);

            ApiResponse<IReadOnlyCollection<AccountInformation>> response = new()
            {
                Success = true,
                Data = result
            };

            return Ok(response);
        } 

        [HttpPost("account/customer/register")]
        public async Task<IActionResult> RegisterCustomerAccount([FromBody] CustomerRegistration registrationInfo)
        {
            User userInformation = _mapper.Map<User>(registrationInfo);

            await _services.CreateUserAccount(userInformation);

            var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email.html");
            await _emailUtilities.SendGoogleEmailAsync(registrationInfo.Email, "SkinTime - New Registration Notice", content.Replace("[0]", registrationInfo.Fullname));

            ApiResponse<string> response = new ApiResponse<string>
            {
                Success = true,
                Data = "Successfully created user account."
            };

            return Ok(response);
        }

        [HttpPost("account")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRegistration registrationInfo)
        {
            User userInformation = _mapper.Map<User>(registrationInfo);

            userInformation.Role = registrationInfo.Role;

            await _services.CreateUserAccount(userInformation);

            var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email_staff.html");
            content = content.Replace("[1]", registrationInfo.Username).Replace("[2]", registrationInfo.Password);
            
            await _emailUtilities.SendGoogleEmailAsync(registrationInfo.Email, "SkinTime - New Registration Notice", content, "collincomms@gmail.com", "bglbjsyeohtjcnhg");

            ApiResponse<string> response = new ApiResponse<string>
            {
                Success = true,
                Data = "Successfully created user account."
            };

            return Created((string) null!,response);
        }
        
        [HttpPost("account/sign-in")]
        public async Task<IActionResult> SignInWithPassword([FromBody] UserCredential credential)
        {
            var userInformation = await _services.GetUserWithCredential(credential.Account, credential.Password);

            Console.WriteLine(userInformation != null);

            if (userInformation != null)
            {   Dictionary<string, string> userObject = new Dictionary<string, string>
                {
                    {"id", userInformation.Id.ToString()},
                    {"role", userInformation.Role},
                };

                ApiResponse<AuthenticationTokens> repsonse = new ApiResponse<AuthenticationTokens>
                {
                    Success = true,
                    Data = new()
                    {
                        AccessToken = _tokenUtilities.CreateJwtFromDictionary(userObject),
                        RefreshToken = _tokenUtilities.CreateBase64RefreshToken(userObject["id"])
                    }
                };

                return Ok(repsonse);
            }

            ApiResponse<string> response = new ApiResponse<string>
            {
                Success = false,
                Data = "Login failed",
            };

            return Unauthorized(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] string refreshToken)
        {
            // Validate refreshtoken.
            string? userIdString = _tokenUtilities.ValidateBase64RefreshToken(refreshToken);

            ApiResponse<AuthenticationTokens> response = new ApiResponse<AuthenticationTokens>
            {
                Success = true
            };

            if ( userIdString != null)
            {
                // Get user information.
                User userInformation = (await _services.GetUser(userIdString))!;

                // Create dictionary to generate new key.
                Dictionary<string, string> userObject = new Dictionary<string, string>
                {
                    {"id", userInformation.Id.ToString()},
                    {"role", userInformation.Role},
                };

                response.Data = new()
                {
                    AccessToken = _tokenUtilities.CreateJwtFromDictionary(userObject),
                    RefreshToken = _tokenUtilities.CreateBase64RefreshToken(userObject["id"])
                };

                return Ok(response);
            }

            response = new ApiResponse<AuthenticationTokens>
            {
                Success = false,
                Data = null
            };

            return Unauthorized(response);
        }
    }
}
