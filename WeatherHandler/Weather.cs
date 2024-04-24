using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace WeatherHandler
{
    public record Weather
    {
        public Location? location { get; init; }
        public Current? current { get; init; }

        public string BuildMessage()
        {
            var mes = $@"Локация: {location.name}, {location.country}
Дата последнего обновления: {current.last_updated}
Текущая температура: {current.temp_c}
Ощущается: {current.feelslike}
Скорость ветра: {current.wind_kph}
Направление ветра: {current.wind_dir}";
            return mes;
        }
    }
}
