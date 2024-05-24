using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;
using System.Threading.Tasks;




class CragParse
{
    private const int DECIMAL_PRECISION = 4;
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

            Console.Write(" | Name: " + url);
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
                    
                    //Saves the HTML contents as a string and loads them into an HtmlDocument
                    string htmlContent = response.Content.ReadAsStringAsync().Result;
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(htmlContent);

                    // Extract GPS data using XPath
                    HtmlNode gpsNode = document.DocumentNode.SelectSingleNode("//td[contains(text(), 'GPS:')]/following-sibling::td/text()[1]");
                    string gpsData = gpsNode?.InnerText.Trim();
                    gpsData = gpsData.Replace(" ", "&lon="); // Remove the space
                    gpsData = gpsData.Replace(",", ""); // Remove the comma

                    return gpsData;
                }
                else{
                    Console.WriteLine("Error code: " + response.StatusCode);
                    return null;
                }
            }
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
            Console.Write(" | Lat: " + parts[0] + " Lon: " + parts[1]);
            return (lat, lon);
          
        }
        else{
            return (-999.9F,-999.9F);
        }
    }
    public static async Task GetCurrentLocation()
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://ipinfo.io/json");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response to extract latitude and longitude
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
                    string[] coordinates = data.loc.ToString().Split(',');
                    double latitude = double.Parse(coordinates[0]);
                    double longitude = double.Parse(coordinates[1]);
                    Console.WriteLine($"Approximate Latitude: {latitude:F3}, Longitude: {longitude:F3}");
                }
                else
                {
                    Console.WriteLine("Failed to retrieve location information.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

}
