using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

class Weather{


    public async static Task<string> GetWeather(Crag crag){
        
        using(HttpClient client = new HttpClient()){
            Console.WriteLine("Reaching out to OpenWeather!\n");
            
            string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat=" + crag.Latlon + "&exclude=hourly,minutely,daily,alerts&appid=45e62891196b886baf19eb0f2efde345";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if(response.IsSuccessStatusCode){
                Console.WriteLine("Weather updated for: " + crag.Name);
                return await response.Content.ReadAsStringAsync();
            }
            else{
                Console.WriteLine("Error code: " + response.StatusCode);
                return null;
            } 
        } 
    }
}