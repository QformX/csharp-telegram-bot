using TokenHandler;
using Newtonsoft.Json;

namespace WeatherHandler
{
    public class WeatherHandler
    {
        private static string? _api_key = null;
        private string _city;

        public WeatherHandler(string city) 
        {
            string path = "..\\..\\..\\..\\WeatherHandler\\encrypted.bin";
            _api_key = new Tokenizer(path).Token();
            _city = city;
        }

        public Weather GetForecast()
        {
            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);

            var responce_city = client.GetAsync($"http://api.weatherapi.com/v1/current.json?key={_api_key}&q={_city}");
            var json = responce_city.Result.Content.ReadAsStringAsync().Result;

            Weather? weather = JsonConvert.DeserializeObject<Weather>(json);

            return weather;
        }
    }
}