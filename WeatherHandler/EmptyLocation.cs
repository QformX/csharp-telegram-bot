using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
