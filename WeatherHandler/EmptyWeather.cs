using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
