using System;
using System.Threading.Tasks;
using onpass_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using onpass_server.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace onpass_server.Controllers
{
    /// <summary>w
    /// API controller that contains all endpoints for authentification
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        
        /// <summary>
        /// Constructor for AuthController class
        /// </summary>
        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,   
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Check if client is authorized
        /// </summary>
        /// <returns>
        /// Response with code 200 if authorized
        /// Response with code 401 if not authorized
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> CheckStatus()
        {
            _logger.LogInformation("CheckStatus called");  
            if (await _userManager.GetUserAsync(User) == null)
            {
                return Unauthorized();
            }
            return Ok();
        }
        
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// Response with code 200 if success
        /// Response with code 400 if username already exist
        /// </returns>
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            _logger.LogInformation("Register called");
            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
 
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User created a new account with password.");
                return Ok(user);
            }

            // If we got this far, something failed, redisplay form
            return BadRequest("Username already exist");
        }
        
        /// <summary>
        /// Log in user into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// Response with code 200 if success
        /// Response with code 400 if account is locked or wrong credentials
        /// </returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            _logger.LogInformation("Login called");
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Ok("Successfully logged in");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return BadRequest("Account locked out");
            }
            
            return BadRequest("Invalid username or/and password ");
        }

        /// <summary>
        /// Log out user from system
        /// </summary>
        /// <returns>
        /// Response with code 200 if success
        /// Response with code 401 if unauthorized
        /// </returns>
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Logout called");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            await _signInManager.SignOutAsync();
            return Ok();
        }
        
        /// <summary>
        /// Check password of current user
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns>
        /// Response with code 200 if success
        /// Response with code 401 if unauthorized
        /// Response with code 40 if wrong password
        /// </returns>
        [HttpPost]
        [Route("CheckPassword")]
        public async Task<IActionResult> CheckPassword([FromBody] string pwd)
        {
            _logger.LogInformation("CheckPassword called");
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }

            if (await _userManager.CheckPasswordAsync(user, pwd))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}