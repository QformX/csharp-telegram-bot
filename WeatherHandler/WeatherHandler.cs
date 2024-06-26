﻿using TokenHandler;
using Newtonsoft.Json;

namespace WeatherHandler
{
    public class WeatherHandler
    {
        private static string? _api_key = null;
        private string _city;

        public WeatherHandler(string city) 
        {
            string path = "encryptedapi.bin";
            _api_key = new Tokenizer(path).Token();
            _city = city;
        }

        public Weather GetForecast()
        {
            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);

            var responce_city = client.GetAsync($"http://api.weatherapi.com/v1/current.json?key={_api_key}&q={_city}&lang=ru");
            var json = responce_city.Result.Content.ReadAsStringAsync().Result;

            Weather? weather = JsonConvert.DeserializeObject<Weather>(json);

            client.Dispose();

            if (weather != null )
            {
                if (weather.location == null & weather.current == null)
                {
                    throw new Exception("Указанного места не существует!");
                }
                else return weather;
            }
            return new EmptyWeather();
        }
    }
}