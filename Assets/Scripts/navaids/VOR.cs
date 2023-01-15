using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;


/**
 * Navigational aids, including radionavigation systems and fixes points.
 *
 * @module Navaids
 * @main Navaids
 * @class VOR
 * @date September 04, 2013
 * @author Jaime Valle Alonso
 */

public class VOR /*: ScriptableObject*/
{

    /**
	 * Three letters identifier for this VOR.
	 * For example: <i>PDT</i> for <i>Perales</i> VOR.
	 * @attribute id
	 * @type {string}
	 */
    string id;
	/**
	 * Name of this VOR.
	 * For example: <i>Perales</i> for <i>Perales</i> VOR.
	 * @attribute name
	 * @type {string}
	 */
	string name;
	/**
	 * Frequency in MHz of this VOR.
	 * For example: <i>116.95</i> for <i>Perales - PDT</i> VOR.
	 * @attribute frequency
	 * @type {float}
	 */
	float frequency;
    /**
	 * Morse identifier of this VOR.
	 * For example: <i>.--. -.. -</i> for <i>Perales - PDT</i> VOR.
	 * @attribute morseCode
	 * @type {string}
	 */
    string morseCode;
	/**
	 * Indicates if this VOR has DME associated.
	 * @attribute hasDME
	 * @type {boolean}
	 */
	bool hasDME;
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
	 * Elevation of this VOR.
	 * For example: <i>2560</i> ft (feet) for <i>Perales - PDT</i> VOR.
	 * @attribute elevation
	 * @type {float}
	 */
    short elevation;
    /**
	 * Position: latitude, longitude (in degrees).
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
	 * Icon to represent objects of this class.
	 * @attribute icon
	 * @type {Texture2D}
	 */
    Texture2D icon;
    /**
	 * GameObject to represent graphically this class.
	 * @attribute go
	 * @type {GameObject}
	 */
    GameObject go;
	
	/**
	 * @class VOR
	 * @constructor
	 * @param {string} id Three letters identifier for this VOR.
	 * @param {string} name Name of this VOR.
	 * @param {float} frequency Frequency in MHz of this VOR.
	 * @param {string} morseCode Morse identifier of this VOR.
	 * @param {float} lat Latitude coordinates in degrees.
	 * @param {float} lon Longitude coordinates in degrees.
	 * @param {ushort} elevation Elevation in feet of airport field referred to measured sea level (MSL).
	 */
	public VOR(string id, string name, float frequency, string morseCode, bool hasDME,
                float lat, float lon, short elevation)
    {

        this.id = id;
        this.name = name;
        this.frequency = frequency;
        this.morseCode = morseCode;
        this.hasDME = hasDME;
        this.lat = lat;
        this.lon = lon;
        this.elevation = elevation;
        this.position = new Vector3(lat, lon, elevation);

        string iconName;

        if (this.hasDME)
			//TODO:
            //if (PlayerPreferences.dme_rose)
            //    iconName = "vor_dme_rose";
            //else
                iconName = "vor_dme";
        else
            iconName = "vor";

        this.icon = DrawRadarScreen.icons[iconName];

        this.go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        this.go.name = this.GetType() + "_" + this.id;
        this.go.GetComponent<Renderer>().material.mainTexture = this.icon;
        //this.go.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        this.go.GetComponent<Renderer>().material.shader = Config.object_shader;
        //this.go.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 1f);
		/*
        Color auxColor = this.go.GetComponent<Renderer>().material.color;
        auxColor.a = 1f;
        this.go.GetComponent<Renderer>().material.color = auxColor;
		*/
        this.go.transform.rotation = Quaternion.Euler(90, 180, 0);

        if (iconName.Contains("rose"))
        {
            //this.go.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            this.go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Config.scale_vor_rose;
        }
		else
		{
            this.go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Config.scale_vor;
        }
    }


    public void SetGameObjectPos()
    {
        //		this.screenPosition = MngScreen.ScreenPosRelToAirport(this.lon, this.lat, 0);
        this.screenPosition = MngScreen.RadarScreenPosRelToAirport(this.lon, this.lat, 0);

        this.go.transform.position = this.screenPosition;
        this.position = this.go.transform.position;
    }

    public float GetLat() { return lat; }
    public float GetLon() { return lon; }
    public string GetName() { return name; }
	public string GetID() { return id; }
	public bool HasDME() { return hasDME; }
    public Vector2 GetScreenPosition() { return screenPosition; }
    public GameObject GetGO() { return this.go; }

}