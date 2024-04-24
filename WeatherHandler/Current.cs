using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherHandler
{
    public record Current
    {
        public DateTime last_updated { get; init; }
        public float temp_c { get; init; }
        public int is_day { get; init; }
        public Condition? condition { get; init; }
        public float wind_kph { get; init; }
        public string? wind_dir { get; init; }
        public float feelslike { get; init; }
    }
}
