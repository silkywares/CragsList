using System;

public class Crag
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Latlon { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public int Index{get; set;}

    public struct CurrentWeather
    {
        public long DateTime { get; set; }
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public double DewPoint { get; set; }
        public double UVIndex { get; set; }
        public int Clouds { get; set; }
        public int Visibility { get; set; }
        public double WindSpeed { get; set; }
        public int WindDegrees { get; set; }
        public double WindGust { get; set; }
        //public WeatherDescription[] WeatherDescriptions { get; set; }
    }

    public Crag(string url)
    {
        Console.Write("\nConstructing... ");
        
        Url = url;
        Name = CragParse.FindName(url);
        Latlon = CragParse.FindGps(url);
        (Latitude, Longitude) = CragParse.SetLatLon(Latlon);
    }
}