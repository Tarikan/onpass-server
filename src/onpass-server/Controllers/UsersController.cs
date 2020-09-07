using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using onpass_server.Models;
using onpass_server.Data;
using Newtonsoft.Json;

namespace onpass_server.Controllers
{
    /// <summary>
    /// Controller for User table in the database.
    /// </summary>
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        
        private readonly DatabaseContext _db;
        
        private readonly UserManager<User> _userManager;

        public UserController(ILogger<UserController> logger,
            DatabaseContext db,
            UserManager<User> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Get all information about current user.
        /// </summary>
        /// <returns>
        /// 401 response if unauthorized
        /// 200 response and username\, password
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            _logger.LogInformation("GetUserAsync called");
            var user = await _userManager.GetUserAsync(User);
            //Console.WriteLine(user.Email);
            //var response = new {UserName = currentUser.}
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new
            {
                UserName = user.UserName,
                Email = user.Email
            });
        }

        /// <summary>
        /// Update current user.
        /// </summary>
        /// <param name="upd">RegisterModel from request body</param>
        /// <returns>
        /// 401 response if unauthorized
        /// 404 response if password is wrong
        /// 200 response and new user if success
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterModel upd)
        {
            _logger.LogInformation("UpdateUserAsync called");
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.CheckPasswordAsync(user, upd.Password))
            {
                return NotFound("Wrong password");
            }

            user.Email = upd.Email;
            user.UserName = upd.UserName;

            await _userManager.UpdateAsync(user);

            return Ok(user);
        }

        /// <summary>
        /// Change password of current user.
        /// </summary>
        /// <param name="upd">Old and new password</param>
        /// <returns>
        /// 401 response if unauthorized
        /// 404 response if password is wrong
        /// 200 response and new user if success
        /// </returns>
        [HttpPut]
        [Route("Password")]
        public async Task<IActionResult> UpdatePassword([FromBody] ResetPasswordModel upd)
        {
            _logger.LogInformation("UpdatePasswordAsync called");
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }
            
            if (!await _userManager.CheckPasswordAsync(user, upd.OldPassword))
            {
                return NotFound("Wrong password");
            }

            await _userManager.ChangePasswordAsync(user, upd.OldPassword, upd.NewPassword);

            return Ok();
        }

        /// <summary>
        /// Delete current user.
        /// </summary>
        /// <returns>
        /// 401 response if unauthorized
        /// 200 response and new user if success
        /// </returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }

            var entries = user.Entries;
            _db.Entries.RemoveRange(entries);
            await _db.SaveChangesAsync();

            var res = await _userManager.DeleteAsync(user);
            
            if (res.Succeeded)
            {
                return Ok();
            }

            return Problem("Something goes wrong...");
        }
    }
}