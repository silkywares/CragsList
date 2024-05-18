using System;
/*
a = sin^2((Δlat)/2)+cos(lat_1)cos(lat_2)sin^2((Δlon)/2)
c = 2arctan(√a/√1-a)
d = Rc
R_meters = 6.378E6
*/
class Haversine{

    const float EARTH_RADIUS_METERS = 6378000F;
    public static double ReturnHaversineDistance(CragManager cragManager, int index)
    {
        // Convert latitude and longitude from degrees to radians
        float homeLatRad = (float)(cragManager.HomeLat * (Math.PI / 180.0));
        float homeLonRad = (float)(cragManager.HomeLon * (Math.PI / 180.0));
        float cragLatRad = (float)(cragManager.crags[index].Latitude * (Math.PI / 180.0));
        float cragLonRad = (float)(cragManager.crags[index].Longitude * (Math.PI / 180.0));

        // Calculate Δlat and Δlon
        float deltaLat = cragLatRad - homeLatRad;
        float deltaLon = cragLonRad - homeLonRad;

        // Calculate a
        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(homeLatRad) * Math.Cos(cragLatRad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        // Calculate c
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate distance
        double d = c * EARTH_RADIUS_METERS;

        Console.WriteLine($"{cragManager.HomeName} to {cragManager.crags[index].Name} : {(d * 0.000001):F3} Mm.");
        

        return d;
    }

}