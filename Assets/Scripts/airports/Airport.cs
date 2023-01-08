using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Airport data and details.
 *
 * @module Airport
 * @main Airport
 * @class Airport
 * @date December 11th, 2022
 * @author Jaime Valle Alonso
 */
public class Airport
{

    /**
	 * Airport name.
	 * For example: <i>Madrid-Barajas</i>.
	 * @attribute name
	 * @type {String}
	 */
    string name;
	/**
	 * Airport identifier code by International Civil Aviation Organization.
	 * For example: <i>LEMD</i> for <i>Madrid-Barajas</i> airport.
	 * @attribute icao
	 * @type {String}
	 */
	string icao;
	/**
	 * Airport identifier code by International Air Transport Association.
	 * For example: <i>MAD</i> for <i>Madrid-Barajas</i> airport.
	 * @attribute iata
	 * @type {String}
	 */
	string iata;
	/**
	 * Latitude coordinates in degrees.
	 * @attribute lat
	 * @type {float}
	 */
	float lat;
	/**
	 * Longitude coordinates in degrees.
	 * @attribute lon
	 * @type {float}
	 */	
	float lon;
	/**
	 * Elevation in feet of airport field referred to measured sea level (MSL).
	 * @attribute elevation
	 * @type {float}
	 */	
	float elevation;
	/**
	 * Position: latitude, longitude (in degrees) and elevation (in feet).
	 * @attribute position
	 * @type {Vector3}
	 */
	Vector3 position;
	/**
	 * Position in screen (in pixels): latitude, longitude.
	 * @attribute screenPosition
	 * @type {Vector3}
	 */
	Vector3 screenPosition;
	/**
	 * City where airport is located.
	 * @attribute city
	 * @type {String}
	 */	
	string city;
	/**
	 * Country where airport is located.
	 * @attribute country
	 * @type {String}
	 */	
	string country;
	/**
	 * Runways of the airport.
	 * @attribute runways
	 * @type {Runway &#91;&#93;}
	 */	
	//var runways : List.<Runway>;
	Runway[] runways;			// Builtin arrays are very fast
//	/**
//	 * Icon to represent objects of this class.
//	 * @attribute icon
//	 * @type {Texture2D}
//	 */	
//	var icon : Texture2D;
	/**
	 * GameObject to represent graphically this class.
	 * @attribute go
	 * @type {GameObject}
	 */	
	GameObject go;

	ushort transAltitude;
	ushort transLevel;
	
	/**
	 * @class Airport
	 * @constructor
	 * @param {String} name Airport name.
	 * @param {String} icao Airport identifier code by International Civil Aviation Organization.
	 * @param {String} iata Airport identifier code by International Air Transport Association.
	 * @param {float} lat Latitude coordinates in degrees.
	 * @param {float} lon Longitude coordinates in degrees.
	 * @param {float} elevation Elevation in feet of airport field referred to measured sea level (MSL).
	 * @param {String} city City where airport is located.
	 * @param {String} country Country where airport is located.
	 * @param {Runway &#91;&#93;} runways Runways of the airport.
	 */
	public Airport(string name, string icao, string iata, float lat, float lon,
                float elevation, string city, string country, ushort transAltitude,
                ushort transLevel, Runway[] runways)
    {

        this.name = name;
        this.icao = icao;
        this.iata = iata;
        this.lat = lat;
        this.lon = lon;
        this.elevation = elevation;
        this.position = new Vector3(lon, lat, elevation);
        this.city = city;
        this.country = country;
        this.runways = runways;

        this.transAltitude = transAltitude;
        this.transLevel = transLevel;

        //		this.icon = DrawRadarScreen.icons["airport"];

        this.go = new GameObject("Airport_" + this.icao);
    }

	public string GetName() { return name; }
    public string GetCodeICAO() { return icao; }
    public string GetCodeIATA() { return iata; }
	public float GetLat() { return lat; }
	public float GetLon() { return lon; }
    public float GetElevation() { return elevation; }
    public Vector3 GetPosition() { return position; }
    public Vector3 GetScreenPosition() { return screenPosition; }
    public string GetCity() { return city; }
    public string GetCountry() { return country; }
    public Runway[] GetRunways() { return runways; }
    public GameObject GetGO() { return this.go; }
	public ushort GetTransAltitude() { return this.transAltitude; }
    public ushort GetTransLevel() { return this.transLevel; }

}// end-class
