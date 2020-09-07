using System;
using System.ComponentModel.DataAnnotations;

namespace onpass_server.Models
{
    /// <summary>
    /// Class that contains username and password for login.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Username for login.
        /// </summary>
        [Required]
        public string UserName { get; set; }
        
        /// <summary>
        /// Password for login.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}