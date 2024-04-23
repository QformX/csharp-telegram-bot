namespace WeatherHandler
{
    public record Weather
    {
        public Dictionary<string, object> location { get; init; }
        public Dictionary<string, object> current { get; init; }
    }
}
