using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

class Weather{
    public struct WeatherResponse{
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        
        [JsonPropertyName("lon")]
        public double Lon { get; set; }
        
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        
        [JsonPropertyName("timezone_offset")]
        public int TimezoneOffset { get; set; }
        
        [JsonPropertyName("current")]
        public CurrentWeather Current { get; set; }
    }
    public struct CurrentWeather{
        [JsonPropertyName("dt")]
        public long DateTime { get; set; }
        
        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }
        
        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
        
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }
        
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }
        
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }
        
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
        
        [JsonPropertyName("dew_point")]
        public double DewPoint { get; set; }
        
        [JsonPropertyName("uvi")]
        public double UVIndex { get; set; }
        
        [JsonPropertyName("clouds")]
        public int Clouds { get; set; }
        
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }
        
        [JsonPropertyName("wind_speed")]
        public double WindSpeed { get; set; }
        
        [JsonPropertyName("wind_deg")]
        public int WindDegrees { get; set; }
        
        [JsonPropertyName("wind_gust")]
        public double WindGust { get; set; }
        
        [JsonPropertyName("weather")]
        public WeatherDescription[] WeatherDescriptions { get; set; }
    }
    public struct WeatherDescription{
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("main")]
        public string Main { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    public async static Task<WeatherResponse> GetWeather(Crag crag){
        using(HttpClient client = new HttpClient()){
            Console.WriteLine("Reaching out to OpenWeather!\n");
            
            string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat=" + crag.Latlon + "&exclude=hourly,minutely,daily,alerts&appid=45e62891196b886baf19eb0f2efde345";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if(response.IsSuccessStatusCode){

                string jsonString = await response.Content.ReadAsStringAsync();
                WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(jsonString);
                // Console.WriteLine($"Latitude: {weatherResponse.Lat}");
                // Console.WriteLine($"Longitude: {weatherResponse.Lon}");
                // Console.WriteLine($"Timezone: {weatherResponse.Timezone}");
                // Console.WriteLine($"Timezone Offset: {weatherResponse.TimezoneOffset}");

                // var currentWeather = weatherResponse.Current;
                // Console.WriteLine($"Current DateTime: {currentWeather.DateTime}");
                // Console.WriteLine($"Sunrise: {currentWeather.Sunrise}");
                // Console.WriteLine($"Sunset: {currentWeather.Sunset}");
                // Console.WriteLine($"Temperature: {currentWeather.Temperature}");
                // Console.WriteLine($"Feels Like: {currentWeather.FeelsLike}");
                // Console.WriteLine($"Pressure: {currentWeather.Pressure}");
                // Console.WriteLine($"Humidity: {currentWeather.Humidity}");
                // Console.WriteLine($"Dew Point: {currentWeather.DewPoint}");
                // Console.WriteLine($"UV Index: {currentWeather.UVIndex}");
                // Console.WriteLine($"Clouds: {currentWeather.Clouds}");
                // Console.WriteLine($"Visibility: {currentWeather.Visibility}");
                // Console.WriteLine($"Wind Speed: {currentWeather.WindSpeed}");
                // Console.WriteLine($"Wind Degrees: {currentWeather.WindDegrees}");
                // Console.WriteLine($"Wind Gust: {currentWeather.WindGust}");

                // foreach (var weatherDesc in currentWeather.WeatherDescriptions)
                // {
                //     Console.WriteLine($"Weather Description: {weatherDesc.Description}");
                //     // Add other fields if needed
                // }
                // Console.WriteLine("weather description2: "+weatherResponse.Current.WeatherDescriptions[2]);
                return weatherResponse;
            }
            else{
                Console.WriteLine("Error code: " + response.StatusCode);
                return new WeatherResponse();

            } 
        } 
    }
    public static void PrintWeatherResponse(WeatherResponse response){
        
    }



}