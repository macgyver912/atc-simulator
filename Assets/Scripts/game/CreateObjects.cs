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
    public static Dictionary<string, VOR> vorList;
    public static Dictionary<string, FIX> fixList;
    public static Dictionary<string, FIX> fixListWithoutRNAV;
    public static List<Navaid> navaidList;
    public static List<Navaid> temp_navaid_list;
    public static List<STAR> starList;
    public static List<SID> sidList;

    private static FIX aux_fix;
    private static VOR aux_vor;
    private static SID aux_sid;
    private static STAR aux_star;
    private static Navaid aux_navaid;
    private static string id_str;

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
        vorList = new Dictionary<string, VOR>();
        fixList = new Dictionary<string, FIX>();
        fixListWithoutRNAV = new Dictionary<string, FIX>();

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
        airport = new Airport("Adolfo Suárez Madrid-Barajas", "LEMD", "MAD", Measurement.DMS2DD(40, 28, 20f), Measurement.DMS2DD(-3, 33, 39f), 1998f, "Madrid", "Spain", 13000, 140, rwys);

        // ### FIX LIST - LEMD ###
        fixList.Add("ADUXO", new FIX("ADUXO", Measurement.DMS2DD(40, 30, 44.4f), Measurement.DMS2DD(-2, 03, 51.4f), FIX.FixTypes.OnRequest, true));
        fixList.Add("AVILA", new FIX("AVILA", Measurement.DMS2DD(40, 37, 28.6f), Measurement.DMS2DD(-4, 32, 59.6f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BANEV", new FIX("BANEV", Measurement.DMS2DD(41, 30, 09.4f), Measurement.DMS2DD(-2, 30, 52.3f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BARDI", new FIX("BARDI", Measurement.DMS2DD(40, 35, 00.6f), Measurement.DMS2DD(-6, 18, 08.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("BUREX", new FIX("BUREX", Measurement.DMS2DD(39, 48, 39.8f), Measurement.DMS2DD(-3, 56, 21.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("DAQSE", new FIX("DAQSE", Measurement.DMS2DD(40, 20, 35.1f), Measurement.DMS2DD(-4, 08, 48.1f), FIX.FixTypes.Compulsory, true));
        fixList.Add("DISKO", new FIX("DISKO", Measurement.DMS2DD(41, 00, 54.9f), Measurement.DMS2DD(-4, 13, 23.7f), FIX.FixTypes.OnRequest, true));
        fixList.Add("FAFEQ", new FIX("FAFEQ", Measurement.DMS2DD(40, 10, 09.8f), Measurement.DMS2DD(-3, 27, 38.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("LONGA", new FIX("LONGA", Measurement.DMS2DD(40, 26, 18.1f), Measurement.DMS2DD(-4, 52, 37.6f), FIX.FixTypes.OnRequest, true));

        fixList.Add("MD001", new FIX("MD001", Measurement.DMS2DD(40, 23, 30.0f), Measurement.DMS2DD(-2, 19, 20.0f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD012", new FIX("MD012", Measurement.DMS2DD(40, 39, 47.1f), Measurement.DMS2DD(-3, 42, 13.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD016", new FIX("MD016", Measurement.DMS2DD(40, 36, 00.5f), Measurement.DMS2DD(-3, 34, 30.8f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD017", new FIX("MD017", Measurement.DMS2DD(40, 37, 44.6f), Measurement.DMS2DD(-3, 33, 27.1f), FIX.FixTypes.Compulsory, false));
        fixList.Add("MD025", new FIX("MD025", Measurement.DMS2DD(40, 44, 16.5f), Measurement.DMS2DD(-3, 33, 27.4f), FIX.FixTypes.Compulsory, false));
        fixList.Add("MD039", new FIX("MD039", Measurement.DMS2DD(40, 38, 25.6f), Measurement.DMS2DD(-3, 40, 43.6f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD040", new FIX("MD040", Measurement.DMS2DD(40, 48, 02.5f), Measurement.DMS2DD(-3, 33, 27.5f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD041", new FIX("MD041", Measurement.DMS2DD(40, 36, 27.7f), Measurement.DMS2DD(-3, 47, 58.2f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD042", new FIX("MD042", Measurement.DMS2DD(40, 45, 11.6f), Measurement.DMS2DD(-3, 49, 49.8f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD043", new FIX("MD043", Measurement.DMS2DD(40, 35, 22.9f), Measurement.DMS2DD(-3, 46, 04.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD044", new FIX("MD044", Measurement.DMS2DD(40, 46, 49.4f), Measurement.DMS2DD(-3, 39, 31.0f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD047", new FIX("MD047", Measurement.DMS2DD(40, 35, 37.1f), Measurement.DMS2DD(-3, 32, 17.6f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD048", new FIX("MD048", Measurement.DMS2DD(40, 45, 13.2f), Measurement.DMS2DD(-3, 21, 33.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD049", new FIX("MD049", Measurement.DMS2DD(40, 42, 12.4f), Measurement.DMS2DD(-3, 16, 19.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD430", new FIX("MD430", Measurement.DMS2DD(40, 31, 30.6f), Measurement.DMS2DD(-4, 14, 24.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD435", new FIX("MD435", Measurement.DMS2DD(40, 52, 06.1f), Measurement.DMS2DD(-4, 13, 10.1f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD440", new FIX("MD440", Measurement.DMS2DD(39, 55, 18.5f), Measurement.DMS2DD(-3, 47, 32.0f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD445", new FIX("MD445", Measurement.DMS2DD(39, 50, 05.4f), Measurement.DMS2DD(-4, 07, 19.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD450", new FIX("MD450", Measurement.DMS2DD(39, 41, 13.7f), Measurement.DMS2DD(-4, 06, 10.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD455", new FIX("MD455", Measurement.DMS2DD(40, 11, 08.7f), Measurement.DMS2DD(-4, 53, 27.7f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD460", new FIX("MD460", Measurement.DMS2DD(39, 31, 07.9f), Measurement.DMS2DD(-4, 19, 26.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD465", new FIX("MD465", Measurement.DMS2DD(39, 25, 20.8f), Measurement.DMS2DD(-3, 53, 07.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD530", new FIX("MD530", Measurement.DMS2DD(40, 24, 58.0f), Measurement.DMS2DD(-3, 00, 35.8f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD535", new FIX("MD535", Measurement.DMS2DD(40, 47, 07.2f), Measurement.DMS2DD(-2, 38, 41.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD540", new FIX("MD540", Measurement.DMS2DD(40, 45, 20.7f), Measurement.DMS2DD(-2, 23, 37.3f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD545", new FIX("MD545", Measurement.DMS2DD(40, 49, 45.9f), Measurement.DMS2DD(-2, 35, 08.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD550", new FIX("MD550", Measurement.DMS2DD(40, 15, 44.1f), Measurement.DMS2DD(-2, 16, 56.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD824", new FIX("MD824", Measurement.DMS2DD(40, 24, 10.7f), Measurement.DMS2DD(-3, 06, 49.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD900", new FIX("MD900", Measurement.DMS2DD(40, 35, 21.6f), Measurement.DMS2DD(-3, 33, 34.9f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD901", new FIX("MD901", Measurement.DMS2DD(40, 38, 11.1f), Measurement.DMS2DD(-3, 32, 33.5f), FIX.FixTypes.Compulsory, false));
        fixList.Add("MD902", new FIX("MD902", Measurement.DMS2DD(40, 42, 12.6f), Measurement.DMS2DD(-3, 32, 12.0f), FIX.FixTypes.Compulsory, false));
        fixList.Add("MD910", new FIX("MD910", Measurement.DMS2DD(40, 41, 11.2f), Measurement.DMS2DD(-3, 38, 49.4f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD913", new FIX("MD913", Measurement.DMS2DD(40, 54, 08.6f), Measurement.DMS2DD(-3, 34, 34.5f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD920", new FIX("MD920", Measurement.DMS2DD(40, 46, 32.7f), Measurement.DMS2DD(-3, 23, 51.5f), FIX.FixTypes.OnRequest, false));
        fixList.Add("MD922", new FIX("MD922", Measurement.DMS2DD(40, 07, 58.5f), Measurement.DMS2DD(-2, 48, 25.3f), FIX.FixTypes.OnRequest, false));

        fixList.Add("MORAL", new FIX("MORAL", Measurement.DMS2DD(39, 00, 00.0f), Measurement.DMS2DD(-3, 32, 31.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("NANDO", new FIX("NANDO", Measurement.DMS2DD(39, 59, 19.9f), Measurement.DMS2DD(-2, 10, 28.4f), FIX.FixTypes.Compulsory, true));
        fixList.Add("NONTU", new FIX("NONTU", Measurement.DMS2DD(41, 30, 01.1f), Measurement.DMS2DD(-4, 10, 08.4f), FIX.FixTypes.OnRequest, true));
        fixList.Add("NOSKO", new FIX("NOSKO", Measurement.DMS2DD(40, 39, 22.8f), Measurement.DMS2DD(-2, 49, 00.2f), FIX.FixTypes.Compulsory, true));
        fixList.Add("ORBIS", new FIX("ORBIS", Measurement.DMS2DD(41, 15, 56.6f), Measurement.DMS2DD(-4, 11, 43.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("PINAR", new FIX("PINAR", Measurement.DMS2DD(40, 58, 49.1f), Measurement.DMS2DD(-2, 35, 57.0f), FIX.FixTypes.OnRequest, true));
        fixList.Add("PRADO", new FIX("PRADO", Measurement.DMS2DD(40, 08, 51.0f), Measurement.DMS2DD(-2, 00, 37.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("RIDAV", new FIX("RIDAV", Measurement.DMS2DD(40, 32, 06.9f), Measurement.DMS2DD(-5, 48, 29.8f), FIX.FixTypes.OnRequest, true));
        fixList.Add("RUDBI", new FIX("RUDBI", Measurement.DMS2DD(40, 15, 29.4f), Measurement.DMS2DD(-3, 08, 10.0f), FIX.FixTypes.Compulsory, true));
        fixList.Add("SIRGU", new FIX("SIRGU", Measurement.DMS2DD(40, 15, 37.8f), Measurement.DMS2DD(-2, 36, 00.5f), FIX.FixTypes.Compulsory, true));
        fixList.Add("SOTUK", new FIX("SOTUK", Measurement.DMS2DD(39, 11, 37.2f), Measurement.DMS2DD(-4, 44, 47.0f), FIX.FixTypes.OnRequest, true));
        fixList.Add("TERSA", new FIX("TERSA", Measurement.DMS2DD(40, 43, 30.1f), Measurement.DMS2DD(-2, 08, 16.2f), FIX.FixTypes.OnRequest, true));
        fixList.Add("URRIF", new FIX("URRIF", Measurement.DMS2DD(40, 14, 32.3f), Measurement.DMS2DD(-3, 44, 46.7f), FIX.FixTypes.OnRequest, true));
        fixList.Add("VILLA", new FIX("VILLA", Measurement.DMS2DD(40, 13, 58.6f), Measurement.DMS2DD(-2, 24, 37.6f), FIX.FixTypes.OnRequest, true));
        fixList.Add("YUNYE", new FIX("YUNYE", Measurement.DMS2DD(40, 02, 38.7f), Measurement.DMS2DD(-3, 37, 44.2f), FIX.FixTypes.OnRequest, true));


        // Copy to fixListWithoutRNAV only the FIX objects that no belongs to RNAV procedures
        foreach (FIX fix_item in fixList.Values)
        {
            // List of FIX buttons
            if (!fix_item.id.StartsWith("MD"))    // Filter RNAV SID-STAR points called MDxxx
            {
                fixListWithoutRNAV.Add(fix_item.GetId(), fix_item);
            }
        }//foreach FIX


        // ### VOR LIST - LEMD ###
        vorList.Add("BAN", new VOR("BAN", "Barahona", 112.80f, "-... .- -.", true, Measurement.DMS2DD(41, 19, 24.8f), Measurement.DMS2DD(-2, 37, 47.2F), true));
        vorList.Add("BRA", new VOR("BRA", "Barajas", 116.45f, "-... .-. .-", true, Measurement.DMS2DD(40, 28, 08.9f), Measurement.DMS2DD(-3, 33, 27.1f), false));
        vorList.Add("CCS", new VOR("CCS", "Cáceres", 114.20f, "-.-. -.-. ...", true, Measurement.DMS2DD(39, 31, 27.7f), Measurement.DMS2DD(-6, 26, 08.4f), true));
        vorList.Add("CNR", new VOR("CNR", "Colmenar", 117.30f, "-.-. -. .-.", true, Measurement.DMS2DD(40, 38, 45.5f), Measurement.DMS2DD(-3, 44, 09.0f), true));
        vorList.Add("PDT", new VOR("PDT", "Perales", 116.75f, ".--. -.. -", true, Measurement.DMS2DD(40, 15, 10f), Measurement.DMS2DD(-3, 20, 52f), true));
        vorList.Add("RBO", new VOR("RBO", "Robledillo", 113.95f, ".-. -... ---", true, Measurement.DMS2DD(40, 51, 13.9f), Measurement.DMS2DD(-3, 14, 47.9f), true));
        vorList.Add("SIE", new VOR("SIE", "Somosierra", 115.40f, "... .. .", true, Measurement.DMS2DD(41, 09, 06.0f), Measurement.DMS2DD(-3, 36, 17.4f), true));
        vorList.Add("SSY", new VOR("SSY", "San Sebastian de los Reyes", 117.85f, "... ... -.--", true, Measurement.DMS2DD(40, 32, 47.1f), Measurement.DMS2DD(-3, 34, 30.7f), false));
        vorList.Add("TLD", new VOR("TLD", "Toledo", 113.20f, "- .-.. -..", true, Measurement.DMS2DD(39, 58, 10.1f), Measurement.DMS2DD(-4, 20, 14.6f), true));
        vorList.Add("VTB", new VOR("VTB", "Villatobas", 112.70f, "...- - -...", true, Measurement.DMS2DD(39, 46, 50.6f), Measurement.DMS2DD(-3, 27, 51.1f), true));
        vorList.Add("ZMR", new VOR("ZMR", "Zamora", 117.10f, "--.. -- .-.", true, Measurement.DMS2DD(41, 31, 48.2f), Measurement.DMS2DD(-5, 38, 23.1f), true));

        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-7, 00, 00), 0));
        // 	vorList.Add(new VOR("INV", "Inventado", 116.75, null, false, Measurement.DMS2DD(40, 00, 00), Measurement.DMS2DD(-3, 33, 39)+3.439167, 0));

        // ### STAR LIST - LEMD ###
        starList = new List<STAR>();

        // LEMD - STAR RNAV (NORTH CONFIGURATION)

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

        // STAR - PRADO3D
        temp_navaid_list.Add(GetFIX("PRADO"));
        temp_navaid_list.Add(GetFIX("MD550"));
        temp_navaid_list.Add(GetFIX("SIRGU"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("PRADO3D", new List<Navaid>(temp_navaid_list)));
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

        // STAR - TERSA3Z
        temp_navaid_list.Add(GetFIX("TERSA"));
        temp_navaid_list.Add(GetFIX("MD540"));
        temp_navaid_list.Add(GetFIX("MD535"));
        temp_navaid_list.Add(GetFIX("NOSKO"));
        temp_navaid_list.Add(GetFIX("MD530"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("TERSA3Z", new List<Navaid>(temp_navaid_list)));
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

        // STAR - VILLA3D
        temp_navaid_list.Add(GetFIX("VILLA"));
        temp_navaid_list.Add(GetFIX("SIRGU"));
        temp_navaid_list.Add(GetFIX("RUDBI"));
        starList.Add(new STAR("VILLA3D", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // STAR - ZMR5C
        temp_navaid_list.Add(GetVOR("ZMR"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("DAQSE"));
        temp_navaid_list.Add(GetFIX("URRIF"));
        temp_navaid_list.Add(GetFIX("FAFEQ"));
        starList.Add(new STAR("ZMR5C", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();


        // ### SID LIST ###
        sidList = new List<SID>();

        // LEMD - SID RNAV (NORTH CONFIGURATION)
        // SID - BARDI3X
        temp_navaid_list.Add(GetFIX("MD016"));
        temp_navaid_list.Add(GetFIX("MD017"));
        temp_navaid_list.Add(GetFIX("MD040"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("LONGA"));
        temp_navaid_list.Add(GetFIX("BARDI"));
        sidList.Add(new SID("BARDI3X", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - BARDI6W
        temp_navaid_list.Add(GetFIX("MD900"));
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD910"));
        temp_navaid_list.Add(GetVOR("CNR"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("LONGA"));
        temp_navaid_list.Add(GetFIX("BARDI"));
        sidList.Add(new SID("BARDI6W", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - BARDI7L
        temp_navaid_list.Add(GetVOR("SSY"));
        temp_navaid_list.Add(GetFIX("MD039"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("LONGA"));
        temp_navaid_list.Add(GetFIX("BARDI"));
        sidList.Add(new SID("BARDI7L", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - CCS2X
        temp_navaid_list.Add(GetFIX("MD016"));
        temp_navaid_list.Add(GetFIX("MD017"));
        temp_navaid_list.Add(GetFIX("MD040"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("LONGA"));
        temp_navaid_list.Add(GetVOR("CCS"));
        sidList.Add(new SID("CCS2X", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - CCS5W
        temp_navaid_list.Add(GetFIX("MD900"));
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD910"));
        temp_navaid_list.Add(GetVOR("CNR"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("LONGA"));
        temp_navaid_list.Add(GetVOR("CCS"));
        sidList.Add(new SID("CCS5W", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - CCS6L
        temp_navaid_list.Add(GetVOR("SSY"));
        temp_navaid_list.Add(GetFIX("MD039"));
        temp_navaid_list.Add(GetFIX("AVILA"));
        temp_navaid_list.Add(GetFIX("LONGA"));
        temp_navaid_list.Add(GetVOR("CCS"));
        sidList.Add(new SID("CCS6L", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - NANDO3N
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD902"));
        temp_navaid_list.Add(GetFIX("MD920"));
        temp_navaid_list.Add(GetFIX("MD049"));
        temp_navaid_list.Add(GetFIX("MD824"));
        temp_navaid_list.Add(GetFIX("MD922"));
        temp_navaid_list.Add(GetFIX("NANDO"));
        sidList.Add(new SID("NANDO3N", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - NANDO3R
        temp_navaid_list.Add(GetFIX("MD047"));
        temp_navaid_list.Add(GetFIX("MD048"));
        temp_navaid_list.Add(GetFIX("MD049"));
        temp_navaid_list.Add(GetFIX("MD824"));
        temp_navaid_list.Add(GetFIX("MD922"));
        temp_navaid_list.Add(GetFIX("NANDO"));
        sidList.Add(new SID("NANDO3R", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - PINAR4N
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD902"));
        temp_navaid_list.Add(GetVOR("RBO"));
        temp_navaid_list.Add(GetFIX("PINAR"));
        sidList.Add(new SID("PINAR4N", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - PINAR4R
        temp_navaid_list.Add(GetFIX("MD047"));
        temp_navaid_list.Add(GetVOR("RBO"));
        temp_navaid_list.Add(GetFIX("PINAR"));
        sidList.Add(new SID("PINAR4R", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - RBO4N
        temp_navaid_list.Add(GetVOR("SSY"));
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD902"));
        temp_navaid_list.Add(GetVOR("RBO"));
        sidList.Add(new SID("RBO4N", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - RBO4R
        temp_navaid_list.Add(GetFIX("MD047"));
        temp_navaid_list.Add(GetVOR("RBO"));
        sidList.Add(new SID("RBO4R", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - SIE1X
        temp_navaid_list.Add(GetFIX("MD016"));
        temp_navaid_list.Add(GetFIX("MD017"));
        temp_navaid_list.Add(GetFIX("MD025"));
        temp_navaid_list.Add(GetFIX("MD913"));
        temp_navaid_list.Add(GetVOR("SIE"));
        sidList.Add(new SID("SIE1X", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - SIE3W
        temp_navaid_list.Add(GetFIX("MD900"));
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD025"));
        temp_navaid_list.Add(GetFIX("MD913"));
        temp_navaid_list.Add(GetVOR("SIE"));
        sidList.Add(new SID("SIE3W", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - SIE6L
        temp_navaid_list.Add(GetVOR("SSY"));
        temp_navaid_list.Add(GetFIX("MD039"));
        temp_navaid_list.Add(GetVOR("SIE"));
        sidList.Add(new SID("SIE6L", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - VTB2R
        temp_navaid_list.Add(GetFIX("MD047"));
        temp_navaid_list.Add(GetFIX("MD048"));
        temp_navaid_list.Add(GetFIX("MD049"));
        temp_navaid_list.Add(GetVOR("PDT"));
        temp_navaid_list.Add(GetVOR("VTB"));
        sidList.Add(new SID("VTB2R", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - VTB2X
        temp_navaid_list.Add(GetFIX("MD016"));
        temp_navaid_list.Add(GetFIX("MD017"));
        temp_navaid_list.Add(GetFIX("MD040"));
        temp_navaid_list.Add(GetFIX("MD042"));
        temp_navaid_list.Add(GetFIX("MD043"));
        temp_navaid_list.Add(GetVOR("BRA"));
        temp_navaid_list.Add(GetVOR("VTB"));
        sidList.Add(new SID("VTB2X", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - VTB6L
        temp_navaid_list.Add(GetVOR("SSY"));
        temp_navaid_list.Add(GetFIX("MD012"));
        temp_navaid_list.Add(GetFIX("MD041"));
        temp_navaid_list.Add(GetVOR("BRA"));
        temp_navaid_list.Add(GetVOR("VTB"));
        sidList.Add(new SID("VTB6L", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - ZMR3W
        temp_navaid_list.Add(GetFIX("MD900"));
        temp_navaid_list.Add(GetFIX("MD901"));
        temp_navaid_list.Add(GetFIX("MD025"));
        temp_navaid_list.Add(GetFIX("DISKO"));
        temp_navaid_list.Add(GetVOR("ZMR"));
        sidList.Add(new SID("ZMR3W", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - ZMR3X
        temp_navaid_list.Add(GetFIX("MD016"));
        temp_navaid_list.Add(GetFIX("MD017"));
        temp_navaid_list.Add(GetFIX("MD025"));
        temp_navaid_list.Add(GetFIX("DISKO"));
        temp_navaid_list.Add(GetVOR("ZMR"));
        sidList.Add(new SID("ZMR3X", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();

        // SID - ZMR7L
        temp_navaid_list.Add(GetVOR("SSY"));
        temp_navaid_list.Add(GetFIX("MD039"));
        temp_navaid_list.Add(GetFIX("MD044"));
        temp_navaid_list.Add(GetFIX("DISKO"));
        temp_navaid_list.Add(GetVOR("ZMR"));
        sidList.Add(new SID("ZMR7L", new List<Navaid>(temp_navaid_list)));
        temp_navaid_list.Clear();      

     
        // ### COMPANIES
        companyList = new List<Company>();

        companyList.Add(new Company("Iberia Líneas Aereas de España", "IBERIA", "IBE"));
        companyList.Add(new Company("Vueling", "VUELING", "VLG"));
        companyList.Add(new Company("Air Europa", "EUROPA", "AEA"));
        companyList.Add(new Company("Air Nostrum Líneas Aéreas del Mediterráneo, S.A.", "NOSTRUM AIR", "ANE"));
        companyList.Add(new Company("Transportes Aereos Portugueses, E.P.", "AIR PORTUGAL", "TAP"));

        companyList.Add(new Company("Ryanair", "RYANAIR", "RYR"));
        companyList.Add(new Company("EasyJet UK Ltd", "EASY", "EZY"));
        companyList.Add(new Company("American Airlines Inc.", "AMERICAN", "AAL"));
        companyList.Add(new Company("British Airways", "SPEEDBIRD", "BAW"));
        companyList.Add(new Company("Delta Airlines", "DELTA", "DAL"));
        companyList.Add(new Company("Deutsche Lufthansa, AG", "LUFTHANSA", "DLH"));
        companyList.Add(new Company("KLM Royal Dutch Airlines", "KLM", "KLM"));
        companyList.Add(new Company("Emirates", "EMIRATES", "UAE"));
        companyList.Add(new Company("Aerovías de México, S.A.", "AEROMEXICO", "AMX"));
        companyList.Add(new Company("Aerovías del Continente Americano, S.A.", "AVIANCA", "AVA"));



        // ### AIRCRAFTS ###
        aircraftList = new List<Aircraft>();

        // -- Arrivals -- 

        // Iberia from Europe country -> Entry from East
        aircraftList.Add(new Aircraft(
            "Airbus A320-214", "A320", Aircraft.Category.Medium, companyList[0], "5472", null, 4257,
            Measurement.DMS2DD(40, 30, 00f), Measurement.DMS2DD(-1, 58, 00f), 270,
            250, 18000, 0,
            220, 6000,
            null,
            GetSTAR("ADUXO2D") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // Iberia from Alicante -> Entry from South-East
        aircraftList.Add(new Aircraft(
            "Airbus A320-214", "A320", Aircraft.Category.Medium, companyList[0], "3275", null, 4167,
            Measurement.DMS2DD(38, 55, 00f), Measurement.DMS2DD(-3, 30, 00f), 330,
            300, 24000, 0,
            280, 16000,
            null,
            GetSTAR("MORAL5C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // Vueling from Barcelona -> Entry from North-East
        aircraftList.Add(new Aircraft(
            "Airbus A320-214", "A320", Aircraft.Category.Medium, companyList[1], "3201", null, 4378,
            Measurement.DMS2DD(40, 45, 00f), Measurement.DMS2DD(-2, 00, 00f), 250,
            280, 20000, 0,
            230, 8000,
            null,
            GetSTAR("TERSA3Z") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // Air Europa from South America -> Entry from West
        aircraftList.Add(new Aircraft(
            "Boeing 787-900", "B789", Aircraft.Category.Heavy, companyList[2], "450C", null, 4268,
            Measurement.DMS2DD(40, 45, 00f), Measurement.DMS2DD(-6, 00, 00f), 117,
            280, 19000, 0,
            250, 7000,
            null,
            GetSTAR("RIDAV4C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // Air Europa from Alicante -> Entry from South-East
        aircraftList.Add(new Aircraft(
            "Boeing 737-800", "B789", Aircraft.Category.Heavy, companyList[2], "3487", null, 4126,
            Measurement.DMS2DD(38, 58, 00f), Measurement.DMS2DD(-3, 00, 00f), 270,
            310, 26000, 0,
            260, 18000,
            null,
            GetSTAR("MORAL5C") as StdProcedure,
            Aircraft.FlightStatus.Incoming
        ));
        
        // Air Nostrum from Bilbao -> Entry from North
        aircraftList.Add(new Aircraft(
            "ATR-72", "AT72", Aircraft.Category.Light, companyList[3], "1100", null, 4158,
            Measurement.DMS2DD(41, 45, 00f), Measurement.DMS2DD(-4, 10, 00f), 185,
            230, 16000, 0,
            210, 8000,
            null,
            GetSTAR("NONTU4C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // TAP Portugal from Oporto -> Entry from West
        aircraftList.Add(new Aircraft(
            "Airbus A320-214", "A320", Aircraft.Category.Medium, companyList[4], "9761", null, 4259,
            Measurement.DMS2DD(39, 55, 00f), Measurement.DMS2DD(-4, 30, 00f), 070,
            220, 10000, 0,
            210, 5000,
            null,
            GetSTAR("TLD3C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // Ryanair from Italy -> Entry from West
        aircraftList.Add(new Aircraft(
            "Boeing 737-800", "B738", Aircraft.Category.Medium, companyList[5], "9761", null, 4765,
            Measurement.DMS2DD(40, 05, 00f), Measurement.DMS2DD(-1, 50, 00f), 299,
            230, 15000, 0,
            210, 7000,
            null,
            GetSTAR("PRADO3D") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // EasyJet from London -> Entry from North
        aircraftList.Add(new Aircraft(
            "Airbus A320-214", "A320", Aircraft.Category.Medium, companyList[6], "6237", null, 4765,
            Measurement.DMS2DD(41, 35, 00f), Measurement.DMS2DD(-2, 25, 00f), 206,
            260, 16000, 0,
            230, 8000,
            null,
            GetSTAR("BANEV4D") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // American Airlines from USA -> Entry from West
        aircraftList.Add(new Aircraft(
            "Boeing 777-300ER", "B77W", Aircraft.Category.Heavy, companyList[7], "94", null, 4687,
            Measurement.DMS2DD(41, 50, 00f), Measurement.DMS2DD(-5, 45, 00f), 138,
            280, 21000, 0,
            230, 9000,
            null,
            GetSTAR("ZMR5C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // British Airways from USA -> Entry from West
        aircraftList.Add(new Aircraft(
            "Boeing 777-300ER", "B77W", Aircraft.Category.Heavy, companyList[8], "734", null, 4687,
            Measurement.DMS2DD(42, 10, 00f), Measurement.DMS2DD(-6, 00, 00f), 105,
            300, 23000, 0,
            280, 17000,
            null,
            GetSTAR("ZMR5C") as StdProcedure,
            Aircraft.FlightStatus.Incoming
        ));

        // Delta Airlines from USA -> Entry from West
        aircraftList.Add(new Aircraft(
            "Airbus A330-300", "A333", Aircraft.Category.Heavy, companyList[9], "126", null, 4395,
            Measurement.DMS2DD(40, 50, 00f), Measurement.DMS2DD(-6, 30, 00f), 120,
            280, 19000, 0,
            230, 15000,
            null,
            GetSTAR("RIDAV4C") as StdProcedure,
            Aircraft.FlightStatus.Incoming
        ));

        // Lufthansa from Europe -> Entry from North-East
        aircraftList.Add(new Aircraft(
            "Airbus A321", "A321", Aircraft.Category.Medium, companyList[10], "3549", null, 4186,
            Measurement.DMS2DD(41, 30, 00f), Measurement.DMS2DD(-1, 30, 00f), 220,
            280, 24000, 0,
            240, 16000,
            null,
            GetSTAR("TERSA3Z") as StdProcedure,
            Aircraft.FlightStatus.Incoming
        ));

        // Emirates from Asia -> Entry from East
        aircraftList.Add(new Aircraft(
            "Airbus A380-800", "A388", Aircraft.Category.Heavy, companyList[12], "143", null, 4731,
            Measurement.DMS2DD(40, 00, 00f), Measurement.DMS2DD(-1, 30, 00f), 299,
            300, 17000, 0,
            240, 7000,
            null,
            GetSTAR("PRADO3D") as StdProcedure,
            Aircraft.FlightStatus.Incoming
        ));

        // Aeromexico from South America -> Entry from South-West
        aircraftList.Add(new Aircraft(
            "Boeing 787-900", "B789", Aircraft.Category.Heavy, companyList[13], "21", null, 4429,
            Measurement.DMS2DD(39, 30, 00f), Measurement.DMS2DD(-5, 30, 00f), 070,
            300, 20000, 0,
            240, 8000,
            null,
            GetSTAR("TLD3C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));

        // Avianca from South America -> Entry from South-West
        aircraftList.Add(new Aircraft(
            "Boeing 787-800", "B788", Aircraft.Category.Heavy, companyList[14], "182", null, 4765,
            Measurement.DMS2DD(39, 00, 00f), Measurement.DMS2DD(-5, 30, 00f), 070,
            280, 22000, 0,
            250, 14000,
            null,
            GetSTAR("SOTUK4C") as StdProcedure,
            Aircraft.FlightStatus.Arrival
        ));


        // -- Departures --
        // Avianca from South America -> Entry from South-West
        aircraftList.Add(new Aircraft(
            "Boeing 787-800", "B788", Aircraft.Category.Heavy, companyList[14], "4876", null, 4839,
            Measurement.DMS2DD(41, 00, 00f), Measurement.DMS2DD(-5, 30, 00f), 360,
            180, 3000, 0,
            220, 8000,
            null,
            GetSTAR("SOTUK4C") as StdProcedure,
            Aircraft.FlightStatus.Departure
        ));

    }

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

    private static SID GetSID(string id_str)
    {
        aux_sid = sidList.Find((x) => x.name == id_str);
        return aux_sid;
    }

    private static STAR GetSTAR(string id_str)
    {
        aux_star = starList.Find((x) => x.name == id_str);
        return aux_star;
    }

}
