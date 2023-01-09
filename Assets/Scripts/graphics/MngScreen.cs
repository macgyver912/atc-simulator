using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MngScreen : MonoBehaviour
{

    private static MngScreen instance;

    private static List<float> lats;
    private static List<float> lons;


    private static float safeMargin = 0.1f;		// percent relative to screen (0.1 = 10%)
    private static Vector2 maxDistance;			// real world coordinates degrees
    static float latLonRatio;
    static float pixelRatio;
    static Vector2 ratio;					// pixels / real world degree

    static Vector2 radarScreenSize;
    //static Vector2 radarScreenScale = new Vector2(0.9, 1.0);
    static Vector2 radarScreenOffset;


    void Awake()
    {
        instance = this;
    }

    public static void Init()
    {

        lats = new List<float>();
        lons = new List<float>();

    //	radarScreenSize = new Vector2(Screen.width*radarScreenScale.x, Screen.height*radarScreenScale.y);
    //	radarScreenOffset = new Vector2(Screen.width * (1-radarScreenScale.x), Screen.height * (1-radarScreenScale.y));
    radarScreenSize = new Vector2(Screen.width - DrawGUI.GetWindowFPSWidth(), Screen.height);
        radarScreenOffset = new Vector2(DrawGUI.GetWindowFPSWidth(), 0);

        //	Debug.Log("Screen.width: " + Screen.width);
        //	Debug.Log("DrawGUI.windowFPSWidth: " + DrawGUI.windowFPSWidth);
        //	Debug.Log("radarScreenSize.x: " + (Screen.width - DrawGUI.windowFPSWidth));

        foreach (FIX fix in CreateObjects.fixList)
        {
            lats.Add(fix.GetLat());
            lons.Add(fix.GetLon());
        }

        foreach (VOR vor in CreateObjects.vorList)
        {
            lats.Add(vor.GetLat());
            lons.Add(vor.GetLon());
        }

        maxDistance = CalculateMaxDistance();

        latLonRatio = Measurement.Ratio_DegreeLon_div_DegreeLat();
        pixelRatio = (MngCamera.GetCamera().orthographicSize * 2) / MngCamera.GetCamera().pixelHeight;
        var pxDegreeRatio = CalculateRatioPx4Degree();
        ratio = new Vector2(pxDegreeRatio * latLonRatio, pxDegreeRatio) * pixelRatio;

#if DEBUG_MODE
	        Debug.Log("maxDistance: " + maxDistance);
	        Debug.Log("safeMargin (º): " + safeMargin);
	        Debug.Log("ratio: " + ratio);
	        Debug.Log("pixelRatio: " + pixelRatio);
#endif

    }

    public static float CalculateRatioPx4Degree()
    {
        float ratioX = ((radarScreenSize.x * (1 - safeMargin)) / 2) / maxDistance.x;
        float ratioY = ((radarScreenSize.y * (1 - safeMargin)) / 2) / maxDistance.y;

        ratioX /= latLonRatio;

        return Mathf.Min(ratioX, ratioY);
    }

    public static Vector2 CalculateMaxDistance()
    {

        float minLat = lats.Min();
        float minLon = lons.Min();
        float maxLat = lats.Max();
        float maxLon = lons.Max();

        //	Debug.Log("minLat: " + minLat);
        //	Debug.Log("minLon: " + minLon);
        float axisXdist = Mathf.Max(maxLon - CreateObjects.airport.GetLon(), CreateObjects.airport.GetLon() - minLon);
        float axisYdist = Mathf.Max(maxLat - CreateObjects.airport.GetLat(), CreateObjects.airport.GetLat() - minLat);

        //	Debug.Log("axisXdist: " + axisXdist);
        //	Debug.Log("axisYdist: " + axisYdist);

        return new Vector2(axisXdist, axisYdist);
        //	return new Vector2(Mathf.Ceil(axisXdist), Mathf.Ceil(axisYdist));

    }

    public static Vector3 ScreenPosRelToAirport(float x, float y, float z)
    {
        Vector3 pos = new Vector3();
        //	pos.x = (x - CreateObjects.airport.GetPosition().x) * MngScreen.ratio;
        //	pos.y = (y - CreateObjects.airport.GetPosition().y) * MngScreen.ratio;
        //	pos.z = (z - CreateObjects.airport.GetPosition().z) * 0;
        pos.x = Measurement.Degrees2Pixels(x - CreateObjects.airport.GetPosition().x).x;
        pos.y = Measurement.Degrees2Pixels(y - CreateObjects.airport.GetPosition().y).y;
        pos.z = 0;

        return pos;
    }

    public static Vector3 ScreenPosRelToAirport(Vector3 coordinates)
    {
        Vector3 pos = new Vector3();
        //	pos.x = (coordinates.x - CreateObjects.airport.GetPosition().x) * MngScreen.ratio;
        //	pos.y = (coordinates.y - CreateObjects.airport.GetPosition().y) * MngScreen.ratio;
        //	pos.z = (coordinates.z - CreateObjects.airport.GetPosition().z) * 0;

        pos.x = Measurement.Degrees2Pixels(coordinates.x - CreateObjects.airport.GetPosition().x).x;
        pos.y = Measurement.Degrees2Pixels(coordinates.y - CreateObjects.airport.GetPosition().y).y;
        pos.z = 0;

        return pos;
    }

    public static Vector2 ScreenPosAbsolute(float x, float y, float z)
    {
        Vector3 pos = ScreenPosRelToAirport(x, y, z);

        Vector3 screenPos = MngCamera.GetCamera().WorldToScreenPoint(pos);
        screenPos.y = radarScreenSize.y - screenPos.y;

        return screenPos;
    }

    public static Vector2 ScreenPosAbsolute(Vector3 coordinates)
    {
        Vector3 screenPos = MngCamera.GetCamera().WorldToScreenPoint(coordinates);
        screenPos.y = radarScreenSize.y - screenPos.y;
        return screenPos;
    }

    public static Vector2 GetScreenSizeOfGameObject(GameObject go)
    {
        var bounds = go.GetComponent<Renderer>().bounds;
        Vector3 origin = MngCamera.GetCamera().WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, 0.0f));
        Vector3 extents = MngCamera.GetCamera().WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, 0.0f));

        return new Vector2(extents.x - origin.x, extents.y - origin.y);
    }

    public static Vector4 GetGameObjectBounds(GameObject go)
    {
        var bounds = go.GetComponent<Renderer>().bounds;
        Vector3 origin = new Vector3(bounds.min.x, bounds.min.y, 0.0f);
        Vector3 extents = new Vector3(bounds.max.x, bounds.max.y, 0.0f);
        return new Vector4(origin.x, extents.x, origin.y, extents.y);
    }


    public static Vector3 GetScreenToWorldPoint(Vector3 screenPos) {
        Vector3 worldPos = MngCamera.GetCamera().ScreenToWorldPoint(screenPos);

        worldPos.y = -worldPos.y;
        worldPos.z = 0;

        return worldPos;
    }


    public static Vector2 GetRadarScreenTopLeft()
    {
        return radarScreenOffset;
    }

    public static Vector2 GetRadarScreenTopRight()
    {
        return new Vector2(radarScreenOffset.x + radarScreenSize.x, radarScreenOffset.y);
    }

    public static Vector2 GetRadarScreenBottomLeft()
    {
        return new Vector2(radarScreenOffset.x, radarScreenSize.y + radarScreenOffset.y);
    }

    public static Vector2 GetRadarScreenBottomRight()
    {
        return radarScreenOffset + radarScreenSize;
    }

    public static Vector3 RadarScreenPosRelToAirport(float x, float y, float z)
    {
        Vector3 posRadar = ScreenPosRelToAirport(x, y, z);
        posRadar.x += radarScreenOffset.x / 2 * pixelRatio;
        posRadar.y += radarScreenOffset.y / 2 * pixelRatio;

        return posRadar;
    }

    public static Vector3 RadarScreenPosRelToAirport(Vector3 coordinates)
    {
        Vector3 posRadar = ScreenPosRelToAirport(coordinates);
        posRadar.x += radarScreenOffset.x / 2 * pixelRatio;
        posRadar.y += radarScreenOffset.y / 2 * pixelRatio;

        return posRadar;
    }

    //static Vector2 RadarScreenPosAbsolute(float x, float y, float z)
    //{
    //	Vector3 posRadar = ScreenPosRelToAirport(x, y, z);
    //	Vector3 screenPos = MngCamera.GetCamera().WorldToScreenPoint(posRadar);
    //	screenPos.y = radarScreenSize.y - screenPos.y; 
    //	
    //	return screenPos;
    //}
    //
    //static Vector2 RadarScreenPosAbsolute(Vector3 coordinates)
    //{
    //	Vector3 screenPos = MngCamera.GetCamera().WorldToScreenPoint(coordinates);
    //	screenPos.y = radarScreenSize.y - screenPos.y; 
    //	return screenPos;
    //}



    public static Vector2 GetRatio() { return ratio; }
    public static float GetPixelRatio() { return pixelRatio; }
    public static Vector2 GetRadarScreenOffset() { return radarScreenOffset; }

}


