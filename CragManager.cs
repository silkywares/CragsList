using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;


class CragManager{
    string filePath = "C:\\Code\\HelloWorld\\crags.json";
    public List<Crag> crags;
    public float HomeLat;
    public float HomeLon;
    public string HomeName = "Home1";

    public CragManager()
    {
        crags = new List<Crag>();
        crags = LoadFromJson();
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

    public void SaveToJson()
    {
        Console.WriteLine("Saving crag to JSON.");
        // Serialize the crags list to JSON
        string json = JsonConvert.SerializeObject(crags, Formatting.Indented);

        // Write the JSON data to the file (overwrite existing content)
        File.WriteAllText(filePath, json);
    }

    public List<Crag> LoadFromJson()
    {
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

    public void AddCrag(Crag crag)
    {
        // Determine the index value for the new crag
        int lastIndex = crags.Count > 0 ? crags[crags.Count - 1].Index : 0;
        crag.Index = lastIndex + 1;

        Console.WriteLine("\nAdding #"+crag.Index +" "+ crag.Name + " to list.");
        
        // Add the new crag to the list
        crags.Add(crag);
        
        // Save the updated crags list to JSON
        SaveToJson();
    }

    public void RemoveCrag(int cragIndex)
    {
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

    public void ListCrags(){
        Console.WriteLine("\n\nListing crags saved: # " + crags.Count);
            for (int i = 0; i < crags.Count; i++)
            {
                Console.WriteLine("Crag " + crags[i].Index + ": " + crags[i].Name);
            }
    }

}