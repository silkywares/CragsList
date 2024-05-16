using System;

class main
{
    static void Main(){
    
        Console.WriteLine("\nRunning main thread...");
        CragManager crags = new CragManager();

        //Console.WriteLine("\n\nAdding Crags");

        //Crag test = new Crag("https://www.mountainproject.com/area/105854470/quartz-mountain");
        //Crag test1 = new Crag("https://www.mountainproject.com/area/106603617/las-cruces-area-climbing");
        
        //crags.AddCrag(test);
        //crags.AddCrag(test1);

        crags.ListCrags();
        
        Console.WriteLine("\nListing weather for crag #"+crags.crags[1].Index+" "+crags.crags[1].Name+"\n");
        string output = WeatherCall.wcall(crags.crags[1].Latlon);
        Console.WriteLine(output+"\n");
    }
}
        
