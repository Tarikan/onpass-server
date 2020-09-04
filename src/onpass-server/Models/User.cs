using  System;
using Isopoh.Cryptography.Argon2;

namespace onpass_server.Models
{
    public class User
    {
        public int Id { get; set; }

        public String Username { get; set; }

        public String Email { get; set; }

        public Entry[] Entries { get; set; }

        public String Password { get; set; }

        public void EncryptPassword()
        {
            Password = Argon2.Hash(Password);
        }

        public Boolean CheckPassword(string unencryptedPassword)
        {
            return Argon2.Verify(Password, unencryptedPassword);
        }

        public override string ToString()
        {
            return $"User" + "\n" +
                   $"Id = {Id.ToString()}" + "\n" +
                   $"Username = {Username}" + "\n" +
                   $"Email = {Email}" + "\n" +
                   $"Password = {Password}";
        }  
    }
}