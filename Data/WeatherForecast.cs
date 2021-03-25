using System;

namespace mgt_teams_explorer.Data
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        //var str = team is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)team) : team;
    }
}
