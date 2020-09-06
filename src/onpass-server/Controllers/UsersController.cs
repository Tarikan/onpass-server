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
                return BadRequest("Unauthorized");
            }
            return Ok(new
            {
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser()
        {
            _logger.LogInformation("UpdateUserAsync called");
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return BadRequest("Unauthorized");
            }

            return Ok(user);
        }
        
        
        
    }
}