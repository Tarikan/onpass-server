using System.ComponentModel.DataAnnotations;

namespace onpass_server.Models
{
    /// <summary>
    /// Class that describes changing passwords.
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// Old password property.
        /// </summary>
        [Required]
        public string OldPassword { get; set; }
        
        /// <summary>
        /// New password property.
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}