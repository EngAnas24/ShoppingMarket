using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Account.Models;
using ShoppingMarket.Account.Services.Interfaces;

using System.Threading.Tasks;

namespace ShoppingMarket.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("generate-email-confirmation-link")]
        public async Task<IActionResult> GenerateEmailConfirmationLink(string userId)
        {
            var confirmationLink = await _accountService.GenerateEmailConfirmationTokenAsync(userId);
            if (confirmationLink == null) { return BadRequest(); }
            return Ok(confirmationLink);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _accountService.RegisterAsync(model, Request);
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _accountService.LoginAsync(loginModel);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, token);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);

        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var resetLink = await _accountService.GeneratePasswordResetTokenAsync(email, Request);
            return Ok(resetLink);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var result = await _accountService.ResetPasswordAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.LogoutAsync();
            return Ok(result);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(AddRoleModel addRoleModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.AddRoleAsync(addRoleModel);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(addRoleModel);
        }

        public async Task<IActionResult> RefreshTokenAsync()
        {
            var RefreshToken = Request.Cookies["refreshToken"];
            var result = await _accountService.RefreshTokenAsync(RefreshToken);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            SetRefreshTokenInCookie(result.RefreshToken,result.RefreshTokenExpiration);
            return Ok(result);
        }

        public async Task<IActionResult> RevokedTokenAsync([FromBody] RevokedToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            var result = await _accountService.RevokeTokenAsync(token);

            if (!string.IsNullOrEmpty(token))
            {
                return NotFound("Token is required");
            }
            if (!result)
            {
                return BadRequest("Token is invalid!");
            }
            return Ok();
        }


        private void SetRefreshTokenInCookie(string refreshToken, DateTime ExpiredOn)
        {
            var CookiesOptions = new CookieOptions
            {

                HttpOnly = true,
                Expires = ExpiredOn,
            };

            Response.Cookies.Append("refreshToken", refreshToken, CookiesOptions);
        }
    }
}

