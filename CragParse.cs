using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;


class CragParse
{
    public static string findName(string url){
        
        int lastSlashIndex = url.LastIndexOf('/');

        if (lastSlashIndex != -1)
        {
            // Trim off the last part of the URL
            url = url.Substring(lastSlashIndex + 1);
            url = url.Replace("-"," ");
            url = url.Replace("area","");
            url = url.Replace("climbing","");
            url = url.TrimEnd();

            Console.Write(" | Name: " + url);
            return url;
        }
        else{
                Console.WriteLine("Invalid URL format");
                return null;
        }
    }

    public static string findGps(string url){
    
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

    public static (float, float) setLatLon(string latlon){
       
        latlon = latlon.Replace("&lon=",",");
        string[] parts = latlon.Split(',');

        int decimalIndex_0 = parts[0].IndexOf('.');
        int decimalIndex_1 = parts[1].IndexOf('.');
        if (decimalIndex_0 != -1 && decimalIndex_1 != -1)
        {   
            parts[0] = parts[0].Substring(0, decimalIndex_0 + 3);
            parts[1] = parts[1].Substring(0, decimalIndex_1 + 3);
            float lat = float.Parse(parts[0]);
            float lon = float.Parse(parts[1]);
            Console.Write(" | Lat: " + parts[0] + " Lon: " + parts[1]);
            return (lat, lon);
          
        }
        else{
            return (-999.9F,-999.9F);
        }
    }

    

    public static string displayHTML(string url){
        
        using(HttpClient client = new HttpClient()){
            
            HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
           
            if(response.IsSuccessStatusCode){
            
                string htmlContent = response.Content.ReadAsStringAsync().Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmlContent);
                
                File.WriteAllText("output.html", htmlContent);
                Console.WriteLine("HTML content has been saved to output.html");
                return document.Text;
            }
            else{
                Console.WriteLine("Error code: " + response.StatusCode);
                return null;
            }
        }
    }


}
