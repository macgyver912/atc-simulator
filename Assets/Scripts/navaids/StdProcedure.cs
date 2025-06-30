    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /**
     * Points to navigation.
     *
     * @module Navaids
     * @class StdProcedure
     * @date June, 2025
     * @author Jaime Valle Alonso
     */
    public class StdProcedure /*: ScriptableObject*/
    {

        /**
          * Name of this Standard Procedure.
          * For example: <i>SOTUK3C</i> FIX.
          * @attribute name
          * @type {string}
          */
        public string name;

        public List<Navaid> navaids;
        /**
	     * @class StdProcedure
	     * @constructor
	     * @param {string} name Name of this Standard Procedure.
	     * @param {List<Navaid>} navaids List of navaids belonging to this Standard Procedure.
	     */
        public StdProcedure(string name, List<Navaid> navaids)
        {

            this.name = name;
            this.navaids = navaids;
        }

        public string GetName() { return name; }
        public List<Navaid> GetNavaids() { return navaids; }
        public ushort GetNumberOfPoints() { return (ushort) this.GetNavaids().Count; }


    }


