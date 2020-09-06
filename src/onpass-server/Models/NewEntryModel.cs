using  System;
using System.ComponentModel.DataAnnotations;

namespace onpass_server.Models
{
    public class NewEntryModel
    {
        [Required]
        public String Website { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }

    }
}