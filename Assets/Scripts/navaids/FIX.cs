using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Points to navigation.
 *
 * @module Navaids
 * @class FIX2
 * @date September 09, 2013
 * @author Jaime Valle Alonso
 */

public class FIX : Navaid /*: ScriptableObject*/
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
	 * Type of fix: mandatory or informative.
	 * @attribute type
	 * @type {FixTypes}
	 */
    FixTypes type;

	/**
	 * @class FIX
	 * @constructor
	 * @param {string} id Five letters identifier for this FIX.
	 * @param {string} name Name of this FIX.
	 * @param {string} morseCode Morse identifier of this FIX.
	 * @param {float} lat Latitude coordinates in degrees.
	 * @param {float} lon Longitude coordinates in degrees.
	 * @param {FixTypes} type Type of fix: mandatory or informative.
	 */
	public FIX(string id, float lat, float lon, FixTypes type, bool isVisible) : base(id, lat, lon, isVisible)
	{
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
		this.go.name = this.GetType() + "_" + this.id;
		if (this.IsVisible())
		{
			this.go.GetComponent<Renderer>().material.mainTexture = this.icon;
			this.go.GetComponent<Renderer>().material.shader = Config.object_shader;
            this.go.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
            this.go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Config.scale_fix;
        }
        

        // Locate GameObject inside "FIXs" GameObject
        GameObject parentGO = GameObject.Find("FIX_List");
        if (parentGO == null)
        {
            parentGO = new GameObject("FIX_List");
		}
		this.go.transform.parent = parentGO.transform;
    }

 


}


