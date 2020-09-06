using System.ComponentModel.DataAnnotations;

namespace onpass_server.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}