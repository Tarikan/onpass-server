using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace onpass_server.Models
{
    /// <summary>
    /// Class that descries user identity.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Collection of entries that belongs to user.
        /// </summary>
        public ICollection<Entry> Entries { get; set; }
    }
}