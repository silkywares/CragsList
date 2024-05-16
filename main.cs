using System;
using System.Threading.Tasks;

class main
{
    static async Task Main(){
    
        Console.WriteLine("\nRunning main thread...");
        CragManager crags = new CragManager();

        /*
        Console.WriteLine("\n\nAdding Crags");
    
        Crag test = new Crag("https://www.mountainproject.com/area/105798818/the-trapps");
        Crag test1 = new Crag("https://www.mountainproject.com/area/105807689/redgarden-wall");
        Crag test2 = new Crag("https://www.mountainproject.com/area/115004598/dark-shadows-wall");
        
        
        crags.AddCrag(test);
        crags.AddCrag(test1);
        
        Crag test3 = new Crag("https://www.mountainproject.com/area/106763709/yangshuo");
        crags.AddCrag(test3);
        */

        crags.ListCrags();

        await crags.SetHomeLocation();
        //Console.WriteLine("\nhomelat: "+crags.HomeLat+"\nhomelon: "+crags.HomeLon+"\n");

        
        
        for(int i = 1; i < 8; i++){
            Haversine.ReturnHaversineDistance(crags,i);
        }
        
        
        //Console.WriteLine("\nListing weather for crag #"+crags.crags[0].Index+" "+crags.crags[0].Name+"\n");
        //string output = WeatherCall.wcall(crags.crags[1].Latlon);
        //Console.WriteLine(output+"\n");
    }
}
        
