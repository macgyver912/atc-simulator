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
 * @class VOR2
 * @date September 04, 2013
 * @author Jaime Valle Alonso
 */

public class VOR : Navaid /*: ScriptableObject*/
{

    /**
	 * Three letters identifier for this VOR.
	 * For example: <i>PDT</i> for <i>Perales</i> VOR.
	 * @attribute id
	 * @type {string}
	 */
    //private string id;
    /**
	 * Frequency in MHz of this VOR.
	 * For example: <i>116.95</i> for <i>Perales - PDT</i> VOR.
	 * @attribute frequency
	 * @type {float}
	 */
    private float frequency;
    /**
	 * Morse identifier of this VOR.
	 * For example: <i>.--. -.. -</i> for <i>Perales - PDT</i> VOR.
	 * @attribute morseCode
	 * @type {string}
	 */
    private string morseCode;
    /**
	 * Indicates if this VOR has DME associated.
	 * @attribute hasDME
	 * @type {boolean}
	 */
    private bool hasDME;
    /*
	 * Elevation of this VOR.
	 * For example: <i>2560</i> ft (feet) for <i>Perales - PDT</i> VOR.
	 * @attribute elevation
	 * @type {float}
	 */
    //private short elevation;
    /**
	 * Position: latitude, longitude (in degrees).
	 * @attribute position
	 * @type {Vector3}
	 */
    //public VOR2(string id, string name, float frequency, string morseCode, bool hasDME,
    //            float lat, float lon, short elevation)
    //public VOR2(string name, float lat, float lon, FixTypes type, string id, float frequency, string morseCode, bool hasDME, short elevation)
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
                float lat, float lon, bool isVisible) : base(id, lat, lon, isVisible)
    {

        //this.id = id;
        this.frequency = frequency;
        this.morseCode = morseCode;
        this.hasDME = hasDME;
        //this.elevation = elevation;
        //this.position = new Vector3(lat, lon, elevation);

        string iconName;

        if (this.hasDME)
            iconName = "vor_dme";
        else
            iconName = "vor";

        this.icon = DrawRadarScreen.icons[iconName];

        this.go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        this.go.name = this.GetType() + "_" + this.id;

        if (this.IsVisible())
        {
            this.go.GetComponent<Renderer>().material.mainTexture = this.icon;
            this.go.GetComponent<Renderer>().material.shader = Config.object_shader;
            this.go.transform.rotation = Quaternion.Euler(90, 180, 0);
        }
        

        if (iconName.Contains("rose"))
        {
            //this.go.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            this.go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Config.scale_vor_rose;
        }
        else
        {
            this.go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Config.scale_vor;
        }

        // Locate GameObject inside "VORs" GameObject
        GameObject parentGO = GameObject.Find("VOR_List");
        if (parentGO == null)
        {
            parentGO = new GameObject("VOR_List");
        }
        this.go.transform.parent = parentGO.transform;
    }

    public string GetID() { return id; }
	public bool HasDME() { return hasDME; }

}