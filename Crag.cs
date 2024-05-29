using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using HtmlAgilityPack;


public class Crag
{
    public Crag(string url){
        Console.WriteLine("Constructing with url... ");
        Url = url;
        Name = FindName(url);
        Latlon = FindGps(url);
        (Latitude, Longitude) = SetLatLon(Latlon);
    }
    public Crag(){
        Console.WriteLine("Constructing from JSON... ");
    }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Latlon { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public int Index{get; set;}
    public double DistanceToHome{get; set;}
    private const int DECIMAL_PRECISION = 4;
    public WeatherResponse WeatherData { get; private set; }
    
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
    
    public static (float, float) SetLatLon(string latlon){
    
    //replace the api format with a comma
    latlon = latlon.Replace("&lon=",",");

    //split the string by the comma just added
    string[] parts = latlon.Split(',');

    //find the element index of the decimal point in each part of the string
    int decimalIndex_0 = parts[0].IndexOf('.');
    int decimalIndex_1 = parts[1].IndexOf('.');

    //truncate only the relevant substring and parse to float
    if (decimalIndex_0 != -1 && decimalIndex_1 != -1)
    {   
        parts[0] = parts[0].Substring(0, decimalIndex_0 + DECIMAL_PRECISION);
        parts[1] = parts[1].Substring(0, decimalIndex_1 + DECIMAL_PRECISION);
        float lat = float.Parse(parts[0]);
        float lon = float.Parse(parts[1]);
        //Console.Write(" | Lat: " + parts[0] + " Lon: " + parts[1]);
        return (lat, lon);
        
    }
    else{
        return (-999.9F,-999.9F);
    }
}
    public static string FindName(string url)
    {
        int lastSlashIndex = url.LastIndexOf('/');

        if (lastSlashIndex != -1)
        {
            // Trim off the last part of the URL
            url = url.Substring(lastSlashIndex + 1);
            url = url.Replace("-", " ");
            url = url.Replace("area", "");
            url = url.Replace("climbing", "");
            url = url.TrimEnd();

            // Capitalize the first letter of each word
            string[] words = url.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            url = string.Join(" ", words);

            // Console.Write(" | Name: " + url);
            return url;
        }
        else
        {
            Console.WriteLine("Invalid URL format");
            return null;
        }
    }
    public static string FindGps(string url){
        
            using(HttpClient client = new HttpClient()){

                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();

                if(response.IsSuccessStatusCode){
                    
                    HtmlDocument document = new HtmlDocument();
                    string htmlContent = response.Content.ReadAsStringAsync().Result;
                    document.LoadHtml(htmlContent);

                    // Extract GPS data using XPath
                    HtmlNode gpsNode = document.DocumentNode.SelectSingleNode("//td[contains(text(), 'GPS:')]/following-sibling::td/text()[1]");
                    string gpsData = gpsNode?.InnerText.Trim();
                    gpsData = gpsData.Replace(", ", "&lon=");

                    return gpsData;
                }
                else{
                    Console.WriteLine("Error code: " + response.StatusCode);
                    return null;
                }
            }
        }  
    public async Task  UpdateWeather(){
        string update = await Weather.GetWeather(this);
        WeatherData = JsonSerializer.Deserialize<WeatherResponse>(update);
        // Console.WriteLine($"Latitude: {WeatherData.Lat}");
        // Console.WriteLine($"Longitude: {WeatherData.Lon}");
        Console.WriteLine($"Timezone: {WeatherData.Timezone}");
        // Console.WriteLine($"Timezone Offset: {WeatherData.TimezoneOffset}");

        var currentWeather = WeatherData.Current;
        Console.WriteLine($"Current DateTime: {currentWeather.DateTime}");
        Console.WriteLine($"Sunrise: {currentWeather.Sunrise}");
        Console.WriteLine($"Sunset: {currentWeather.Sunset}");
        Console.WriteLine($"Temperature: {currentWeather.Temperature}");
        Console.WriteLine($"Feels Like: {currentWeather.FeelsLike}");
        Console.WriteLine($"Pressure: {currentWeather.Pressure}");
        Console.WriteLine($"Humidity: {currentWeather.Humidity}");
        Console.WriteLine($"Dew Point: {currentWeather.DewPoint}");
        Console.WriteLine($"UV Index: {currentWeather.UVIndex}");
        Console.WriteLine($"Clouds: {currentWeather.Clouds}");
        Console.WriteLine($"Visibility: {currentWeather.Visibility}");
        Console.WriteLine($"Wind Speed: {currentWeather.WindSpeed}");
        Console.WriteLine($"Wind Degrees: {currentWeather.WindDegrees}");
        Console.WriteLine($"Wind Gust: {currentWeather.WindGust}");

        foreach (var weatherDesc in currentWeather.WeatherDescriptions)
        {
            Console.WriteLine($"Weather Description: {weatherDesc.Description}");
            // Add other fields if needed
        }
    }
}