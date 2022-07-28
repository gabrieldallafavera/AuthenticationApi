using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("authentication")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <remarks>
        /// Password pattern:
        /// 
        ///     Must have at least 8 characters.
        ///     Must have at least one letter, number and special caracter.
        ///     Must have at leadt one uppercase and lowercase letter.
        /// 
        /// Request example:
        /// 
        ///     Post /UserRequest
        ///     {
        ///         "name": "Name Example",
        ///         "username": "nameExample",
        ///         "email": "example@email.com",
        ///         "password": "Example1@",
        ///         "confirmPassword": "Example1@",
        ///         "userRoles": [ /* Optional */
        ///             {
        ///                 "role": "Admin"
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        /// <param name="userRequest">Object UserRequest</param>
        /// <response code="201">User criated</response>
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(UserRequest userRequest)
        {
            return StatusCode((int)HttpStatusCode.Created, await _authService.Register(userRequest));
        }

        /// <summary>
        /// Log in to the system
        /// </summary>
        /// <remarks>
        /// Request example:
        /// 
        ///     Post /LoginRequest
        ///     {
        ///         "identification": "Example", /* Username or Email */
        ///         "password": "PasswordExample"
        ///     }
        /// </remarks>
        /// <param name="loginRequest">Object LoginRequest with user or email and password</param>
        /// <response code="200">Return object TokenResponse</response>
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(LoginRequest loginRequest)
        {
            return Ok(await _authService.Login(loginRequest));
        }

        /// <summary>
        /// Get new token
        /// </summary>
        /// <param name="token">Token</param>
        /// <response code="200">Return object TokenResponse</response>
        [HttpGet("refresh-token/{token}")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(string token)
        {
            return Ok(await _authService.RefreshToken(token));
        }

        /// <summary>
        /// Resend verify email
        /// </summary>
        /// <param name="identification">Identification</param>
        /// <response code="204">E-mail sended to verify</response>
        [HttpGet("resend-verify-email/{identification}")]
        public async Task<IActionResult> ResendVerifyEmail(string identification)
        {
            await _authService.ResendVerifyEmail(identification);
            return NoContent();
        }

        /// <summary>
        /// Verify e-mail
        /// </summary>
        /// <param name="token">Token</param>
        /// <response code="204">E-mail verified</response>
        [HttpPut("verify-email/{token}")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            await _authService.VerifyEmail(token);
            return NoContent();
        }

        /// <summary>
        /// Request to reset password
        /// </summary>
        /// <param name="identification">Identification</param>
        /// <response code="204">E-mail sended to redefine password</response>
        [HttpGet("forgot-password/{identification}")]
        public async Task<IActionResult> ForgotPassword(string identification)
        {
            await _authService.ForgotPassword(identification);
            return NoContent();
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// Password pattern:
        /// 
        ///     Must have at least 8 characters.
        ///     Must have at least one letter, number and special caracter.
        ///     Must have at leadt one uppercase and lowercase letter.
        /// 
        /// Request example:
        /// 
        ///     Post /ResetPasswordRequest
        ///     {
        ///         "password": "PasswordExample",
        ///         "confirmPassword": "PasswordExample"
        ///     }
        /// </remarks>
        /// <param name="token">Token</param>
        /// <param name="resetPasswordRequest">Object ResetPasswordRequest</param>
        /// <response code="204">Password reseted</response>
        [HttpPut("reset-password/{token}")]
        public async Task<IActionResult> VerifyEmail(string token, ResetPasswordRequest resetPasswordRequest)
        {
            await _authService.ResetPassword(token, resetPasswordRequest);
            return NoContent();
        }
    }
}
