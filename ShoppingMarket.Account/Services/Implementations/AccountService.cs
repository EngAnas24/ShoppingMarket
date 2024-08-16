using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShoppingMarket.Account.Models;
using ShoppingMarket.Account.Repositories.Interfaces;
using ShoppingMarket.Account.Services.Interfaces;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ShoppingMarket.Account.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager,
            IEmailService emailService, IAccountRepository accountRepository,
           IConfiguration configuration, SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _accountRepository = accountRepository;
            _configuration = configuration;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return encodedToken;
        }

        public async Task<AuthModel> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthModel { Message = "User not found." };
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                return new AuthModel { Message = "Email confirmed successfully." };
            }

            return new AuthModel { Message = "Error confirming email." };
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email, HttpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return "Invalid request.";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink = _emailService.GeneratePasswordResetLink(email, encodedToken, request);

            // For testing, return the reset link
            return resetLink;
        }

        public async Task<AuthModel> ResetPasswordAsync(ResetPasswordModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new AuthModel { Message = "Passwords do not match." };
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AuthModel { Message = "User not found." };
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

            if (result.Succeeded)
            {
                return new AuthModel { Message = "Password has been reset successfully." };
            }

            var errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += $"{error.Description},";
            }

            return new AuthModel { Message = errors };
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model, HttpRequest request)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await _accountRepository.CreateJwtToken(user);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = _emailService.GenerateEmailConfirmationLink(user.Id, encodedToken, request);

            // For testing, return the confirmation link
            var message = $"Please confirm your account by clicking <a href=\"{confirmationLink}\">here</a>.";

            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                Message = message
            };
        }

        public async Task<AuthModel> LoginAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return new AuthModel { Message = "The email is not valid" };
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!passwordValid)
            {
                return new AuthModel { Message = "The password is not valid" };
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:DurationInMinutes"])),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthModel> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new AuthModel { Message = "You logout successfully" };
        }

        public async Task<string> AddRoleAsync(AddRoleModel Rolemodel)
        {
            var user = await _userManager.FindByIdAsync(Rolemodel.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(Rolemodel.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, Rolemodel.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, Rolemodel.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";

        }

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var user =  _userManager.Users.SingleOrDefault(x => x.RefreshTokens.Any(x => x.Token == token));
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (user == null)
            {
                var authModel = new AuthModel();
                authModel.Message = "Invalid Token";
                return authModel;
            }

            if (!refreshToken.IsActive) {
                var authModel = new AuthModel();
                authModel.Message = "Invalid Token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var JwtToken= await CreateJwtToken(user);
           var AuthModel = new AuthModel();
            AuthModel.IsAuthenticated = true;
            AuthModel.Token = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            AuthModel.Email = user.Email;
            AuthModel.Username = user.UserName;
            AuthModel.IsExpired = false;
            AuthModel.ExpiresOn = JwtToken.ValidTo;
            AuthModel.RefreshToken = newRefreshToken.Token;
            AuthModel.RefreshTokenExpiration = newRefreshToken.ExpiredOn;

            return AuthModel;
        }
            public async Task<bool> RevokeTokenAsync(string token)
            {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (user is  null)
                return false;


            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
        }

            private RefreshToken GenerateRefreshToken()
            {
                var randomNumber = new byte[32];
                var generator = new RNGCryptoServiceProvider();
                generator.GetBytes(randomNumber);
                return new RefreshToken {
                    Token = Convert.ToBase64String(randomNumber),
                    CreatedOn = DateTime.Now,
                    ExpiredOn = DateTime.Now.AddDays(10)

                };

            }





        }
    }

