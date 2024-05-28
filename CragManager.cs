using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;


class CragManager{
    const float EARTH_RADIUS_METERS = 6378000F;
    string filePath = "C:\\Code\\HelloWorld\\crags.json";
    public List<Crag> crags;
    public float HomeLat;
    public float HomeLon;
    public string HomeName = "Home";

    public CragManager(){
        crags = new List<Crag>();
        crags = LoadFromJson();
    }
    public void AddCrag(Crag crag){
        // Determine the index value for the new crag
        int lastIndex = crags.Count > 0 ? crags[crags.Count - 1].Index : 0;
        crag.Index = lastIndex + 1;

        Console.WriteLine("\nAdding #"+crag.Index +" "+ crag.Name + " to list.");
        
        // Add the new crag to the list
        crags.Add(crag);
        
        // Save the updated crags list to JSON
        SaveToJson();
    }
    void RemoveCrag(int cragIndex){
        // Load data from JSON file
        List<Crag> cragsList = JsonConvert.DeserializeObject<List<Crag>>(File.ReadAllText(filePath));

        if (cragIndex >= 0 && cragIndex < cragsList.Count)
        {
            // Remove the item at the specified index
            cragsList.RemoveAt(cragIndex);

            // Serialize the updated list back to JSON
            string json = JsonConvert.SerializeObject(cragsList, Formatting.Indented);

            // Write the JSON data back to the file
            File.WriteAllText(filePath, json);

            crags = cragsList;

            Console.WriteLine("\nRemoved item at index " + cragIndex + " from the list and saved to file.");
            SaveToJson();
        }
        else
        {
            Console.WriteLine("Invalid index.");
        }
    }
    void SaveToJson(){
        Console.WriteLine("Saving crags to JSON.");
        string json = JsonConvert.SerializeObject(crags, Formatting.Indented);

        File.WriteAllText(filePath, json);// Write the JSON data to the file (overwrite existing content)
    }
    List<Crag> LoadFromJson(){
        try
        {
            Console.WriteLine("\nLoading crags from JSON into CragManager.");
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Crag>>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading crags from JSON: " + ex.Message);
            return new List<Crag>(); // Return an empty list to avoid returning null
        }
    }
    public void ListCrags(){
        Console.WriteLine("\n\nListing crags saved: # " + crags.Count);
            for (int i = 0; i < crags.Count; i++)
            {
                Console.WriteLine("Crag " + crags[i].Index + ": " + crags[i].Name);
            }
    }
    public static async Task GetCurrentLocation(){
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
    public async Task SetHomeLocation()
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
                    //Console.WriteLine($"Approximate Latitude: {latitude}, Longitude: {longitude}");
                    HomeLat = (float)latitude; // Set HomeLat
                    HomeLon = (float)longitude; // Set HomeLon
                    Console.WriteLine($"HomeLat ={HomeLat} HomeLon=={HomeLat}");
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
    public double SetHaversineDistance(Crag crag){
    
        float homeLatRad = (float)(HomeLat * (Math.PI / 180.0));
        float homeLonRad = (float)(HomeLon * (Math.PI / 180.0));
        float cragLatRad = (float)(crag.Latitude * (Math.PI / 180.0));
        float cragLonRad = (float)(crag.Longitude * (Math.PI / 180.0));

        float deltaLat = cragLatRad - homeLatRad;
        float deltaLon = cragLonRad - homeLonRad;

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(homeLatRad) * Math.Cos(cragLatRad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = c * EARTH_RADIUS_METERS;

        //Console.WriteLine($"{HomeName} to {crag.Name} : {distance * 0.000001:F3} Mm.");
        return distance;
    }
    public void SetDistanceToHome(){
            //This function will override all of the DistanceToHomes for each crag and saves
            foreach (Crag crag in crags){
                crag.DistanceToHome = SetHaversineDistance(crag);
                Console.WriteLine($"Distance from {HomeName} to {crag.Name} : {crag.DistanceToHome * 0.000001:F3} Mm.");
            }
            SaveToJson();
    }
}