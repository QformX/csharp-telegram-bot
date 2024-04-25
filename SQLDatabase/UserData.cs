using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDatabase
{
    public record UserData
    {
        public string _city {  get; set; }
        public string _country { get; set; }
        public string _temp_c { get; set; }
        public string _feelslike { get; set; }
        public string _is_day { get; set; }
        public string _wind_speed { get; set; }
        public string _wind_dir { get; set; }
        public string _clouth { get; set; }
        public string _chat_id { get; set; }

        public UserData(string city, string country, double temp_c, double feelslike, int is_day, double wind_speed, string wind_dir, string clouth, int chat_id)
        {
            _chat_id = chat_id.ToString();
            _city = city;
            _country = country;
            _temp_c = temp_c.ToString();
            _feelslike = feelslike.ToString();
                
            _is_day = is_day.ToString();
            _wind_speed = wind_speed.ToString();
            _wind_dir = wind_dir;
            _clouth = clouth;

        }
    }
}
