using Newtonsoft.Json;
using OpenWeatherWindows.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;

namespace OpenWeatherWindows
{
    public partial class Forecast : Window
    {
        public Profile profile;

        public Forecast()
        {
            InitializeComponent();

            profile = MainWindow.profile;

            var data = GetForecast();

            var item1 = GetDataItem1(data.Item1);
            
            var item2 = GetDataItem2(data.Item2);

            var temps = new List<string>();

            var _date = new List<DateTimeOffset>();

            var _headersValues = new List<string>();

            var _downheaders = new List<string>();

            foreach (var item in item1)
            {
                var date = UnixTimeToDateTime(long.Parse(item.Item5));
                _date.Add(date);

                _headersValues = GetUniqueDayOfWeekFromList(_date);

                _downheaders = GetUniqueDayFromList(_date);

                float temp = float.Parse(item.Item1.Replace(".", ","));

                var temp1 = Math.Round((decimal)(temp - PermaLinks.celsius), 1);

                temps.Add($"{date.DateTime.Hour}:00 \n {temp1}°C");

                debugblock.Text += $" |{date}| ";
            }

            for(int i = 0; i < 5; i++)
            {
                InstantiateVisualBox(i, _headersValues[i], _downheaders[i], temps);
            }
        }

        private List<string> GetUniqueDayOfWeekFromList(List<DateTimeOffset> dateTimes)
        {
            if(dateTimes.Count == 0)
                return new List<string>();

            var list = new List<string>();

            string value = "";

            foreach(var item in dateTimes)
            {
                if(value != item.DayOfWeek.ToString())
                {
                    value = item.DayOfWeek.ToString();

                    list.Add(item.DayOfWeek.ToString());
                }
            }

            return list;
        }

        private List<string> GetUniqueDayFromList(List<DateTimeOffset> dateTimes)
        {
            if (dateTimes.Count == 0)
                return new List<string>();

            var list = new List<string>();

            string value = "";

            foreach (var item in dateTimes)
            {
                if (value != item.Day.ToString())
                {
                    value = item.Day.ToString();

                    list.Add(item.Day.ToString());
                }
            }

            return list;
        }

        public DateTimeOffset UnixTimeToDateTime(long unixtime)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixtime);

            dateTime.ToLocalTime();

            return dateTime; 
        }

        private List<(string, string)> GetDataItem2 (List<Weather> data)
        {
            var value = new List<(string, string)>();

            foreach (var item in data)
            {
                var main = item.main;

                var desc = item.description;

                value.Add((main, desc));
            }

            return value;
        }

        private List<(string, string, string, string, string)> GetDataItem1(List<ForecastData> data)
        {
            var value = new List<(string, string, string, string, string)>();

            foreach (var item in data)
            {
                var temp = item.temp;

                var feelsLike = item.feels_like;

                var pressure = item.pressure;

                var humidity = item.humidity;

                var dt = item.dt;

                value.Add((temp, feelsLike, pressure, humidity, dt));
            }

            return value;
        }

        private void InstantiateVisualBox(double offset, string header, string downheader, List<string> temps)
        {
            //main grid
            Grid grid = new Grid();
            
            grid.Height = 318;
            grid.Width = 98;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
            grid.Margin = new Thickness(offset * grid.Width, 0, 0, 0);

            //header
            TextBlock block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Top;
            block.HorizontalAlignment = HorizontalAlignment.Center;   
            block.Margin = new Thickness(0, 5, 0, 0);
            block.Text = header;
            block.FontSize = 14;
            block.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            grid.Children.Add(block);

            //down header
            TextBlock downHeader = new TextBlock();
            downHeader.VerticalAlignment = VerticalAlignment.Top;
            downHeader.HorizontalAlignment = HorizontalAlignment.Center;
            downHeader.Margin = new Thickness(0, 20, 0, 0);
            downHeader.Text = downheader;
            downHeader.FontSize = 14;
            downHeader.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            grid.Children.Add(downHeader);

            //temps textboxes
            var textBlocks = InstantiateTextBlockToGrid(70, temps);

            foreach(var item in textBlocks)
                grid.Children.Add(item);

            //last setup
            ElementPanel.Children.Add(grid);
        }

        private List<TextBlock> InstantiateTextBlockToGrid(double constOffset, List<string> texts)
        {
            //textboxes
            double topOffset = 50;
            double rightOffset = 30;

            List<Thickness> thicknesses = new List<Thickness>()
            {
                new Thickness(0, constOffset, rightOffset, 0),
                new Thickness(0, constOffset + topOffset, rightOffset, 0),
                new Thickness(0, constOffset + topOffset * 2, rightOffset, 0),
                new Thickness(0, constOffset + topOffset * 3, rightOffset, 0),
                new Thickness(0, constOffset + topOffset * 4, rightOffset, 0)
            };

            var values = new List<TextBlock>();

            for (int i = 0; i < 5; i++)
            {
                var textblock = InstantiateTextBlock(thicknesses[i], texts[i]);
                values.Add(textblock);
            }

            return values;
        }

        private TextBlock InstantiateTextBlock(Thickness thic, string text)
        {
            TextBlock block = new TextBlock();
            block.VerticalAlignment = VerticalAlignment.Top;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.Margin = thic;
            block.Width = 60;
            block.Text = text;
            block.FontSize = 14;
            block.FontFamily = new FontFamily("Arial Black");
            block.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            
            return block;
        }

        private (List<ForecastData>, List<Weather>) GetForecast()
        {
            var json = NetClient.GetResponseJson(PermaLinks.GetForecastData(profile));

            dynamic? dyn = JsonConvert.DeserializeObject(json);

            var weatherList = new List<ForecastData>();

            var weatherListMain = new List<Weather>();

            foreach (var i in dyn.list)
            {
                weatherList.Add(new ForecastData()
                {
                    dt = i.dt,
                    temp = i.main.temp,
                    feels_like = i.main.feels_like,
                    temp_min = i.main.temp_min,
                    temp_max = i.main.temp_max,
                    pressure = i.main.pressure,
                    sea_level = i.main.sea_level,
                    grnd_level = i.main.grnd_level,
                    humidity = i.main.humidity,

                });

                foreach (var weather in i.weather)
                {
                    weatherListMain.Add(new Weather()
                    {
                        main = weather.main,
                        description = weather.description
                    });
                }
            }

            return (weatherList, weatherListMain);
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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
            this.Close();
        }
        #endregion
    }
}
