using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using onpass_server.Models;
using  onpass_server.Data;
using Newtonsoft.Json;

namespace onpass_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        
        private readonly DatabaseContext _db;

        public UserController(ILogger<UserController> logger, DatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers called");
            var response = _db.Users;
 
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int Id)
        {
            _logger.LogInformation($"GetUserById called with id {Id.ToString()}");

            if (!_db.Users.Any(u => u.Id == Id))
            {
                return NotFound();
            }

            var response = _db.Users.Single(u => u.Id == Id);
            
            return Ok(response);
            }

        [HttpPost]
        public IActionResult PostUser([FromBody] User user)
        {
            _logger.LogInformation($"PostUser called");
            try
            {
                user.EncryptPassword();
                _db.Users.Add(user);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            _logger.LogInformation($"UpdateUserById called with id {user.Id.ToString()}");
            if (!_db.Users.Any(u => u.Id == user.Id))
            {
                return NotFound();
            }
            user.EncryptPassword();

            var dbUser = _db.Users.Where(u => u.Id == user.Id).SingleOrDefault();
            dbUser.Username = user.Username;
            dbUser.Password = user.Password;
            dbUser.Email = user.Email;
            _db.SaveChanges();
            return Ok(dbUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserById(int Id)
        {
            _logger.LogInformation($"DeleteUserById called with id {Id.ToString()}");
            if (!_db.Users.Any(u => u.Id == Id))
            {
                return NotFound();
            }
            
            _db.Users.Remove(_db.Users.Single(u => u.Id == Id));
            _db.SaveChanges();
            return Ok();
        }
    }
}