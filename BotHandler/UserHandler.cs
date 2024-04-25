
﻿using SQLDatabase;
using System.Collections.Generic;
using WeatherHandler;

namespace BotHandler
{
    internal class UserHandler
    {
        private Dictionary<string, object> _keyValuePairs;
        private Dictionary<long, Dictionary<string, object>> _users;

        public UserHandler() 
        {
            _keyValuePairs = new Dictionary<string, object>();
            _keyValuePairs.Add("callback", false);
            _keyValuePairs.Add("weather", new EmptyWeather());
            _keyValuePairs.Add("message", String.Empty);
            _users = new Dictionary<long, Dictionary<string, object>>();
        }

        public void Callback(long userId)
        {


            _users[userId]["callback"] = !(bool)_users[userId]["callback"];
        }

        public void Add(long userId)
        {

            _users.Add(userId, _keyValuePairs);
        }

        public void AddWeather(long userId, Weather weather)
        {
            _users[userId]["weather"] = weather;

        }

        public void AddInputMessage(long userId, string message)
        {
            _users[userId]["message"] = message;

        }

        public UserData GetUserData(long userId)
        {
            var message = (string)_users[userId]["message"];
            var weather = (Weather)_users[userId]["weather"];
            return new UserData(weather.location.name, weather.location.country, weather.current.temp_c, weather.current.feelslike_c, weather.current.is_day, weather.current.wind_kph, weather.current.wind_dir, message, (int)userId);
        }

        public bool UserExist(long userId)
        {

            return _users.ContainsKey(userId);
        }

        public bool IsUserCallback(long userId)
        {

            return (bool)_users[userId]["callback"];

        }
    }
}
