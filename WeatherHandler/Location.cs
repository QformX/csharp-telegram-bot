namespace WeatherHandler
{
    public record Location
    {
        public string name { get; init; }
        public string country { get; init; }
        public DateTime localtime { get; init; }
    }
}
