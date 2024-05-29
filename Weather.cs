using System;
using System.Text.Json;
using System.Net.Http;
class Weather{

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
        // public CurrentWeather[] WeatherDescriptions { get; set; }
    }
    public static string GetWeather(Crag crag){
        using(HttpClient client = new HttpClient()){
            Console.WriteLine("Reaching out to OpenWeather!\n");
            
            string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat=" + crag.Latlon + "&exclude=hourly,minutely,daily,alerts&appid=45e62891196b886baf19eb0f2efde345";
            HttpResponseMessage response = client.GetAsync(apiUrl).GetAwaiter().GetResult();

            if(response.IsSuccessStatusCode){
                return response.Content.ReadAsStringAsync().Result;
            }
            else{
                Console.WriteLine("Error code: " + response.StatusCode);
                return null;
            } 
        } 
    }




}