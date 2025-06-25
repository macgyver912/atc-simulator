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
        fixList = new Dictionary<string, FIX>();
        vorList = new Dictionary<string, VOR>();

        temp_navaid_list = new List<Navaid>();

        Debug.Log("Creating objects...");

        // Runways
        Runway[] rwys = new Runway[8];
        
        rwys[0] = new Runway("14R", 143, 3988, Measurement.DMS2DD(40, 29, 05.50f), Measurement.DMS2DD(-3, 34, 33.64f), 2000f, false);
        rwys[1] = new Runway("32L", 323, 3988, Measurement.DMS2DD(40, 27, 47.10f), Measurement.DMS2DD(-3, 33, 14.02f), 2000f, true);

        rwys[2] = new Runway("14L", 143, 3500, Measurement.DMS2DD(40, 29, 41.71f), Measurement.DMS2DD(-3, 33, 28.33f), 2000f, false);
        rwys[3] = new Runway("32R", 323, 3500, Measurement.DMS2DD(40, 28, 24.85f), Measurement.DMS2DD(-3, 32, 10.30f), 2000f, true);

        rwys[4] = new Runway("36L", 360, 4179, Measurement.DMS2DD(40, 29, 33.32f), Measurement.DMS2DD(-3, 34, 28.64f), 2000f, false);
        rwys[5] = new Runway("18R", 180, 4179, Measurement.DMS2DD(40, 31, 22.40f), Measurement.DMS2DD(-3, 34, 29.27f), 2000f, true);

        rwys[6] = new Runway("36R", 360, 3500, Measurement.DMS2DD(40, 30, 03.97f), Measurement.DMS2DD(-3, 33, 33.15f), 2000f, false);
        rwys[7] = new Runway("18L", 180, 3500, Measurement.DMS2DD(40, 31, 41.22f), Measurement.DMS2DD(-3, 33, 33.68f), 2000f, true);

        // Airport
        airport = new Airport("Madrid-Barajas", "LEMD", "MAD", Measurement.DMS2DD(40, 28, 20f), Measurement.DMS2DD(-3, 33, 39f), 2000f, "Madrid", "Spain", 13000, 140, rwys);
     
        // ### FIX LIST ###
        // LEMD - FIX LIST - STAR 3 RNAV (NORTH CONFIGURATION)
        fixList.Add("AVILA", new FIX("AVILA", Measurement.DMS2DD(40, 37, 28.6f), Measurement.DMS2DD(-4, 32, 59.6f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BUREX", new FIX("BUREX", Measurement.DMS2DD(39, 48, 39.8f), Measurement.DMS2DD(-3, 56, 21.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("DAQSE", new FIX("DAQSE", Measurement.DMS2DD(40, 20, 35.1f), Measurement.DMS2DD(-4, 08, 48.1f), FIX.FixTypes.Compulsory, true));
        fixList.Add("FAFEQ", new FIX("FAFEQ", Measurement.DMS2DD(40, 10, 09.8f), Measurement.DMS2DD(-3, 27, 38.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("MD430", new FIX("MD430", Measurement.DMS2DD(40, 31, 30.6f), Measurement.DMS2DD(-4, 14, 24.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD435", new FIX("MD435", Measurement.DMS2DD(40, 52, 06.1f), Measurement.DMS2DD(-4, 13, 10.1f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD440", new FIX("MD440", Measurement.DMS2DD(39, 55, 18.5f), Measurement.DMS2DD(-3, 47, 32.0f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD445", new FIX("MD445", Measurement.DMS2DD(39, 50, 05.4f), Measurement.DMS2DD(-4, 07, 19.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD450", new FIX("MD450", Measurement.DMS2DD(39, 41, 13.7f), Measurement.DMS2DD(-4, 06, 10.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD455", new FIX("MD455", Measurement.DMS2DD(40, 11, 08.7f), Measurement.DMS2DD(-4, 53, 27.7f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD460", new FIX("MD460", Measurement.DMS2DD(39, 31, 07.9f), Measurement.DMS2DD(-4, 19, 26.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD465", new FIX("MD465", Measurement.DMS2DD(39, 25, 20.8f), Measurement.DMS2DD(-3, 53, 07.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MORAL", new FIX("MORAL", Measurement.DMS2DD(39, 00, 00.0f), Measurement.DMS2DD(-3, 32, 31.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("NONTU", new FIX("NONTU", Measurement.DMS2DD(41, 30, 01.1f), Measurement.DMS2DD(-4, 10, 08.4f), FIX.FixTypes.OnRequest, true));
        fixList.Add("ORBIS", new FIX("ORBIS", Measurement.DMS2DD(41, 15, 56.6f), Measurement.DMS2DD(-4, 11, 43.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("RIDAV", new FIX("RIDAV", Measurement.DMS2DD(40, 32, 06.9f), Measurement.DMS2DD(-5, 48, 29.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("SOTUK", new FIX("SOTUK", Measurement.DMS2DD(39, 11, 37.2f), Measurement.DMS2DD(-4, 44, 47.0f), FIX.FixTypes.OnRequest, true));
        fixList.Add("URRIF", new FIX("URRIF", Measurement.DMS2DD(40, 14, 32.3f), Measurement.DMS2DD(-3, 44, 46.7f), FIX.FixTypes.OnRequest, true));
        fixList.Add("YUNYE", new FIX("YUNYE", Measurement.DMS2DD(40, 02, 38.7f), Measurement.DMS2DD(-3, 37, 44.2f), FIX.FixTypes.OnRequest, true));
        // LEMD - FIX LIST - STAR 4 RNAV (NORTH CONFIGURATION)
        fixList.Add("ADUXO", new FIX("ADUXO", Measurement.DMS2DD(40, 30, 44.4f), Measurement.DMS2DD(-2, 03, 51.4f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BANEV", new FIX("BANEV", Measurement.DMS2DD(41, 30, 09.4f), Measurement.DMS2DD(-2, 30, 52.3f), FIX.FixTypes.OnRequest, true));
        fixList.Add("RUDBI", new FIX("RUDBI", Measurement.DMS2DD(40, 15, 29.4f), Measurement.DMS2DD(-3, 08, 10.0f), FIX.FixTypes.Compulsory, true));
        fixList.Add("MD001", new FIX("MD001", Measurement.DMS2DD(40, 23, 30.0f), Measurement.DMS2DD(-2, 19, 20.0f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD530", new FIX("MD530", Measurement.DMS2DD(40, 24, 58.0f), Measurement.DMS2DD(-3, 00, 35.8f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD535", new FIX("MD535", Measurement.DMS2DD(40, 47, 07.2f), Measurement.DMS2DD(-2, 38, 41.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD540", new FIX("MD540", Measurement.DMS2DD(40, 45, 20.7f), Measurement.DMS2DD(-2, 23, 37.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD545", new FIX("MD545", Measurement.DMS2DD(40, 49, 45.9f), Measurement.DMS2DD(-2, 35, 08.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD550", new FIX("MD550", Measurement.DMS2DD(40, 15, 44.1f), Measurement.DMS2DD(-2, 16, 56.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("NOSKO", new FIX("NOSKO", Measurement.DMS2DD(40, 39, 22.8f), Measurement.DMS2DD(-2, 49, 00.2f), FIX.FixTypes.Compulsory, true));
        fixList.Add("PINAR", new FIX("PINAR", Measurement.DMS2DD(40, 58, 49.1f), Measurement.DMS2DD(-2, 35, 57.0f), FIX.FixTypes.OnRequest, true));
        fixList.Add("PRADO", new FIX("PRADO", Measurement.DMS2DD(40, 08, 51.0f), Measurement.DMS2DD(-2, 00, 37.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("SIRGU", new FIX("SIRGU", Measurement.DMS2DD(40, 15, 37.8f), Measurement.DMS2DD(-2, 36, 00.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("TERSA", new FIX("TERSA", Measurement.DMS2DD(40, 43, 30.1f), Measurement.DMS2DD(-2, 08, 16.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("VILLA", new FIX("VILLA", Measurement.DMS2DD(40, 13, 58.6f), Measurement.DMS2DD(-2, 24, 37.6f), FIX.FixTypes.OnRequest, true));


        // ### VOR LIST ###
        // LEMD - VOR   
        vorList.Add("SSY", new VOR("SSY", "San Sebastian de los Reyes", 117.85f, "... ... -.--", true, Measurement.DMS2DD(40, 32, 47.1f), Measurement.DMS2DD(-3, 34, 30.7f), false));
        vorList.Add("BRA", new VOR("BRA", "Barajas", 116.45f, "-... .-. .-", true, Measurement.DMS2DD(40, 28, 08.9f), Measurement.DMS2DD(-3, 33, 27.1f), false));
        vorList.Add("CNR", new VOR("CNR", "Colmenar Viejo", 117.30f, "-.-. -. .-.", true, Measurement.DMS2DD(40, 38, 45.5f), Measurement.DMS2DD(-3, 44, 09.0f), true));
        vorList.Add("PDT", new VOR("PDT", "Perales", 116.75f, ".--. -.. -", true, Measurement.DMS2DD(40, 15, 10f), Measurement.DMS2DD(-3, 20, 52f), true));
        vorList.Add("TLD", new VOR("TLD", "Toledo", 113.20f, "- .-.. -..", true, Measurement.DMS2DD(39, 58, 10.1f), Measurement.DMS2DD(-4, 20, 14.6f), true));
        vorList.Add("RBO", new VOR("RBO", "Robledillo", 113.95f, ".-. -... ---", true, Measurement.DMS2DD(40, 51, 13.9f), Measurement.DMS2DD(-3, 14, 47.9f), true));
        vorList.Add("ZMR", new VOR("ZMR", "Zamora", 117.10f, "--.. -- .-.", true, Measurement.DMS2DD(41, 31, 48.2f), Measurement.DMS2DD(-5, 38, 23.1f), true));
        vorList.Add("BAN", new VOR("BAN", "Barahona", 112.80f, "-... .- -.", true, Measurement.DMS2DD(41, 19, 24.8f), Measurement.DMS2DD(-2, 37, 47.2F), true));

        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-7, 00, 00), 0));
        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-3, 33, 39)+3.439167, 0));

        // ### STAR LIST ###
        starList = new List<STAR>();
        // LEMD - STAR LIST - STAR 3 RNAV (NORTH CONFIGURATION)
        // STAR - MORAL5C
        temp_navaid_list.Add(GetFIX("MORAL"));
        temp_navaid_list.Add(GetFIX("MD465"));
        temp_navaid_list.Add(GetFIX("MD450"));
        temp_navaid_list.Add(GetFIX("BUREX"));
        temp_navaid_list.Add(GetFIX("MD440"));
        temp_navaid_list.Add(GetFIX("YUNYE"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("MORAL5C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - NONTU4C
        temp_navaid_list.Add(GetFIX("NONTU"));
        temp_navaid_list.Add(GetFIX("ORBIS"));
        temp_navaid_list.Add(GetFIX("MD435"));
        temp_navaid_list.Add(GetFIX("MD430"));
        temp_navaid_list.Add(GetFIX("DAQSE"));
        temp_navaid_list.Add(GetFIX("URRIF"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("NONTU4C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - RIDAV4C
        temp_navaid_list.Add(GetFIX("RIDAV"));
        temp_navaid_list.Add(GetFIX("MD455"));
        temp_navaid_list.Add(GetVOR("TLD"));
        temp_navaid_list.Add(GetFIX("MD445"));
        temp_navaid_list.Add(GetFIX("BUREX"));
        temp_navaid_list.Add(GetFIX("MD440"));
        temp_navaid_list.Add(GetFIX("YUNYE"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("RIDAV4C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - SOTUK4C
        temp_navaid_list.Add(GetFIX("SOTUK"));
        temp_navaid_list.Add(GetFIX("MD460"));
        temp_navaid_list.Add(GetFIX("MD450"));
        temp_navaid_list.Add(GetFIX("BUREX"));
        temp_navaid_list.Add(GetFIX("MD440"));
        temp_navaid_list.Add(GetFIX("YUNYE"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("SOTUK4C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - TLD3C
        temp_navaid_list.Add(GetVOR("TLD"));
        temp_navaid_list.Add(GetFIX("MD445"));
        temp_navaid_list.Add(GetFIX("BUREX"));
        temp_navaid_list.Add(GetFIX("MD440"));
        temp_navaid_list.Add(GetFIX("YUNYE"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("TLD3C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - ZMR5C
        temp_navaid_list.Add(GetVOR("ZMR"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("DAQSE"));
        temp_navaid_list.Add(GetFIX("URRIF"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("ZMR5C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // LEMD - FIX LIST - STAR 4 RNAV (NORTH CONFIGURATION)
        // STAR - ADUXO2D
        temp_navaid_list.Add(GetFIX("ADUXO"));
        temp_navaid_list.Add(GetFIX("MD001"));
        temp_navaid_list.Add(GetFIX("SIRGU"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("ADUXO2D", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - BANEV4D
        temp_navaid_list.Add(GetFIX("BANEV"));
        temp_navaid_list.Add(GetVOR("BAN"));
        temp_navaid_list.Add(GetFIX("PINAR"));
        temp_navaid_list.Add(GetFIX("MD545"));
        temp_navaid_list.Add(GetFIX("NOSKO"));
        temp_navaid_list.Add(GetFIX("MD530"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("BANEV4D", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - PRADO3D
        temp_navaid_list.Add(GetFIX("PRADO"));
        temp_navaid_list.Add(GetFIX("MD550"));
        temp_navaid_list.Add(GetFIX("SIRGU"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("PRADO3D", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - TERSA3Z
        temp_navaid_list.Add(GetFIX("TERSA"));
        temp_navaid_list.Add(GetFIX("MD540"));
        temp_navaid_list.Add(GetFIX("MD535"));
        temp_navaid_list.Add(GetFIX("NOSKO"));
        temp_navaid_list.Add(GetFIX("MD530"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("TERSA3Z", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - VILLA3D
        temp_navaid_list.Add(GetFIX("VILLA"));
        temp_navaid_list.Add(GetFIX("SIRGU"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("VILLA3D", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();


        // ### SID LIST ###
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
            GetVOR("TLD") as Navaid,
            Aircraft.FlightStatus.Departure
        ));
        /*
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
