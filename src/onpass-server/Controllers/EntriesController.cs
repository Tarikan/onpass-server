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
using onpass_server.Utils;
using Newtonsoft.Json;

namespace onpass_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EntriesController : Controller
    {
        private readonly ILogger<EntriesController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _db;

        public EntriesController(ILogger<EntriesController> logger,
            DatabaseContext db,
            UserManager<User> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }
        
        [AllowAnonymous]
        [HttpPost("new_password")]
        public IActionResult GeneratePassword([FromBody] RandomPWDConfig config)
        {
            _logger.LogInformation($"GeneratePassword called with params\n{config.ToString()}");
            if (config.Length < 8 || config.Length > 127)
            {
                return BadRequest("The length must be between 8 and 127");
            }

            if (!config.Letters & !config.Symbols & !config.Numbers)
            {
                return BadRequest("At least one flag must be enabled");
            }
            
            var response = PasswordGenerator.generatePassword(config);
            
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEntries()
        {
            _logger.LogInformation($"GetAllEntriesAsync called");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            //_db.Entries.Where(entry => entry.User == user)
            var EntriesList = await _db.Entries.Where(entry => entry.User == user).ToListAsync();
            foreach (var e in EntriesList)
            {
                e.User = null;
            }
            return Ok(new
            {
                entries = EntriesList,
                count = EntriesList.Count
            });
        }

        [HttpPost]
        public async Task<IActionResult> PostEntries([FromBody] NewEntryModel entry)
        {
            _logger.LogInformation($"PostEntryAsync called");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            
            Entry NewEntry = new Entry()
            {
                Website = entry.Website,
                UserName = entry.UserName,
                Password = entry.Password,
                User = user
            };
            var result = _db.Entries.AddAsync(NewEntry);
            await _db.SaveChangesAsync();

            return Ok(NewEntry);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEntries([FromBody] Entry Entries)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            _logger.LogInformation($"UpdateEntriesById called with id {Entries.Id.ToString()}");
            if (!_db.Entries.Any(u => u.Id == Entries.Id))
            {
                return NotFound();
            }

            var dbEntry = await _db.Entries.Where(u => u.Id == Entries.Id).SingleOrDefaultAsync();
            dbEntry.UserName = Entries.UserName;
            dbEntry.Password = Entries.Password;
            dbEntry.Website = Entries.Website;
            _db.SaveChanges();
            return Ok(dbEntry);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntriesById(int Id)
        {
            _logger.LogInformation($"DeleteEntriesById called with id {Id.ToString()}");
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            Entry entry = await _db.Entries.Where(e => e.Id == Id).SingleOrDefaultAsync();

            if (entry == null)
            {
                return NotFound();
            }
            
            if (!_db.Entries.Any(u => u.Id == Id))
            {
                return NotFound();
            }
            
            _db.Entries.Remove(entry);
            _db.SaveChanges();
            return Ok();
        }
    }
}