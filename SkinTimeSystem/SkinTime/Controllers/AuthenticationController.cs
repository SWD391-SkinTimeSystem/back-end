using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;
using SkinTime.BLL.Commons;
using SkinTime.BLL.Services.AuthenticationService;
using SkinTime.DAL.Entities;
using SkinTime.Models;

namespace SkinTime.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private IAuthService _authService { get; set; }

        public AuthenticationController(IMapper mapper, IEmailUtilities emailUtils, ITokenUtilities tokenUtils, IConfiguration configuration, IAuthService authService)
        : base(mapper, emailUtils, tokenUtils)
        {
            this._authService = authService;
        }

        /// <summary>
        ///     Create an access and a refresh token (valid for 5 and 10 minutes respectively) using user 
        ///     credentials (username/email and password).
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>A new <see cref="AuthenticationTokens"/> containing an access token and a refresh token,
        /// else this will return an error response (400BadRequest) if user validation failed.</returns>
        [ProducesResponseType<AuthenticationTokens>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("signin")]
        public async Task<ActionResult> SignInWithCredentials([FromBody] UserCredential credentials)
        {
            return await HandleServiceCall<AuthenticationTokens>(async () =>
            {
                ServiceResult result = await _authService.GetUserWithCredential(credentials.Account, credentials.Password);

                if (result.IsSuccess)
                {
                    Dictionary<string, string> userObject = new Dictionary<string, string>
                    {
                        {"id", (result.Data as User)!.Id.ToString()},
                        {"role", (result.Data as User)!.Role.ToString() },
                    };

                    AuthenticationTokens tokens = new()
                    {
                        AccessToken = _tokenUtils.CreateJwtFromDictionary(userObject),
                        RefreshToken = _tokenUtils.CreateBase64RefreshToken(userObject["id"])
                    };

                    return ServiceResult<AuthenticationTokens>.Success(tokens);
                }

                return ServiceResult<AuthenticationTokens>
                .Failed(ServiceError.ValidationFailed("The given credentials does not match with any account"));
            });
        }

        /// <summary>
        ///     Allow user sign in (or create account) with Google identity token.
        /// </summary>
        /// <remarks>This should have been two different endpoints</remarks>
        /// <param name="token"></param>
        /// <returns><seealso cref="AuthenticationTokens"/> if success</returns>
        [HttpPost("signin-google")]
        [ProducesResponseType<AuthenticationTokens>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignInWithGoogleCredentials([FromBody] GoogleIdentityToken token)
        {
            return await HandleServiceCall(async () =>
            {
                ServiceResult<User> result = await _authService.GetUserWithGoogleWebToken(token.Token);
                Dictionary<string, string> userData;
                if (result.IsSuccess)
                {
                    userData = new()
                    {
                        {"id", result.Data!.Id.ToString() },
                        {"role", result.Data!.Id.ToString()}
                    };
                }
                else if (result.IsFailed && result.Error.Code == ServiceError._NotFound)
                {
                    // New user => created user account
                    result = await _authService.CreateUserWithGoogleWebToken(token.Token);

                    userData = new()
                    {
                        {"id", result.Data!.Id.ToString() },
                        {"role", result.Data!.Role.ToString()}
                    };

                    var content = System.IO.File.ReadAllText(".\\StaticResoucres\\register_email.html");
                    await _emailUtils.SendGoogleEmailAsync(result.Data.Email, "SkinTime - New Registration Notice", content.Replace("[0]", result.Data.FullName));

                    await _emailUtils.SendGoogleEmailAsync(result.Data.Email, "SkinTime - New Registration Notice", content);
                }
                else
                {
                    return result;
                }

                return ServiceResult.Success(new AuthenticationTokens
                {
                    AccessToken = _tokenUtils.CreateJwtFromDictionary(userData),
                    RefreshToken = _tokenUtils.CreateBase64RefreshToken(userData["id"])
                });

                
            });
        }


        /// <summary>
        ///     Get a new pair of access and refresh token.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        [HttpPost("refresh-tokens")]
        public async Task<ActionResult> RefreshAccessToken([FromBody] AuthenticationTokens tokens)
        {
            // Validate refreshtoken.
            string? userIdString = _tokenUtils.ValidateBase64RefreshToken(tokens.RefreshToken);

            return await HandleServiceCall<AuthenticationTokens>(async () =>
            {
                if (userIdString == null)
                {
                    return ServiceResult.Failed(ServiceError.Unauthorized("invalid refresh token"));
                }

                // Asynchronously wait for 0 seconds...
                await Task.Delay(0);

                // Get data from old token.
                Dictionary<string, string> oldInformation = _tokenUtils.GetDataDictionaryFromJwt(tokens.AccessToken);

                if (oldInformation.TryGetValue("error", out var error))
                {
                    return ServiceResult.Failed(ServiceError.ValidationFailed(error));
                }

                return ServiceResult<AuthenticationTokens>.Success(new()
                {
                    AccessToken = _tokenUtils.CreateJwtFromDictionary(oldInformation),
                    RefreshToken = _tokenUtils.CreateBase64RefreshToken(userIdString)
                });

            });
        }

        /// <summary>
        ///     verify user account.
        /// </summary>
        /// <remarks>We can send an email that will redirect user to the page that's call this endpoint.</remarks>
        /// <param name="id">The user id</param>
        /// <returns></returns>
        [HttpGet("verify")]
        public async Task<ActionResult<ApiResponse>> VerifyUserAccount(string id)
        {
            return await HandleServiceCall(async () =>
            {
                ServiceResult result = await _authService.VerifyUserAccount(id);

                if (result.IsSuccess)
                {
                    return ServiceResult.Success(new ApiResponse(true, "successfully verified user account"));
                }

                else return result;
            });
        }

    }
}
