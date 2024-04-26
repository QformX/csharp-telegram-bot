namespace WeatherHandler
{
    public record EmptyCondition : Condition
    {
        public EmptyCondition() 
        {
            text = string.Empty;
            icon = string.Empty;
        }
    }
}
