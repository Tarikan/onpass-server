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