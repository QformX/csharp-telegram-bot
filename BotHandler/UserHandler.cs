using SQLDatabase;
using WeatherHandler;

namespace BotHandler
{
    internal class UserHandler
    {
        public Dictionary<long, UserMessage> _userMessages;
        public Dictionary<long, bool> _usersCallback;

        public UserHandler() 
        {
            _userMessages = new Dictionary<long, UserMessage>();
            _usersCallback = new Dictionary<long, bool>();
        }

        public void CallbackOn(long userId)
        {
            _usersCallback[userId] = true;
        }

        public void CallbackOff(long userId)
        {
            _usersCallback[userId] = false;
        }

        public void Add(long userId)
        {
            _userMessages.Add(userId, new UserMessage(userId));
            Console.WriteLine(_userMessages.Count);
            _usersCallback.Add(userId, false);
        }

        public void AddWeather(long userId, Weather weather)
        {
            var weatherData = new Dictionary<string, object>();
            weatherData.Add("weather", weather);
            _userMessages[userId].AddWeather(userId, weather);

        }

        public void AddInputMessage(long userId, string message)
        {
            Console.WriteLine(message);
            var weatherData = new Dictionary<string, object>();
            weatherData.Add("weather", message);
            _userMessages[userId].AddMessage(userId, message);

        }

        public UserData GetUserData(long userId)
        {
            var message = (string)_userMessages[userId].GetMessage(userId);
            var weather = (Weather)_userMessages[userId].GetWeather(userId);
            return new UserData(weather.location.name, weather.location.country, weather.current.temp_c, weather.current.feelslike_c, weather.current.is_day, weather.current.wind_kph, weather.current.wind_dir, message, (int)userId);
        }

        public bool UserExist(long userId)
        {

            return _usersCallback.ContainsKey(userId);
        }

        public bool IsUserCallback(long userId)
        {

            return _usersCallback[userId];

        }
    }
}
