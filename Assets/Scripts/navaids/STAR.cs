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

    public class STAR : StdProcedure
    {

        /**
	     * @class STAR
	     * @constructor
	     * @param {string} name Name of this STAR.
	     * @param {List<Navaid>} navaids List of navaids belonging to this STAR.
	     */
        public STAR(string name, List<Navaid> navaids) : base(name, navaids)
        {

            // Empty constructor (derived to StdProcedure)
        }

    }


