using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Points to navigation.
 *
 * @module Navaids
 * @class FIX
 * @date September 09, 2013
 * @author Jaime Valle Alonso
 */

public class FIX /*: ScriptableObject*/
{

    /**
	 * Type of FIX.
	 * For example: <i>ASBIN</i> is 'Compulsory' because position reporting is required.<br>
	 * <i>PRADO</i> is 'OnRequest' because position reporting is not obligatory.
	 * @attribute name
	 * @type {enum}
	 */
    public enum FixTypes { Compulsory, OnRequest };

    /**
	 * Name of this FIX.
	 * For example: <i>ASBIN</i> FIX.
	 * @attribute name
	 * @type {string}
	 */
    string name;
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
	 * Position: latitude, longitude (in degrees).
	 * @attribute position
	 * @type {Vector2}
	 */
    Vector2 position;
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
	 * Type of fix: mandatory or informative.
	 * @attribute type
	 * @type {FixTypes}
	 */
    FixTypes type;
    /**
	 * GameObject to represent graphically this class.
	 * @attribute go
	 * @type {GameObject}
	 */
    GameObject go;

	/**
	 * @class FIX
	 * @constructor
	 * @param {string} id Three letters identifier for this FIX.
	 * @param {string} name Name of this FIX.
	 * @param {float} frequency Frequency in MHz of this FIX.
	 * @param {string} morseCode Morse identifier of this FIX.
	 * @param {float} lat Latitude coordinates in degrees.
	 * @param {float} lon Longitude coordinates in degrees.
	 * @param {ushort} elevation Elevation in feet of airport field referred to measured sea level (MSL).
	 * @param {FixTypes} type Type of fix: mandatory or informative.
	 */
	public FIX(string name, float lat, float lon, FixTypes type)
    {

        this.name = name;
        this.lat = lat;
        this.lon = lon;
        this.position = new Vector2(lat, lon);
		
        this.type = type;
		
        if (this.type == FixTypes.Compulsory)
        {
            this.icon = DrawRadarScreen.icons["fix_filled"];
        }
        else
        {
            this.icon = DrawRadarScreen.icons["fix_empty"];
        }

        this.go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        UnityEngine.Object.Destroy(this.go.GetComponent<Collider>());
		this.go.name = this.GetType() + "_" + this.name;
        this.go.GetComponent<Renderer>().material.mainTexture = this.icon;
        this.go.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
		//this.go.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 1f);
		/*
		Color auxColor = this.go.GetComponent<Renderer>().material.color;
		auxColor.a = 1f;
		this.go.GetComponent<Renderer>().material.color = auxColor;
		*/
        this.go.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        this.go.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    void SetGameObjectPos()
    {
        //		this.screenPosition = MngScreen.ScreenPosRelToAirport(this.lon, this.lat, -1);
        this.screenPosition = MngScreen.RadarScreenPosRelToAirport(this.lon, this.lat, 0);

        this.go.transform.position = this.screenPosition;
        this.position = this.go.transform.position;
    }

	public float GetLat() { return lat; }
	public float GetLon() { return lon; }
	public string GetName() { return name; }
	public Vector2 GetScreenPosition() { return screenPosition; }
	public GameObject GetGO() { return this.go; }

}


