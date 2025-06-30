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

public class SID : StdProcedure
{

    /**
	 * @class SID
	 * @constructor
	 * @param {string} name Name of this SID.
	 * @param {List<Navaid>} navaids List of navaids belonging to this SID.
	 */
    public SID(string name, List<Navaid> navaids) : base(name, navaids)
    {
        // Empty constructor (derived to StdProcedure)
    }

}


