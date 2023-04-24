using Newtonsoft.Json;
using System.IO;

namespace OpenWeatherWindows.Core
{
    public class Profile
    {
        public string? lat { get; set; }
        public string? lon { get; set; }
        public string? key { get; set; }

        public Profile(string? lat, string? lon, string? key)
        {
            this.lat = lat;
            this.lon = lon;
            this.key = key;
        }

        public static Profile OpenProfile(string path)
        {
            dynamic? data = JsonConvert.DeserializeObject(File.ReadAllText(path));

            return new Profile((string)data.lat, (string)data.lon, (string)data.key);
        }

        public static void SaveProfile(Profile profile, string path)
        {
            var jsonProfile = JsonConvert.SerializeObject(profile);

            File.WriteAllText(path, jsonProfile);
        }
    }
}
