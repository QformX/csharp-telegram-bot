using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            feelslike = 0;
        }
    }
}
