namespace OpenWeatherWindows.Core
{
    public class Weather
    {
        public string? lat { get; set; }
        public string? lon { get; set; }
        //weather
        public string? main { get; set; }
        public string? description { get; set; }
        //main
        public float? temp { get; set; }
        public float? feels_like { get; set; }
        public string? pressure { get; set; }
        public string? humidity { get; set; }
        public string? temp_min { get; set; }
        public string? temp_max { get; set; }
        public string? sea_level { get; set; }
        public string? grnd_level { get; set; }
        //visibility
        public string? visibility { get; set; }
        //wind
        public string? speed { get; set; }
        public string? deg { get; set; }
        public string? gust { get; set; }
        //clouds
        public string? all { get; set; }
        //rain
        public string? rain_one_h { get; set; }
        public string? rain_three_h { get; set; }
        //snow
        public string? snow_one_h { get; set; }
        public string? snow_three_h { get; set; }
        //sys 
        public string? country { get; set; }
        public string? sunrise { get; set; }
        public string? sunset { get; set; }
    }

    public class ForecastData
    {
        //Time of data forecasted, unix, UTC
        public string? dt { get; set; }

        public string? temp { get; set; }

        public string? feels_like { get; set; }

        public string? temp_min { get; set; }
        public string? temp_max { get; set; }

        public string? pressure { get; set; }

        public string? sea_level { get; set; }
        public string? grnd_level { get; set; }

        public string? humidity { get; set; }

        public string? main { get; set; }
        public string? description { get; set; }

        //clouds
        public string? all { get; set; }

        //wind
        public string? speed { get; set; }
        public string? deg { get; set; }
        public string? gust { get; set; }

        //visibility
        public string? visibility { get; set; }

        // Probability of precipitation. The values of the parameter vary between 0 and 1,
        // where 0 is equal to 0%, 1 is equal to 100%
        public string? pop { get; set; }

        //rain
        public string? rain_one_h { get; set; }
        public string? rain_three_h { get; set; }
        //snow
        public string? snow_one_h { get; set; }
        public string? snow_three_h { get; set; }

        // Part of the day (n - night, d - day)
        public string? pod { get; set; }

        //city
        public string? id { get; set; }
        public string? name { get; set; }

        public string? lat { get; set; }
        public string? lon { get; set; }

        public string? population { get; set; }
    }
}
