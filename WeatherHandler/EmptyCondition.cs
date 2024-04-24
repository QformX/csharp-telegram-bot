using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
