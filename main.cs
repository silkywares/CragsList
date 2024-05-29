using System;
using System.Threading.Tasks;

class main
{
    static async Task Main(){
    
        Console.WriteLine("\nRunning main thread...");
        CragManager cragManager = new CragManager();
        await cragManager.SetHomeLocation();
        // cragManager.SetDistanceToHome();
        Console.WriteLine(Weather.GetWeather(cragManager.crags[1])+"\n");
    }
}
        
