using System;

public class Crag
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Latlon { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public int Index{get; set;}

    public Crag(string url)
    {
        Console.Write("\nConstructing crag: ");
        
        Url = url;
        Name = CragParse.findName(url);
        Latlon = CragParse.findGps(url);
        (Latitude, Longitude) = CragParse.setLatLon(Latlon);
        
    }
}