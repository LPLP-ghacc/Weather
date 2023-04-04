using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OpenWeatherWindows
{
    public partial class MainWindow : Window
    {
        private string lat;
        private string lon;
        private string apiKey; //https://home.openweathermap.org/api_keys

        private const float celsius = 273.15f;

        public MainWindow()
        {
            InitializeComponent();

            var path = $"{Environment.CurrentDirectory}/profile.json";

            SetProfile(OpenProfile(path));

            OutputText();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Tick);
            timer.Interval = new TimeSpan(0, 30, 0);
            timer.Start();
        }

        private void Tick(object? sender, EventArgs e) => OutputText();

        private void OutputText()
        {
            var weather = GetWeather();

            temp.Text = $"{Math.Round((decimal)(weather.temp - celsius))}";
            feelsLike.Text = $"FEELS LIKE: {Math.Round((decimal)(weather.feels_like - celsius))}";
            humiditytb.Text = $"HUMIDITY: {weather.humidity}";
            windspeed.Text = $"WIND SPEED: {weather.speed} m/s"; 
            clouds_all.Text = $"CLOUDINESS: {weather.all}%";
        }

        private Weather GetWeather()
        {
            string request = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";

            HttpWebRequest? httpRequest = (HttpWebRequest)WebRequest.Create(request);

            HttpWebResponse? httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var json = streamReader.ReadToEnd();

                dynamic? dyn = JsonConvert.DeserializeObject(json);

                if (dyn == null)
                    return null;

                Weather weather = new Weather()
                {
                    lon = dyn.coord.lon,
                    lat = dyn.coord.lat,

                    main = dyn.weather[0].main,
                    description = dyn.weather[0].description,

                    temp = dyn.main.temp,
                    feels_like = dyn.main.feels_like,
                    pressure = dyn.main.pressure,
                    humidity = dyn.main.humidity,
                    temp_min = dyn.main.temp_min,
                    temp_max = dyn.main.temp_max,
                    sea_level = dyn.main.sea_level,
                    grnd_level = dyn.main.grnd_level,

                    visibility = dyn.visibility,
                    speed = dyn.wind.speed,
                    deg = dyn.wind.deg,
                    gust = dyn.wind.gust,

                    all = dyn.clouds.all,

                    country = dyn.sys.contry,
                    sunrise = dyn.sys.sunrise,
                    sunset = dyn.sys.sunset
                };

                if (dyn.rain != null)
                {
                    var rainfallList = new List<string>();

                    foreach (var item in dyn.rain)
                        rainfallList.Add(item);

                    weather.rain_one_h = rainfallList[0];
                    weather.rain_three_h = rainfallList[1];
                }

                if (dyn.snow != null)
                {
                    var snowfallList = new List<string>();

                    foreach (var item in dyn.snow)
                        snowfallList.Add(item);

                    weather.snow_one_h = snowfallList[0];
                    weather.snow_three_h = snowfallList[1];
                }

                return weather;
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void SetProfile(Profile? profile)
        {
            if (profile != null)
            {
                lat = profile.lat;
                lon = profile.lon;
                apiKey = profile.key;
            }
        }

        public Profile OpenProfile(string path)
        {
            dynamic? data = JsonConvert.DeserializeObject(File.ReadAllText(path));

            return new Profile((string)data.lat, (string)data.lon, (string)data.key);
        }

        public void SaveProfile(Profile profile, string path)
        {
            var pipapipa = JsonConvert.SerializeObject(profile);

            File.WriteAllText(path, pipapipa);
        }

        #region Exit Ellipse

        private void exitEllipse_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var ellipse = sender as Ellipse;

            ellipse.Fill = Brushes.Red;
        }

        private void exitEllipse_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var ellipse = sender as Ellipse;

            var bc = new BrushConverter();

            ellipse.Fill = (Brush)bc.ConvertFrom("#FFA50000");
        }

        private void exitEllipse_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion
    }

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
    }
}
