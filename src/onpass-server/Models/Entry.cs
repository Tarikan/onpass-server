using  System;
using Newtonsoft.Json;

namespace onpass_server.Models
{
    /// <summary>
    /// Class that describes "Entry" table from database.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// Id of entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Website of entry.
        /// </summary>
        public String Website { get; set; }

        /// <summary>
        /// Username of Entry.
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// Password of Entry.
        /// </summary>
        public String Password { get; set; }
        
        /// <summary>
        /// User, which is an owner of entry.
        /// </summary>
        public User User { get; set; }

    }
}