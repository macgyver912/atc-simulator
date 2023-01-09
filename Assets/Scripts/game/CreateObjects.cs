using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public static Airport airport;
    public static List<Company> companyList;
    public static List<Aircraft> aircraftList;
    public static List<FIX> fixList;
    public static List<VOR> vorList;

    Quaternion fromRotation;

    void Update()
    {
        //	aircraft.gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 20);
        //	
        //	aircraft.gameObject.transform.rotation.x +=  Time.deltaTime * 20;

        //	transform.rotation =
        //		  Quaternion.Lerp (from.rotation, to.rotation, Time.time * speed);

        //	var desiredRotation : float = 270;
        //	
        //	var toRotation = aircraft.gameObject.transform.rotation;
        //	toRotation.x = desiredRotation;
        //	aircraft.gameObject.transform.rotation =
        //		  Quaternion.Lerp (fromRotation, toRotation, Time.time * 0.1);
    }

    public static void Init()
    {
        Debug.Log("Hello World!");

        
        Runway[] rwys = new Runway[4];
        rwys[0] = new Runway("14R", 144, 13083, Measurement.DMS2DD(40, 29, 05.50f), Measurement.DMS2DD(-3, 34, 33.64f), 2000f);
        rwys[1] = new Runway("32L", 324, 13083, Measurement.DMS2DD(40, 27, 47.10f), Measurement.DMS2DD(-3, 33, 14.02f), 2000f);
        rwys[2] = new Runway("14L", 144, 11482, Measurement.DMS2DD(40, 29, 41.71f), Measurement.DMS2DD(-3, 33, 28.33f), 2000f);
        rwys[3] = new Runway("32R", 324, 11482, Measurement.DMS2DD(40, 28, 24.85f), Measurement.DMS2DD(-3, 32, 10.30f), 2000f);

        airport = new Airport("Madrid-Barajas", "LEMD", "MAD", Measurement.DMS2DD(40, 28, 20f), Measurement.DMS2DD(-3, 33, 39f), 2000f, "Madrid", "Spain", 13000, 140, rwys);

        fixList = new List<FIX>();
      
        fixList.Add(new FIX("ASBIN", Measurement.DMS2DD(40, 15, 18f), Measurement.DMS2DD(-3, 10, 35f), FIX.FixTypes.Compulsory));
        fixList.Add(new FIX("TOBEK", Measurement.DMS2DD(40, 11, 47f), Measurement.DMS2DD(-3, 25, 28f), FIX.FixTypes.Compulsory));
        fixList.Add(new FIX("PRADO", Measurement.DMS2DD(40, 08, 51f), Measurement.DMS2DD(-2, 00, 37f), FIX.FixTypes.Compulsory));
        fixList.Add(new FIX("MORAL", Measurement.DMS2DD(39, 00, 00f), Measurement.DMS2DD(-3, 32, 32f), FIX.FixTypes.Compulsory));
        fixList.Add(new FIX("RIDAV", Measurement.DMS2DD(40, 32, 07f), Measurement.DMS2DD(-5, 48, 30f), FIX.FixTypes.Compulsory));

        fixList.Add(new FIX("AUX1", Measurement.DMS2DD(40, 00, 00f), Measurement.DMS2DD(-4, 00, 00f), FIX.FixTypes.OnRequest));
        fixList.Add(new FIX("AUX2", Measurement.DMS2DD(40, 00, 00f), Measurement.DMS2DD(-3, 00, 00f), FIX.FixTypes.OnRequest));
        fixList.Add(new FIX("AUX3", Measurement.DMS2DD(39, 00, 00f), Measurement.DMS2DD(-3, 00, 00f), FIX.FixTypes.OnRequest));

        vorList = new List<VOR>();
        vorList.Add(new VOR("PDT", "Perales", 116.75f, null, true, Measurement.DMS2DD(40, 15, 10f), Measurement.DMS2DD(-3, 20, 52f), 0));
        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-7, 00, 00), 0));
        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-3, 33, 39)+3.439167, 0));
        
        companyList = new List<Company>();
        aircraftList = new List<Aircraft>();

        companyList.Add(new Company("Iberia Lineas Aereas de España", "IBERIA", "IBE"));
      
        aircraftList.Add(new Aircraft(
            "Airbus A320-214",
            "A320",
            Aircraft.Category.Medium,
            companyList[0],
            "5472",
            null,
            4257,
            Measurement.DMS2DD(40, 47, 00),
            Measurement.DMS2DD(-3, 56, 00),
            360,
            0,
            240,
            0,
            0,
            0,
            6000,
            0,
            0,
            15000,
            220,
            "PDT",
            Aircraft.FlightStatus.Arrival
        ));

        aircraftList.Add(new Aircraft(
            "Boeing 747-300",
            "B743",
            Aircraft.Category.Heavy,
            companyList[0],
            "6112",
            null,
            4358,
            Measurement.DMS2DD(39, 47, 00),
            Measurement.DMS2DD(-4, 56, 00),
            180,
            0,
            200,
            0,
            0,
            0,
            7000,
            0,
            0,
            7000,
            200,
            "TOBEK",
            Aircraft.FlightStatus.Departure
        ));

        aircraftList.Add(new Aircraft(
            "Boeing 737-800",
            "B738",
            Aircraft.Category.Medium,
            companyList[0],
            "23BZ",
            null,
            4726,
            Measurement.DMS2DD(39, 00, 00),
            Measurement.DMS2DD(-1, 30, 00),
            090,
            0,
            400,
            0,
            0,
            0,
            4000,
            0,
            0,
            4000,
            230,
            "PDT",
            Aircraft.FlightStatus.Transferred
        ));

        aircraftList.Add(new Aircraft(
            "Airbus A380",
            "A380",
            Aircraft.Category.Heavy,
            companyList[0],
            "625C",
            null,
            5532,
            Measurement.DMS2DD(40, 02, 02),
            Measurement.DMS2DD(-3, 36, 10),
            270,
            0,
            240,
            0,
            0,
            0,
            6000,
            0,
            0,
            6000,
            240,
            "ASBIN",
            Aircraft.FlightStatus.Incoming
        ));

        //var aircraft = ScriptableObject.CreateInstance<Aircraft>();

        Debug.Log(companyList[0].GetCompanyName() + " | " + companyList[0].GetCallsign() + " | " + companyList[0].GetCallsignCode());
        //Debug.Log(aircraftList[0]);
    }

    /*
    public List<Aircraft> GetAircraftList()
    {
        return aircraftList;
    }

    public List<Company> GetCompanyList()
    {
        return companyList;
    }
    */

    public static void PrepareDraw()
    {
        foreach (Runway rwy in airport.GetRunways())
        {
            rwy.SetGameObjectPos();
        }

        foreach (Aircraft aircraft in aircraftList)
        {
            aircraft.SetGameObjectPos();
        }
        /*
        foreach (FIX fix in fixList)
        {
            fix.SetGameObjectPos();
        }

        foreach (VOR vor in vorList)
        {
            vor.SetGameObjectPos();
        }
        */
    }

    
}
