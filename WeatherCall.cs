using System;
using System.Net.Http;


/*API key for OpenWeather 45e62891196b886baf19eb0f2efde345
API call format
https://api.openweathermap.org/data/3.0/onecall?
lat={lat}&lon={lon}&exclude={part}&appid={API key}
lat (-90,90)
lon(-180,180)
appid
exclude - (current, minutely, hourly, daily, alerts)
units - (standard, metric, imperial) *standard is default)
*/


class WeatherCall{

    public static string wcall(string latlon){
        using(HttpClient client = new HttpClient()){
            Console.WriteLine("Reaching out to OpenWeather!\n");
            
            string apiUrl = "https://api.openweathermap.org/data/3.0/onecall?lat=" + latlon+ "&exclude=hourly,minutely,daily,alerts&appid=45e62891196b886baf19eb0f2efde345";
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