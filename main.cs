using System;

class main
{
    static void Main(){
    
        Console.WriteLine("\nRunning main thread...");
        

        Crag test = new Crag("https://www.mountainproject.com/area/105854470/quartz-mountain");
        Crag test1 = new Crag("https://www.mountainproject.com/area/106603617/las-cruces-area-climbing");
        Crag test2 = new Crag("https://www.mountainproject.com/area/108254627/sugarloaf-area");
        Crag test3 = new Crag("https://www.mountainproject.com/area/106009888/los-alamos-white-rock");
        
        CragManager crags = new CragManager();
        
        Console.WriteLine("\n\nAdding Crags");
        
        crags.AddCrag(test);
        crags.AddCrag(test1);
        crags.AddCrag(test2);
        crags.AddCrag(test3);

        crags.LoadFromJson();

        Console.WriteLine("\n");
        crags.ListCrags();

        //WeatherCall reporter = new WeatherCall();
        //Console.WriteLine("\nListing weather for crag 0\n");
        //WeatherCall.wcall(crags[0].Name);

        Console.WriteLine("");  
    }
}
        