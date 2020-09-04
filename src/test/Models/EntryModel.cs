using System;

namespace onpass_server.Models
{
    public class Entry
    {
        public int Id { get; set; }

        public String Website { get; set; }

        public String Login { get; set; }

        public String Password { get; set; }

        public User User { get; set; }

    }
}