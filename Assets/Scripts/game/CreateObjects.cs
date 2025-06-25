using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public static Airport airport;
    public static List<Company> companyList;
    public static List<Aircraft> aircraftList;
    public static Dictionary<string, FIX> fixList;
    public static Dictionary<string, VOR> vorList;
    public static List<Navaid> navaidList;
    public static List<Navaid> temp_navaid_list;

    private static FIX aux_fix;
    private static VOR aux_vor;
    private static Navaid aux_navaid;
    private static string id_str;


    public static List<STAR> starList;
    public static List<SID> sidList;

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
        Debug.Log("Creating objects...");

        // Runways
        Runway[] rwys = new Runway[8];
        
        rwys[0] = new Runway("14R", 143, 13083, Measurement.DMS2DD(40, 29, 05.50f), Measurement.DMS2DD(-3, 34, 33.64f), 2000f, false);
        rwys[1] = new Runway("32L", 323, 13083, Measurement.DMS2DD(40, 27, 47.10f), Measurement.DMS2DD(-3, 33, 14.02f), 2000f, true);

        rwys[2] = new Runway("14L", 143, 11482, Measurement.DMS2DD(40, 29, 41.71f), Measurement.DMS2DD(-3, 33, 28.33f), 2000f, false);
        rwys[3] = new Runway("32R", 323, 11482, Measurement.DMS2DD(40, 28, 24.85f), Measurement.DMS2DD(-3, 32, 10.30f), 2000f, true);

        rwys[4] = new Runway("36L", 360, 11482, Measurement.DMS2DD(40, 29, 33.32f), Measurement.DMS2DD(-3, 34, 28.64f), 2000f, false);
        rwys[5] = new Runway("18R", 180, 11482, Measurement.DMS2DD(40, 31, 22.40f), Measurement.DMS2DD(-3, 34, 29.27f), 2000f, true);

        rwys[6] = new Runway("36R", 360, 11482, Measurement.DMS2DD(40, 30, 03.97f), Measurement.DMS2DD(-3, 33, 33.15f), 2000f, false);
        rwys[7] = new Runway("18L", 180, 11482, Measurement.DMS2DD(40, 31, 41.22f), Measurement.DMS2DD(-3, 33, 33.68f), 2000f, true);


        // Airport
        airport = new Airport("Madrid-Barajas", "LEMD", "MAD", Measurement.DMS2DD(40, 28, 20f), Measurement.DMS2DD(-3, 33, 39f), 2000f, "Madrid", "Spain", 13000, 140, rwys);


        fixList = new Dictionary<string, FIX>();
        vorList = new Dictionary<string, VOR>();

        temp_navaid_list = new List<Navaid>();

        // Testing
        /*
        fixList.Add("ASBIN", new FIX("ASBIN", Measurement.DMS2DD(40, 15, 18f), Measurement.DMS2DD(-3, 10, 35f), FIX.FixTypes.Compulsory));
        fixList.Add("TOBEK", new FIX("TOBEK", Measurement.DMS2DD(40, 11, 47f), Measurement.DMS2DD(-3, 25, 28f), FIX.FixTypes.Compulsory));
        fixList.Add("PRADO", new FIX("PRADO", Measurement.DMS2DD(40, 08, 51f), Measurement.DMS2DD(-2, 00, 37f), FIX.FixTypes.Compulsory));
        fixList.Add("MORAL", new FIX("MORAL", Measurement.DMS2DD(39, 00, 00f), Measurement.DMS2DD(-3, 32, 32f), FIX.FixTypes.Compulsory));
        fixList.Add("RIDAV", new FIX("RIDAV", Measurement.DMS2DD(40, 32, 07f), Measurement.DMS2DD(-5, 48, 30f), FIX.FixTypes.Compulsory));
        
        fixList.Add("AUX1", new FIX("AUX1", Measurement.DMS2DD(40, 00, 00f), Measurement.DMS2DD(-4, 00, 00f), FIX.FixTypes.OnRequest));
        fixList.Add("AUX2", new FIX("AUX2", Measurement.DMS2DD(40, 00, 00f), Measurement.DMS2DD(-3, 00, 00f), FIX.FixTypes.OnRequest));
        fixList.Add("AUX3", new FIX("AUX3", Measurement.DMS2DD(39, 00, 00f), Measurement.DMS2DD(-3, 00, 00f), FIX.FixTypes.OnRequest));

        */

        // LEMD - STAR 3 RNAV
        fixList.Add("AVILA", new FIX("AVILA", Measurement.DMS2DD(40, 37, 28.6f), Measurement.DMS2DD(-4, 32, 59.6f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BUREX", new FIX("BUREX", Measurement.DMS2DD(39, 48, 39.8f), Measurement.DMS2DD(-3, 56, 21.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("DAQSE", new FIX("DAQSE", Measurement.DMS2DD(40, 20, 35.1f), Measurement.DMS2DD(-4, 08, 48.1f), FIX.FixTypes.Compulsory, true));
        fixList.Add("FAFEQ", new FIX("FAFEQ", Measurement.DMS2DD(40, 10, 09.8f), Measurement.DMS2DD(-3, 27, 38.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("MORAL", new FIX("MORAL", Measurement.DMS2DD(39, 00, 00.0f), Measurement.DMS2DD(-3, 32, 31.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("NONTU", new FIX("NONTU", Measurement.DMS2DD(41, 30, 01.1f), Measurement.DMS2DD(-4, 10, 08.4f), FIX.FixTypes.OnRequest, true));
        fixList.Add("ORBIS", new FIX("ORBIS", Measurement.DMS2DD(41, 15, 56.6f), Measurement.DMS2DD(-4, 11, 43.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("RIDAV", new FIX("RIDAV", Measurement.DMS2DD(40, 32, 06.9f), Measurement.DMS2DD(-5, 48, 29.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("SOTUK", new FIX("SOTUK", Measurement.DMS2DD(39, 11, 37.2f), Measurement.DMS2DD(-4, 44, 47.0f), FIX.FixTypes.OnRequest, true));
        fixList.Add("URRIF", new FIX("URRIF", Measurement.DMS2DD(40, 14, 32.3f), Measurement.DMS2DD(-3, 44, 46.7f), FIX.FixTypes.OnRequest, true));
        fixList.Add("YUNYE", new FIX("YUNYE", Measurement.DMS2DD(40, 02, 38.7f), Measurement.DMS2DD(-3, 37, 44.2f), FIX.FixTypes.OnRequest, true));


        //vorList.Add("ZMR", new VOR("ZMR", "Zamora", 0.0f, null, true, Measurement.DMS2DD(41, 31, 48.2f), Measurement.DMS2DD(-5, 38, 23.1f)));




        // LEMD - STAR 4 RNAV
        fixList.Add("ADUXO", new FIX("ADUXO", Measurement.DMS2DD(40, 30, 44.4f), Measurement.DMS2DD(-2, 03, 51.4f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BANEV", new FIX("BANEV", Measurement.DMS2DD(41, 30, 09.4f), Measurement.DMS2DD(-2, 30, 52.3f), FIX.FixTypes.OnRequest, true));
        fixList.Add("RUDBI", new FIX("RUDBI", Measurement.DMS2DD(40, 15, 29.4f), Measurement.DMS2DD(-3, 08, 10.0f), FIX.FixTypes.Compulsory, true));
        fixList.Add("NOSKO", new FIX("NOSKO", Measurement.DMS2DD(40, 39, 22.8f), Measurement.DMS2DD(-2, 49, 00.2f), FIX.FixTypes.Compulsory, true));
        fixList.Add("PINAR", new FIX("PINAR", Measurement.DMS2DD(40, 58, 49.1f), Measurement.DMS2DD(-2, 35, 57.0f), FIX.FixTypes.OnRequest, true));
        fixList.Add("PRADO", new FIX("PRADO", Measurement.DMS2DD(40, 08, 51.0f), Measurement.DMS2DD(-2, 00, 37.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("SIRGU", new FIX("SIRGU", Measurement.DMS2DD(40, 15, 37.8f), Measurement.DMS2DD(-2, 36, 00.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("TERSA", new FIX("TERSA", Measurement.DMS2DD(40, 43, 30.1f), Measurement.DMS2DD(-2, 08, 16.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("VILLA", new FIX("VILLA", Measurement.DMS2DD(40, 13, 58.6f), Measurement.DMS2DD(-2, 24, 37.6f), FIX.FixTypes.OnRequest, true));

        // LEMD - VOR   
        vorList.Add("SSY", new VOR("SSY", "San Sebastian de los Reyes", 117.85f, "... ... -.--", true, Measurement.DMS2DD(40, 32, 47.1f), Measurement.DMS2DD(-3, 34, 30.7f), false));
        vorList.Add("BRA", new VOR("BRA", "Barajas", 116.45f, "-... .-. .-", true, Measurement.DMS2DD(40, 28, 08.9f), Measurement.DMS2DD(-3, 33, 27.1f), false));
        vorList.Add("CNR", new VOR("CNR", "Colmenar Viejo", 117.30f, "-.-. -. .-.", true, Measurement.DMS2DD(40, 38, 45.5f), Measurement.DMS2DD(-3, 44, 09.0f), true));
        vorList.Add("PDT", new VOR("PDT", "Perales", 116.75f, ".--. -.. -", true, Measurement.DMS2DD(40, 15, 10f), Measurement.DMS2DD(-3, 20, 52f), true));
        vorList.Add("TLD", new VOR("TLD", "Toledo", 113.20f, "- .-.. -..", true, Measurement.DMS2DD(39, 58, 10.1f), Measurement.DMS2DD(-4, 20, 14.6f), true));
        vorList.Add("RBO", new VOR("RBO", "Robledillo", 113.95f, ".-. -... ---", true, Measurement.DMS2DD(40, 51, 13.9f), Measurement.DMS2DD(-3, 14, 47.9f), true));

        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-7, 00, 00), 0));
        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-3, 33, 39)+3.439167, 0));

        // STARs
        starList = new List<STAR>();
        // STAR - TLD3C
        temp_navaid_list.Add(GetVOR("TLD"));
        //temp_navaid_list.Add(GetFIX("MD445"));
        temp_navaid_list.Add(GetFIX("BUREX"));
        //temp_navaid_list.Add(GetFIX("MD440"));
        temp_navaid_list.Add(GetFIX("YUNYE"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));

        starList.Add(new STAR("TLD3C", new List<Navaid>(temp_navaid_list)));
        //Debug.Log(temp_navaid_list.Count);
        temp_navaid_list.Clear();




        // SIDs
        sidList = new List<SID>();
        // SID - PINAR4N
        temp_navaid_list.Add(GetVOR("RBO"));
        temp_navaid_list.Add(GetFIX("PINAR"));

        sidList.Add(new SID("PINAR2R", new List<Navaid>(temp_navaid_list)));
        //Debug.Log(temp_navaid_list.Count);
        temp_navaid_list.Clear();





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
            GetVOR("TLD") as Navaid,
            Aircraft.FlightStatus.Arrival
        ));
        /*
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
            CreateObjects.fixList[1],
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
            CreateObjects.fixList[2],
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
            CreateObjects.fixList[3],
            Aircraft.FlightStatus.Incoming
        ));
        */
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

        foreach (FIX fix in fixList.Values)
        {
            fix.SetGameObjectPos();
        }

        foreach (VOR vor in vorList.Values)
        {
            vor.SetGameObjectPos();
        }

    }

    private static FIX GetFIX(string id_str)
    {
        if (fixList.TryGetValue(id_str, out aux_fix))
        {
            return aux_fix;
        }
        else
        {
            return null;
        }
    }

    private static VOR GetVOR(string id_str)
    {
        if (vorList.TryGetValue(id_str, out aux_vor))
        {
            return aux_vor;
        }
        else
        {
            return null;
        }
    }

}
