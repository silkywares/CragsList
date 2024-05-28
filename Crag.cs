using System;

using System.Net.Http;
using HtmlAgilityPack;


public class Crag
{
    private const int DECIMAL_PRECISION = 4;
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
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Latlon { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public int Index{get; set;}
    public double DistanceToHome{get; set;}
    
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
    public string WeatherCaller(){
        using(HttpClient client = new HttpClient()){
            Console.WriteLine("Reaching out to OpenWeather!\n");
            
            string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat=" + Latlon + "&exclude=hourly,minutely,daily,alerts&appid=45e62891196b886baf19eb0f2efde345";
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