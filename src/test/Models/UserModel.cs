using System;

namespace onpass_server.Models
{
    public class User
    {
        public int Id { get; set; }

        public String Username { get; set; }

        public String Email { get; set; }

        public Entry[] Entries { get; set; }

        public String Password { get; set; }
    }
}