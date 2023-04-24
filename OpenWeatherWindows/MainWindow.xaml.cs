using Newtonsoft.Json;
using OpenWeatherWindows.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OpenWeatherWindows
{
    public partial class MainWindow : Window
    {
        public static Profile profile;

        public MainWindow()
        {
            InitializeComponent();

            profile = Profile.OpenProfile(PermaLinks.path);
            
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
        
            var celsius = PermaLinks.celsius;   
        
            temp.Text = $"{Math.Round((decimal)(weather.temp - celsius))}";
        
            feelsLike.Text = $"FEELS LIKE: {Math.Round((decimal)(weather.feels_like - celsius))}";
        
            humiditytb.Text = $"HUMIDITY: {weather.humidity}";
        
            windspeed.Text = $"WIND SPEED: {weather.speed} m/s"; 
        
            clouds_all.Text = $"CLOUDINESS: {weather.all}%";
        
            WeatherStatusTextBox.Text = $"{weather.description?.ToUpper()}";
        }

        private Weather GetWeather()
        {
            var json = NetClient.GetResponseJson(PermaLinks.GetCurrentWeather(profile));

            dynamic? dyn = JsonConvert.DeserializeObject(json);

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

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch
            {
                //lol
            }
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

        private void MoreInformationButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Forecast forecastWindow = new Forecast();

            forecastWindow.Show();
        }
    }
}
