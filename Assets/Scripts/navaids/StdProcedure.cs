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

        public Navaid GetNavaid(string id)
        {
            int index = navaids.FindIndex(x => x.id == id);
            return navaids[index];
        }

        public void RemoveNavaid(string id)
        {
            int index = navaids.FindIndex(x => x.id == id);
            if (index != -1)
                navaids.RemoveAt(index);

            if (navaids.Count == 0)
                navaids = null;            
        }

        
        public StdProcedure CopyStdProcedure(StdProcedure origStdProcedure, Navaid initPoint)
        {
            // Authorized point can be null to define entire route
            bool initialPointFound = (initPoint == null);
            //bool initialPointFound = false;

            string name = origStdProcedure.GetName();
            List<Navaid> navaidsToCopy = new List<Navaid>();

            foreach (Navaid navaid in origStdProcedure.GetNavaids())
            {
                if ((initPoint != null) && (navaid.GetId() == initPoint.GetId()))
                {
                    initialPointFound = true;
                }
                if (initialPointFound)
                {
                    navaidsToCopy.Add(navaid);
                }
            }
            StdProcedure newStdProcedure = new StdProcedure(name, navaidsToCopy);

            return newStdProcedure;
        }

        public string GetName() { return name; }
        public List<Navaid> GetNavaids() { return navaids; }
        public ushort GetNumberOfPoints() { return (ushort) this.GetNavaids().Count; }


    }


