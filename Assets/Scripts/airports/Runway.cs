using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma strict

/**
 * Runway data and details.
 *
 * @module Airport
 * @class Runway
 * @date December 11th, 2022
 * @author Jaime Valle Alonso
 */
public class Runway
{
    /**
	 * Identifier number and letter of runway. Note that number of runway is heading
	 * decimal truncated and rounded. Letter identifies if runway is Left, Center or Right
	 * if two or more runways are parallel.
	 * For example: runway 32L of Madrid-Barajas airport has magnetic heading of 324ยบ.
	 * @attribute id
	 * @type {String}
	 */
    string id;
    /**
	 * Heading refers to direction that runway is pointing.
	 * @attribute heading
	 * @type {ushort}
	 */
    ushort heading;
	/**
	 * Length of runway on meters. Its only used to information. Graphically,
	 * runway will be as long as difference between thresholds coordinates.
	 * @attribute length
	 * @type {ushort}
	 */
	ushort length;
	/**
	 * Latitude coordinates in degrees of runway threshold.
	 * @attribute thrLat
	 * @type {float}
	 */
	float thrLat;
	/**
	 * Longitude coordinates in degrees of runway threshold.
	 * @attribute thrLon
	 * @type {float}
	 */	
	float thrLon;
	/**
	 * Elevation in feet of airport field referred to measured sea level (MSL).
	 * @attribute elevation
	 * @type {ushort}
	 */	
	float elevation;
	/**
	 * Position: latitude, longitude and elevation. <br><br>
	 * <b>NOTE: currently elevation of runways is equals to airport field elevation.</b>
	 * @attribute thrPosition
	 * @type {Vector3}
	 */
	Vector3 thrPosition;
	/**
	 * Position in screen (in pixels): latitude, longitude.
	 * @attribute screenPosition
	 * @type {Vector3}
	 */
	Vector3 screenPosition;
//	/**
//	 * GameObject to represent the ILS.
//	 * @attribute ILS_GO
//	 * @type {GameObject}
//	 */	
//	var ILS_GO : GameObject;
	/**
	 * Transform of prefab instance of ILS.
	 * @attribute ILS_Instance
	 * @type {Transform}
	 */	
	Transform ILS_Instance;
	
	/**
	 * @class Runway
	 * @constructor
	 * @param {String} id Identifier number and letter of runway.
	 * @param {ushort} heading Heading refers to direction that runway is pointing.
	 * @param {ushort} length Length of runway on meters.
	 * @param {float} thrLat Latitude coordinates in degrees of runway threshold.
	 * @param {float} thrLon Longitude coordinates in degrees of runway threshold.
	 * @param {float} elevation Elevation in feet of airport field referred to measured sea level (MSL).
	 */
	public Runway(string id, ushort heading, ushort length, float thrLat, float thrLon, float elevation)
    {
        this.id = id;
        this.heading = heading;
        this.length = length;
        this.thrLat = thrLat;
        this.thrLon = thrLon;
        this.elevation = elevation;
        this.thrPosition = new Vector3(thrLon, thrLat, elevation);

        this.ILS_Instance = CreateInstance.CreateILSInstance();
        //		this.ILS_Instance = Instantiate(this.ILS_Instance, Vector3.zero, Quaternion.identity);
        this.ILS_Instance.transform.gameObject.name = this.id;
    }

    public void SetGameObjectPos()
    {
        GameObject parentGO = GameObject.Find("ILSs");
        if (parentGO == null)
        {
            parentGO = new GameObject("ILSs");
            parentGO.transform.parent = CreateObjects.airport.GetGO().transform;
        }
        this.ILS_Instance.transform.parent = parentGO.transform;

        //		this.screenPosition = MngScreen.ScreenPosRelToAirport(this.lon, this.lat, -1);
        this.screenPosition = MngScreen.RadarScreenPosRelToAirport(this.thrLon, this.thrLat, 0);

        this.ILS_Instance.transform.position = this.screenPosition;
        //		this.thrPosition = this.ILS_Instance.transform.position;

        //		Debug.LogWarning("RWY HDG: " + this.heading);

        this.ILS_Instance.transform.localRotation = Quaternion.Euler(0, 0, -this.heading);
        float scale = Config.ils_range * Measurement.GetNM_Degree().y * MngScreen.GetRatio().y / MngScreen.GetPixelRatio();
        //		Debug.LogWarning("scale: " + scale);
        this.ILS_Instance.transform.localScale = new Vector3(0.3f, scale, 1.0f);


        // Center ILS to RWY from ILS size (bounds.extents)	
        int sign_x;
        int sign_y;
        if (this.heading == 0 || this.heading == 360)
        {
            sign_x = 0;
            sign_y = -1;
        }
        else if (this.heading == 90)
        {
            sign_x = -1;
            sign_y = 0;
        }
        else if (this.heading == 180)
        {
            sign_x = 0;
            sign_y = 1;
        }
        else if (this.heading == 270)
        {
            sign_x = 1;
            sign_y = 0;
        }
        else if (this.heading > 0 && this.heading < 90)
        {
            sign_x = -1;
            sign_y = -1;
        }
        else if (this.heading > 90 && this.heading < 180)
        {
            sign_x = -1;
            sign_y = 1;
        }
        else if (this.heading > 180 && this.heading < 270)
        {
            sign_x = 1;
            sign_y = 1;
        }
        else
        {
            sign_x = 1;
            sign_y = -1;
        }

        //this.ILS_Instance.transform.localPosition.x += sign_x * this.ILS_Instance.gameObject.GetComponent<Renderer>().bounds.extents.x;
        //this.ILS_Instance.transform.localPosition.y += sign_y * this.ILS_Instance.gameObject.GetComponent<Renderer>().bounds.extents.y;
        Vector3 prev_pos = this.ILS_Instance.transform.localPosition;
        this.ILS_Instance.transform.localPosition = new Vector3(
             prev_pos.x + sign_x * this.ILS_Instance.gameObject.GetComponent<Renderer>().bounds.extents.x,
             prev_pos.y + sign_y * this.ILS_Instance.gameObject.GetComponent<Renderer>().bounds.extents.y, 
             0);
    }

    public string GetID() { return this.id; }
    public ushort GetHeading() { return this.heading; }
    public ushort GetLength() { return this.length; }
    public float GetThrLat() { return this.thrLat; }
    public float GetThrLon() { return this.thrLon; }
    public float GetElevation() { return this.elevation; }
    public Vector3 GetThrPosition() { return this.thrPosition; }
    public Vector3 GetScreenPosition() { return this.screenPosition; }


}// end-class
