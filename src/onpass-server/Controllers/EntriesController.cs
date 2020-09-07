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
    /// <summary>
    /// CRUD Entry from the database and generate random password.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class EntriesController : Controller
    {
        private readonly ILogger<EntriesController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _db;

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public EntriesController(ILogger<EntriesController> logger,
            DatabaseContext db,
            UserManager<User> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Generate random password.
        /// </summary>
        /// <param name="config">RandomPWDConfig from request body.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all entries that belongs to current user
        /// </summary>
        /// <returns>
        /// 200 response and array of entries
        /// 401 response if unauthorized
        /// </returns>
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

        /// <summary>
        /// Create new entry
        /// </summary>
        /// <param name="entry">NewEntryModel from request body</param>
        /// <returns>
        /// 200 response and new entry
        /// 401 response if unauthorized
        /// </returns>
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

        /// <summary>
        /// Update existing entry
        /// </summary>
        /// <param name="Entries">Entry from request body</param>
        /// <returns>
        /// 200 response and new entry
        /// 401 response if unauthorized
        /// 404 response if entry not found
        /// </returns>
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

        /// <summary>
        /// Delete entry by Id
        /// </summary>
        /// <param name="Id">Id of entry</param>
        /// <returns>
        /// 200 response if deleted
        /// 401 response if unauthorized
        /// 404 response if entry not found
        /// </returns>
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