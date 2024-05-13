using System;

class main
{
    static void Main(){
    
        Console.WriteLine("\nRunning main thread...");
        CragManager crags = new CragManager();

        //Console.WriteLine("\n\nAdding Crags");

        //Crag test = new Crag("https://www.mountainproject.com/area/105854470/quartz-mountain");
        //Crag test1 = new Crag("https://www.mountainproject.com/area/106603617/las-cruces-area-climbing");
        //Crag test2 = new Crag("https://www.mountainproject.com/area/108254627/sugarloaf-area");
        //Crag test3 = new Crag("https://www.mountainproject.com/area/106009888/los-alamos-white-rock");
        
        //crags.AddCrag(test);
        //crags.AddCrag(test1);
        //crags.AddCrag(test2);
        //crags.AddCrag(test);

        //crags.crags = crags.LoadFromJson();
        crags.ListCrags();

        WeatherCall reporter = new WeatherCall();
        Console.WriteLine("\nListing weather for crag #"+crags.crags[1].Index+" "+crags.crags[1].Name+"\n");
        string output = WeatherCall.wcall(crags.crags[1].Latlon);
        Console.WriteLine(output+"\n");
    }
}
        
