using  System;
using Newtonsoft.Json;

namespace onpass_server.Models
{
    public class Entry
    {
        public int Id { get; set; }

        public String Website { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }
        
        [JsonIgnore]
        public User User { get; set; }

    }
}