using WeatherHandler;

namespace BotHandler
{
    internal class UserMessage
    {
        private long _id;
        private Dictionary<string, object> _attributes;
        private Dictionary<long, Dictionary<string, object>> keyValuePairs;

        public UserMessage(long id)
        {
            _id = id;
            _attributes = new Dictionary<string, object>();
            keyValuePairs = new Dictionary<long, Dictionary<string, object>>();
            _attributes.Add("message", String.Empty);
            _attributes.Add("weather", new object());
            keyValuePairs.Add(id, _attributes);
        }

        public void AddMessage(long id, string message)
        {
            keyValuePairs[id]["message"] = message;
        }

        public void AddWeather(long id, Weather message)
        {
            keyValuePairs[id]["weather"] = message;
        }

        public string GetMessage(long id)
        {
            return (string)keyValuePairs[id]["message"];
        }

        public Weather GetWeather(long id)
        {
            return (Weather)keyValuePairs[id]["weather"];
        }
    }
}
