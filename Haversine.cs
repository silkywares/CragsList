using System;
class Haversine{
/*
a = sin^2((Δlat)/2)+cos(lat_1)cos(lat_2)sin^2((Δlon)/2)
c = 2arctan(√a/√1-a)
d = Rc
R_meters = 6.378E6
*/

    const float EARTH_RADIUS_METERS = 6378000F;

    public static double ReturnHaversineDistance(CragManager cragManager, int index){
        
        //calculates Δlat and Δlon
        float deltaLat = cragManager.crags[index].Latitude - cragManager.HomeLat;
        float deltaLon = cragManager.crags[index].Longitude - cragManager.HomeLon;
        //Console.WriteLine("\nFrome home to "+cragManager.crags[index].Name+" \nDelta lat: "+ deltaLat+"\n");
        //Console.WriteLine("\nFrome home to "+cragManager.crags[index].Name+" \nDelta lon: "+ deltaLon+"\n");

        //calculate a
        double a = Math.Sin(deltaLat/2)*Math.Sin(deltaLat/2)+Math.Cos(cragManager.crags[index].Latitude)*Math.Cos(cragManager.HomeLat)*Math.Sin(deltaLon/2)*Math.Sin(deltaLon/2);
        double c = 2*Math.Atan(Math.Sqrt(a)/Math.Sqrt(1-a));
        double d = c * EARTH_RADIUS_METERS;

        Console.WriteLine("Distance from "+cragManager.HomeName+" to "+cragManager.crags[index].Name+" is "+ d*.001f+"km.\n");
        return d;
    }

}