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
        
        rwys[0] = new Runway("14R", 143, 13083, 40.290550f, -3.343364f, 2000f, false);
        rwys[1] = new Runway("32L", 323, 13083, 40.274710f, -3.331402f, 2000f, true);

        rwys[2] = new Runway("14L", 143, 11482, 40.294171f, -3.332833f, 2000f, false);
        rwys[3] = new Runway("32R", 323, 11482, 40.282485f, -3.321030f, 2000f, true);

        rwys[4] = new Runway("36L", 360, 11482, 40.293332f, -3.342864f, 2000f, false);
        rwys[5] = new Runway("18R", 180, 11482, 40.312240f, -3.342927f, 2000f, true);

        rwys[6] = new Runway("36R", 360, 11482, 40.300397f, -3.333315f, 2000f, false);
        rwys[7] = new Runway("18L", 180, 11482, 40.314122f, -3.333368f, 2000f, true);


        // Airport
        //airport = new Airport("Madrid-Barajas", "LEMD", "MAD", Measurement.DMS2DD(40, 28, 20f), Measurement.DMS2DD(-3, 33, 39f), 2000f, "Madrid", "Spain", 13000, 140, rwys);
        airport = new Airport("Madrid-Barajas", "LEMD", "MAD", 40.2820f, -3.3339f, 2000f, "Madrid", "Spain", 13000, 140, rwys);




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
        fixList.Add("AVILA", new FIX("AVILA", 40.37286f, -4.32596f, FIX.FixTypes.OnRequest));
        fixList.Add("BUREX", new FIX("BUREX", 39.48398f, -3.56215f, FIX.FixTypes.Compulsory));
        fixList.Add("DAQSE", new FIX("DAQSE", 40.20351f, -4.08481f, FIX.FixTypes.Compulsory));
        fixList.Add("FAFEQ", new FIX("FAFEQ", 40.10098f, -3.27385f, FIX.FixTypes.Compulsory));
        fixList.Add("MORAL", new FIX("MORAL", 39.00000f, -3.32318f, FIX.FixTypes.OnRequest));
        fixList.Add("NONTU", new FIX("NONTU", 41.30011f, -4.10084f, FIX.FixTypes.OnRequest));
        fixList.Add("ORBIS", new FIX("ORBIS", 41.15566f, -4.11432f, FIX.FixTypes.OnRequest));
        fixList.Add("RIDAV", new FIX("RIDAV", 40.32069f, -5.48298f, FIX.FixTypes.OnRequest));
        fixList.Add("SOTUK", new FIX("SOTUK", 39.11372f, -4.44470f, FIX.FixTypes.OnRequest));
        fixList.Add("URRIF", new FIX("URRIF", 40.14323f, -3.44467f, FIX.FixTypes.OnRequest));
        fixList.Add("YUNYE", new FIX("YUNYE", 40.02387f, -3.37442f, FIX.FixTypes.OnRequest));

        
        //vorList.Add("ZMR", new VOR("ZMR", "Zamora", 0.0f, null, true, 41.31482f, -5.38231f));




        // LEMD - STAR 4 RNAV
        fixList.Add("ADUXO", new FIX("ADUXO", 40.30444f, -2.03514f, FIX.FixTypes.OnRequest));
        fixList.Add("BANEV", new FIX("BANEV", 41.30094f, -2.30523f, FIX.FixTypes.OnRequest));
        fixList.Add("RUDBI", new FIX("RUDBI", 40.15294f, -3.08100f, FIX.FixTypes.Compulsory));
        fixList.Add("NOSKO", new FIX("NOSKO", 40.39228f, -2.49002f, FIX.FixTypes.Compulsory));
        fixList.Add("PINAR", new FIX("PINAR", 40.58491f, -2.35570f, FIX.FixTypes.OnRequest));
        fixList.Add("PRADO", new FIX("PRADO", 40.08510f, -2.00372f, FIX.FixTypes.OnRequest));
        fixList.Add("SIRGU", new FIX("SIRGU", 40.15378f, -2.36005f, FIX.FixTypes.Compulsory));
        fixList.Add("TERSA", new FIX("TERSA", 40.43301f, -2.08162f, FIX.FixTypes.OnRequest));
        fixList.Add("VILLA", new FIX("VILLA", 40.13586f, -2.24376f, FIX.FixTypes.OnRequest));

        // LEMD - VOR
        vorList.Add("SSY", new VOR("SSY", "San Sebastian de los Reyes", 117.85f, "... ... -.--", true, 40.3247f, -3.3431f));
        vorList.Add("BRA", new VOR("BRA", "Barajas", 116.45f, "-... .-. .-", true, 40.2809f, -3.3327f));
        vorList.Add("CNR", new VOR("CNR", "Colmenar Viejo", 117.30f, "-.-. -. .-.", true, 40.3846f, -3.4409f));
        vorList.Add("PDT", new VOR("PDT", "Perales", 116.75f, ".--. -.. -", true, Measurement.DMS2DD(40, 15, 10f), Measurement.DMS2DD(-3, 20, 52f)));
        vorList.Add("TLD", new VOR("TLD", "Toledo", 113.20f, "- .-.. -..", true, 39.58100f, -4.2015f));
        vorList.Add("RBO", new VOR("RBO", "Robledillo", 113.95f, ".-. -... ---", true, 40.51143f, -3.14474f));



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
