namespace WeatherHandler
{
    public record EmptyLocation : Location
    {
        public EmptyLocation() 
        {
            name = string.Empty;
            country = string.Empty;
            localtime = DateTime.MinValue;
        }
    }
}
