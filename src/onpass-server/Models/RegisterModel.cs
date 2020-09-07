using System;
using System.ComponentModel.DataAnnotations;

namespace onpass_server.Models
{
    /// <summary>
    /// Model for registration request.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// The Email property represents the new user Email adress.
        /// </summary>
        [Required]
        public string Email { get; set; }
        
        /// <summary>
        /// The Email property represents the new user username.
        /// </summary>
        [Required]
        public string UserName { get; set; }
        
        /// <summary>
        /// The Email property represents the new user password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}