using System;
using System.Linq;
using onpass_server.Models;

namespace onpass_server.Utils
{
    public class PasswordGenerator
    {
        public static String generatePassword(RandomPWDConfig config)
        {
            var alpha = "abcdefghijklmnopqrstuvwxyz";
            var num = "0123456789";
            var sym = "`~!@#$%^&*()-_=+[]{}\\|\"';:,.<>/?";

            var new_password = "";
            
            var rng = new  Random();

            while (new_password.Length < config.Length)
            {
                var character = "";
                if (config.Letters)
                {
                    int index = rng.Next(alpha.Length);
                    if (rng.NextDouble() >= 0.5)
                    {
                        character += alpha[index].ToString().ToUpper();
                    }
                    else
                    {
                        character += alpha[index];
                    }
                }
                if (config.Numbers)
                {
                    int index = rng.Next(num.Length);
                    character += num[index];
                }
                if (config.Symbols)
                {
                    int index = rng.Next(sym.Length);
                    character += sym[index];
                }
                
                new_password += character;
            }
            var response = new string(new_password.ToCharArray().
                    OrderBy(s => (rng.Next(2) % 2) == 0).ToArray())
                .Substring(new_password.Length - config.Length);

            return response;
        }
    }
}