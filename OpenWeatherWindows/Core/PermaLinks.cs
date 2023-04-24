using System;

namespace OpenWeatherWindows.Core
{
    public static class PermaLinks
    {
        public static float celsius = 273.15f;

        public static string path = $"{Environment.CurrentDirectory}/profile.json";

        /// <summary>
        /// https://api.openweathermap.org/data/2.5/weather?lat=
        /// </summary>
        public static string GetCurrentWeather(Profile profile)
        {
            return $"https://api.openweathermap.org/data/2.5/weather?lat=" +
                $"{profile.lat}&lon={profile.lon}&appid={profile.key}";
        }

        /// <summary>
        /// Call 5 day / 3 hour forecast data
        /// </summary>
        public static string GetForecastData(Profile profile)
        {
            return $"https://api.openweathermap.org/data/2.5/forecast?lat=" +
                $"{profile.lat}&lon={profile.lon}&appid={profile.key}";
        }
    }
}
