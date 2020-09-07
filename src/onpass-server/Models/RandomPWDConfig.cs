namespace onpass_server.Models
{
    /// <summary>
    /// Class that describes options for random password feature.
    /// </summary>
    public class RandomPWDConfig
    {
        /// <summary>
        /// Lenght property for new random password.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Letters bool property for new random password.
        /// </summary>
        public bool Letters { get; set; }

        /// <summary>
        /// Numbers bool property for new random password.
        /// </summary>
        public bool Numbers { get; set; }

        /// <summary>
        /// Symbols bool property for new random password.
        /// </summary>
        public bool Symbols { get; set; }

        /// <summary>
        /// ToString method for the class.
        /// </summary>
        /// <returns>String representation of properties</returns>
        public override string ToString()
        {
            return $"Length = {Length.ToString()}\n" +
                   $"Letters is {Letters.ToString()}\n" +
                   $"Numbers is {Numbers.ToString()}\n" +
                   $"Symbols is {Symbols.ToString()}\n";
        }
    }
}