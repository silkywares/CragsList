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
        Crag test3 = new Crag("https://www.mountainproject.com/area/106763709/yangshuo");
        crags.AddCrag(test);
        crags.AddCrag(test1);
        crags.AddCrag(test2);
        crags.AddCrag(test3);
        */

        crags.ListCrags();

        await crags.SetHomeLocation();
        Console.WriteLine($"\nhomelat: {crags.HomeLat:F1}\nhomelon: {crags.HomeLon:F1}\n");

        
        
        for(int i = 0; i < 6; i++){
            Haversine.ReturnHaversineDistance(crags,i);
        }
        Console.WriteLine();
      
    }
}
        
