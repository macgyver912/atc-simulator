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

    public class STAR /*: ScriptableObject*/
    {

        /**
          * Name of this STAR.
          * For example: <i>SOTUK3C</i> FIX.
          * @attribute name
          * @type {string}
          */
        public string name;

        public List<Navaid> navaids;
        /**
	     * @class STAR
	     * @constructor
	     * @param {string} name Name of this STAR.
	     * @param {List<Navaid>} navaids List of navaids belonging to this STAR.
	     */
        public STAR(string name, List<Navaid> navaids)
        {

            this.name = name;
            this.navaids = navaids;
        }

        public string GetName() { return name; }
        public List<Navaid> GetNavaids() { return navaids; }


    }


