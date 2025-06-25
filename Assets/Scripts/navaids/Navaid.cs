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

public class Navaid /*: ScriptableObject*/
{

    

    /**
	 * Identifier of this FIX.
	 * For example: <i>ASBIN</i>.
	 * @attribute id
	 * @type {string}
	 */
    public string id;
    /**
	 * Latitude coordinates in degrees.
	 * @attribute lat
	 * @type {float}
	 */
    public float lat;
	/**
	 * Longitude coordinates in degrees.
	 * @attribute lon
	 * @type {float}
	 */	
	public float lon;
    /**
	 * Position: latitude, longitude (in degrees).
	 * @attribute position
	 * @type {Vector2}
	 */
    public Vector2 position;
    /**
	 * Position in screen (in pixels): latitude, longitude.
	 * @attribute screenPosition
	 * @type {Vector3}
	 */
    public Vector3 screenPosition;
    /**
	 * Icon to represent objects of this class.
	 * @attribute icon
	 * @type {Texture2D}
	 */
    public Texture2D icon;
    /**
	 * GameObject to represent graphically this class.
	 * @attribute go
	 * @type {GameObject}
	 */
    public GameObject go;
    /**
	 * Indicates if this VOR should to be renderer or be hidden to avoid occlusion of an airport for example.
	 * @attribute isVisible
	 * @type {boolean}
	 */
    private bool isVisible;

    /**
	 * @class FIX
	 * @constructor
	 * @param {string} id Three or Five letters identifier for this VOR or FIX.
	 * @param {float} lat Latitude coordinates in degrees.
	 * @param {float} lon Longitude coordinates in degrees.
	 */
    public Navaid(string id, float lat, float lon, bool isVisible)
    {

        this.id = id;
        this.lat = lat;
        this.lon = lon;
        this.position = new Vector2(lat, lon);
		this.isVisible = isVisible;
		
		/*
        this.go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        UnityEngine.Object.Destroy(this.go.GetComponent<Collider>());
		this.go.name = this.GetType() + "_" + this.name;
        this.go.GetComponent<Renderer>().material.mainTexture = this.icon;
        //this.go.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        this.go.GetComponent<Renderer>().material.shader = Config.object_shader;
        //this.go.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        this.go.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        //this.go.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        this.go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Config.scale_fix;
		*/
    }

    public void SetGameObjectPos()
    {
        //		this.screenPosition = MngScreen.ScreenPosRelToAirport(this.lon, this.lat, -1);
        this.screenPosition = MngScreen.RadarScreenPosRelToAirport(this.lon, this.lat, 0);

        this.go.transform.position = this.screenPosition;
        this.position = this.go.transform.position;
    }
	/*
	public void SetLat(float lat) { this.lat = lat; }
    public void SetLat(float lon) { this.lon = lon; }
	*/
    public float GetLat() { return lat; }
	public float GetLon() { return lon; }
	public string GetId() { return id; }
	public Vector2 GetPosition() { return position; }
	public Vector2 GetScreenPosition() { return screenPosition; }
	public GameObject GetGO() { return this.go; }
	public bool IsVisible() { return this.isVisible; }

}


