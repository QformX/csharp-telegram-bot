namespace WeatherHandler
{
    public record EmptyCurrent : Current
    {
        public EmptyCurrent()
        {
            last_updated = DateTime.MinValue;
            temp_c = 0;
            is_day = 0;
            condition = new EmptyCondition();
            wind_kph = 0;
            wind_dir = string.Empty;
            feelslike_c = 0;
        }
    }
}
