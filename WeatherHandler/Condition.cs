using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherHandler
{
    public record Condition
    {
        public string? text { get; init; }
        public string? icon { get; init; }
    }
}
