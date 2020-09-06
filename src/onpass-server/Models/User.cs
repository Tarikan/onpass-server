using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace onpass_server.Models
{
    public class User : IdentityUser
    {
        public ICollection<Entry> Entries { get; set; }
    }
}