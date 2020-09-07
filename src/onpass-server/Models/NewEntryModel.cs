using  System;
using System.ComponentModel.DataAnnotations;

namespace onpass_server.Models
{
    /// <summary>
    /// Class that describes new entry.
    /// </summary>
    public class NewEntryModel
    {
        /// <summary>
        /// Website property of entry.
        /// Required.
        /// </summary>
        [Required]
        public String Website { get; set; }

        /// <summary>
        /// Username property of entry.
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// Password property of entry.
        /// </summary>
        public String Password { get; set; }

    }
}