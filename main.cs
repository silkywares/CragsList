using System;
using System.Threading.Tasks;

class main
{
    static async Task Main(){
    
        Console.WriteLine("\nRunning main thread...");
        CragManager cragManager = new CragManager();
        await cragManager.SetHomeLocation();
        
        Console.WriteLine("\n\nAdding Crags");
        Crag test = new Crag("https://www.mountainproject.com/area/105798818/the-trapps");
        Crag test1 = new Crag("https://www.mountainproject.com/area/105807689/redgarden-wall");
        Crag test2 = new Crag("https://www.mountainproject.com/area/115004598/dark-shadows-wall");
        Crag test3 = new Crag("https://www.mountainproject.com/area/106763709/yangshuo");
        cragManager.AddCrag(test);
        cragManager.AddCrag(test1);
        cragManager.AddCrag(test2);
        cragManager.AddCrag(test3);
        // crags.ListCrags();

         foreach(Crag crag in cragManager.crags){
            cragManager.SetHaversineDistance(crag);
    
        }
    }
}
        
