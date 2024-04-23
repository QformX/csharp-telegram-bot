using TokenHandler;

namespace WeatherHandler
{
    public class WeatherHandler
    {
        private static string? _api_key = null;

        public WeatherHandler() 
        {
            string path = "..\\..\\..\\..\\BotHandler\\encrypted.bin";
            var token = new Tokenizer(path).Token();
        }
    }
}