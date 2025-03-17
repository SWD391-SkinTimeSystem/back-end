using API.Model;
using AutoMapper;
using BusinessObject.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Services.Commons;
using Services.Commons.DTOs.Authentication;
using Services.Interfaces;
using SharedLibrary.EmailUtilities;
using SharedLibrary.TokenUtilities;

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
            ServiceResult result = await _authService.AuthenUserWithCredential(credentials.Account, credentials.Password);

            return HandleServiceCall(result);
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
            ServiceResult result = await _authService.AuthenUserWithGoogleWebToken(token.Token);

            return HandleServiceCall(result);
        }


        /// <summary>
        ///     Get a new pair of access and refresh token.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshAccessToken([FromBody] AuthenticationTokens tokens)
        {
            ServiceResult result = await _authService.RegenerateToken(tokens);

            return HandleServiceCall(result);
        }

        /// <summary>
        ///     verify user account.
        /// </summary>
        /// <remarks>We can send an email that will redirect user to the page that's call this endpoint.</remarks>
        /// <param name="id">The user id</param>
        /// <returns>200 if success, else 400</returns>
        [HttpPost("verify")]
        public async Task<ActionResult<ApiResponse>> VerifyUserAccount([FromBody] string id)
        {
            ServiceResult result = await _authService.VerifyUserAccount(id);

            return HandleServiceCall(result);
            
        }
    }
}
