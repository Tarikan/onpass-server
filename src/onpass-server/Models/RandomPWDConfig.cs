namespace onpass_server.Models
{
    public class RandomPWDConfig
    {
        public int Length { get; set; }
        public bool Letters { get; set; }
        public bool Numbers { get; set; }
        public bool Symbols { get; set; }

        public override string ToString()
        {
            return $"Length = {Length.ToString()}\n" +
                   $"Letters is {Letters.ToString()}\n" +
                   $"Numbers is {Numbers.ToString()}\n" +
                   $"Symbols is {Symbols.ToString()}\n";
        }
    }
}