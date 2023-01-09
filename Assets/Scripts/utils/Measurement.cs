using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measurement : ScriptableObject
{

    static float _EARTH_RADIUS = 6371f;	// in km
    static float _NM2KM = 1.852f;
    static float _FT2MT = 3.2808f;
    static float _DEGREE2NM = 60f;

    static Vector2 degree2km;
    static Vector2 degree2nm;

    static Vector2 km2degree;
    static Vector2 nm2degree;

    public static void Init()
    {
        //	Debug.Log("Distance (40, -3)-(40, -2): " + Distance_DD_KM( Vector3(40, -3), Vector3(40, -2) ) + " km");
        //	Debug.Log("Distance (70, -3)-(70, -2): " + Distance_DD_KM( Vector3(70, -3), Vector3(70, -2) ) + " km");
        //	Debug.Log("Distance AUX1 to AUX2: " + Distance_DD_KM( Vector3(40, -4), Vector3(40, -3) ) + " km");
        //	Debug.Log("Distance AUX2 to AUX3: " + Distance_DD_KM( Vector3(40, -3), Vector3(39, -3) ) + " km");
        //	Debug.Log("Length latitude: " + CalcLatLonLength_KM().x + " MT - " + CalcLatLonLength_NM().x + " NM");
        //	Debug.Log("Length longitude: " + CalcLatLonLength_KM().y + " MT - " + CalcLatLonLength_NM().y + " NM");
        //	Debug.Log(Ratio_DegreeLon_div_DegreeLat());


        degree2km = CalcLatLonLength_KM();
        degree2nm = CalcLatLonLength_NM();

        km2degree = new Vector2(1 / GetDegree_KM().x, 1 / GetDegree_KM().y);
        nm2degree = new Vector2(1 / GetDegree_NM().x, 1 / GetDegree_NM().y);

        #if DEBUG_MODE
	        Debug.Log("Length latitude: " + GetDegree_KM().y + " km - " + GetDegree_NM().y + " nm");
	        Debug.Log("Length longitude: " + GetDegree_KM().x + " km - " + GetDegree_NM().x + " nm");
	        Debug.Log("1 km is " + GetKM_Degree().y + "º of latitude and " + GetKM_Degree().x + "º of longitude");
	        Debug.Log("1 nm is " + GetNM_Degree().y + "º of latitude and " + GetNM_Degree().x + "º of longitude");
        #endif
    }


    // ############ SCREEN #############
    public static float DMS2DD(int d, int m, float s)
    {
	    float dd = Mathf.Sign(d) * (Mathf.Abs(d) + (m / 60f) + (s / 3600f));
    //	float dd = Mathf.Sign(d) * ( Mathf.Abs(d) + ( ( (m * 60) + s ) / 3600 ) );
	    return dd;
    }

    public static Vector2 DMS2DD(int lat_d, int lat_m, float lat_s, int lon_d, int lon_m, float lon_s)
    {
        float lat_dd = Mathf.Sign(lat_d) * (Mathf.Abs(lat_d) + (lat_m / 60f) + (lat_s / 3600f));
        float lon_dd = Mathf.Sign(lon_d) * (Mathf.Abs(lon_d) + (lon_m / 60f) + (lon_s / 3600f));
        //	return Vector2(lat_dd, lon_dd);
        return new Vector2(lon_dd, lat_dd);
    }

    public static Vector3 DD2DMS(float dd)
    {
        // TODO: Revise int or float
        float sign = Mathf.Sign(dd);
        dd = Mathf.Abs(dd);
        float d = Mathf.Floor(Mathf.Abs(dd));                     // degrees
        float m = (dd - d) * 60;                                  // minutes
        float s = (m - Mathf.Floor(m)) * 60;                      // seconds

        return new Vector3(sign * d, m, s);
    }

    public static Vector2 Pixels2Degrees(float pixels) 
    {
        return new Vector2(pixels / MngScreen.GetRatio().x, pixels / MngScreen.GetRatio().y);
    }

    public static Vector2 Degrees2Pixels(float degrees)
    {
        return new Vector2(degrees * MngScreen.GetRatio().x, degrees * MngScreen.GetRatio().y);
    }

    // ############ WORLD ############
    public static float NM2KM(float nm)
    {
        return nm * _NM2KM;
    }

    public static float KM2NM(float km)
    {
        return km / _NM2KM;
    }

    public static float FT2MT(float ft)
    {
        return ft / _FT2MT;
    }

    public static float MT2FT(float mt)
    {
        return mt * _FT2MT;
    }

    // RETURN LATERAL DISTANCE (NO ALTITUDE DIFFERENCE BECAUSE LATERALLY DISTANCE IS THE IMPORTANT DISTANCE IN RADAR TO ATC)
    // AND TWO AIRPLANES FLYING NEAR IN APPROACH WILL BE SIMILAR ALTITUDE
    public static float Distance_DD_NM(float lat1, float lon1, float lat2, float lon2)
    {
        return Distance_DD_NM(new Vector2(lat1, lon1), new Vector2(lat2, lon2));
    }

    public static float Distance_DD_KM(Vector2 pos1, Vector2 pos2)
    {
        float dLat = Mathf.Abs(pos1.x - pos2.x) * Mathf.Deg2Rad;
        float dLon = Mathf.Abs(pos1.y - pos2.y) * Mathf.Deg2Rad;
        float lat1 = pos1.x * Mathf.Deg2Rad;
        float lat2 = pos2.x * Mathf.Deg2Rad;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2) * Mathf.Cos(lat1) * Mathf.Cos(lat2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float d = _EARTH_RADIUS * c;

        return d;
    }

    public static float Distance_DD_NM(Vector2 pos1, Vector2 pos2)
    {
        return KM2NM(Distance_DD_KM(pos1, pos2));
    }

    public static Vector2 CalcLatLonLength_KM()
    {
        Airport refPos = CreateObjects.airport;

        float lat = refPos.GetLat() * Mathf.Deg2Rad;

        // Constants
        float m1 = 111132.92f;     // lat term1
        float m2 = -559.82f;       // lat term2
        float m3 = 1.175f;         // lat term3
        float m4 = -0.0023f;       // lat term4
        float p1 = 111412.84f;     // lon term1
        float p2 = -93.5f;         // lon term2
        float p3 = 0.118f;         // lon term3

        // Length of a degree of latitude and longitude in meters
        float latLen = m1 + (m2 * Mathf.Cos(2 * lat)) + (m3 * Mathf.Cos(4 * lat)) + (m4 * Mathf.Cos(6 * lat));
        float lonLen = (p1 * Mathf.Cos(lat)) + (p2 * Mathf.Cos(3 * lat)) + (p3 * Mathf.Cos(5 * lat));

        //	return Vector2(latLen, lonLen)/1000f;
        return new Vector2(lonLen, latLen) / 1000f;

    }

    public static Vector2 CalcLatLonLength_NM() 
    {
        return CalcLatLonLength_KM() / _NM2KM;
    }

    public static float Ratio_DegreeLon_div_DegreeLat()
    {
        Vector2 aux = CalcLatLonLength_KM();
        #if DEBUG_MODE
	        Debug.Log("Ratio_DegreeLon_div_DegreeLat: " + (aux.x / aux.y));
        #endif
        return aux.x / aux.y;
    }



    public static Vector2 GetDegree_KM()
    {
        return degree2km;
    }

    public static Vector2 GetDegree_NM()
    {
        return degree2nm;
    }

    public static Vector2 GetKM_Degree()
    {
        return km2degree;
    }

    public static Vector2 GetNM_Degree()
    {
        return nm2degree;
    }

}
