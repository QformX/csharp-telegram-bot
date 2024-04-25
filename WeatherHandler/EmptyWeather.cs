namespace WeatherHandler
{
    public record EmptyWeather : Weather
    {
        public EmptyWeather() 
        {
            location = new EmptyLocation();
            current = new EmptyCurrent();
        }
    }
}
