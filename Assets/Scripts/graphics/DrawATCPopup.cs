
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UIElements;
using System.Linq;

public class DrawATCPopup : MonoBehaviour
{

    public static AircraftCtrl acftCtrl;

    //public static String identifier = String.Empty;

    private static int ctrlId;
    private static int noCtrlId;
    private static int transCtrlId;
    private static int headingPopupId;
    private static int altitudePopupId;
    private static int speedPopupId;

    public static bool showCtrlGUI = false;
    public static bool showNoCtrlGUI = false;
    public static bool showTransCtrlGUI = false;

    static bool showHeadingPopup = false;
    static bool showAltitudePopup = false;
    static bool showSpeedPopup = false;
    static bool showFlyToPopup = false;
    static bool showProceduresPopup = false;

    public static Vector2 ctrlPopupSize;
    public static Rect ctrlPopupRect;
    public static Vector2 ctrlButtonSize;
    public static Vector2 noCtrlPopupSize;
    public static Rect noCtrlPopupRect;
    public static Vector2 noCtrlButtonSize;
    public static Vector2 transCtrlPopupSize;
    public static Rect transCtrlPopupRect;
    public static Vector2 transCtrlButtonSize;

    static Vector2 submenuSize;
    static Vector2 submenuAsideTextSize;
    static Vector2 submenuInputBoxSize;
    static Vector2 submenuSelButtonSize;
    static Vector2 submenuAcceptButtonSize;
    static Rect submenuRect;

    static Vector2 numberShortcutSize;

    static bool submenuIsAltitude;
    static bool submenuIsAltSpeedUp;

    static bool submenuIsSpeedSpeedUp;

    static bool initGUI;

    static string acceptText;
    static List<string> noCtrlTexts;
    static List<string> ctrlTexts;
    static List<string> transCtrlTexts;

    public GUISkin popup_guistyle;

    public Texture buttonUpIcon;
    public Texture buttonDownIcon;
    public Texture buttonLeftIcon;
    public Texture buttonRightIcon;
    public Texture turnLeftIcon;
    public Texture turnIcon;
    public Texture turnRightIcon;

    private GUIStyle popupStyle;
    private GUIStyle buttonStyle;
    private GUIStyle numberShortcutStyle;

    private GUIStyle submenuAsideTextStyle;
    private GUIStyle submenuInputBoxStyle;
    private GUIStyle submenuSelButtonStyle;
    private GUIStyle submenuAcceptButtonStyle;

    int submenuHeadingToolbarInt = 1;
    Texture[] submenuHeadingToolbarTextures;

    bool setupSubmenu;

    static string inputsName = "InputBox";
    static ushort submenuMaxNumberOfDigits = 5;
    static string[] submenuDigits = new string[submenuMaxNumberOfDigits];

    ushort submenuElemSep = 1;	// separation between elements in pixels
    //ushort popupOffset;
    ushort popupOffset = 1;       // offset in pixels


    ushort tgtHdg;
    int tgtAlt;
    ushort tgtSpd;

    private const ushort HDG_MIN = 001;   // in degrees - really it should be 0 but in ATC phraseology is 360
    private const ushort HDG_MAX = 360;   // in degrees - really it should be 359 but in ATC phraseology is 360

    private const int ALT_MIN = -2000;   // in feet - lowest airport is about -1240 ft in Israel
    private const int ALT_MAX = 55000;   // in feet

    private const ushort SPD_MIN = 120;     // in kts
    private const ushort SPD_MAX = 500;     // in kts	

    private ushort nDigits;
    ushort[] aux;

    public void Awake()
    {
        submenuHeadingToolbarTextures = new Texture[] { turnLeftIcon, turnIcon, turnRightIcon };
        nDigits = (ushort) HDG_MAX.ToString().Length;
        aux = new ushort[nDigits];
        //Debug.Log("nDigits = " + nDigits);
    }

    public static void Init()
    {
        for (ushort i = 0; i < submenuDigits.Length; i++)
        {
            submenuDigits[i] = "0";
        }

        ctrlId = General.AssignWindowId();
        noCtrlId = General.AssignWindowId();
        transCtrlId = General.AssignWindowId();

        headingPopupId = General.AssignWindowId();
        altitudePopupId = General.AssignWindowId();
        speedPopupId = General.AssignWindowId();

        noCtrlTexts = new List<string>();
        ctrlTexts = new List<string>();
        transCtrlTexts = new List<string>();

        SetTexts();

        initGUI = true;
    }


    private static void SetTexts()
    {
        acceptText = "Accept";

        noCtrlTexts.Add("Accept");

        transCtrlTexts.Add("Req. come back with you");

        ctrlTexts.Add("Heading");
        ctrlTexts.Add("Altitude");
        ctrlTexts.Add("Speed");
        ctrlTexts.Add("Fly to");
        ctrlTexts.Add("SID/STAR");
        ctrlTexts.Add("Handoff");
    }

    // #################
    // ##### MENUS #####
    // #################

    void MngCtrlPressedButton(int index)
    {
#if DEBUG_MODE
	    Debug.Log("MngCtrlPressedButton - Option selected: " + index);
#endif

        switch (index)
        {
            case 0:
                ShowHeadingPopup(Input.mousePosition);
                break;
            case 1:
                ShowAltitudePopup(Input.mousePosition);
                break;
            case 2:
                ShowSpeedPopup(Input.mousePosition);
                break;
            case 3:
                ShowFlyToPopup(Input.mousePosition);
                break;
            case 4:
                ShowProceduresPopup(Input.mousePosition);
                break;
            case 5:
                Handoff();
                break;
            default:
                showCtrlGUI = false;
                break;
        }
        showCtrlGUI = false;
        //		Debug.Log("Pressed " + ctrlTexts[i]);

    }

    void Handoff()
    {
        Debug.Log("Requested to handoff to " + acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber());
        // Change status condition of the aircraft from tranferred to be under your control
        // if is Outgoing traffic
        if (acftCtrl.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Departure)
            acftCtrl.GetAircraft().SetFlightStatus(Aircraft.FlightStatus.Outgoing);
        // if is Incoming traffic						
        else
            acftCtrl.GetAircraft().SetFlightStatus(Aircraft.FlightStatus.Incoming);
    }

    // Make the contents of the window
    void DoCtrlPopup(int windowID)
    {
        ushort j = 0;   // reset the internal counter

        for (ushort i = 0; i < ctrlTexts.Count; i++)
        {

            if (GUI.Button(new Rect(popupOffset, popupOffset + ctrlButtonSize.y * i,
                ctrlButtonSize.x, ctrlButtonSize.y), ctrlTexts[i], buttonStyle))
            {

                MngCtrlPressedButton(i);
            }

            GUI.Label(new Rect(ctrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + ctrlButtonSize.y * j,
                    numberShortcutSize.x, ctrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

            j++;    // increments the internal counter

            // Refresh window size
            ctrlPopupRect.height = ctrlButtonSize.y * j + 2 * popupOffset;

        }//for

    }//DoCtrlPopup

    // Make the contents of the window
    /*
    void DoCtrlPopup(int windowID)
    {
        //	ushort e = Event.current;

        ushort i = 0;
        ushort j = 0;

        if (Input.GetButton("Instructions1"))
        {
            j = 0;  // reset the internal counter

            // if "Instructions1" button pressed, show first group of ATC instructions
            for (i = 0; i < 3; i++)
            {

                if (GUI.Button(new Rect(popupOffset, popupOffset + ctrlButtonSize.y * j,
                    ctrlButtonSize.x, ctrlButtonSize.y), ctrlTexts[i], buttonStyle))
                {

                    MngCtrlPressedButton(i);
                }


                GUI.Label(new Rect(ctrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + ctrlButtonSize.y * j,
                        numberShortcutSize.x, ctrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

                j++;    // increments the internal counter

                // Refresh window size
                ctrlPopupRect.height = ctrlButtonSize.y * j + 2 * popupOffset;

            }//for

        }
        else if (Input.GetButton("Instructions2"))
        {
            j = 0;  // reset the internal counter

            // else if "Instructions2" button pressed, show second group of ATC instructions
            for (i = 3; i < 5; i++)
            {

                if (GUI.Button(new Rect(popupOffset, popupOffset + ctrlButtonSize.y * j,
                    ctrlButtonSize.x, ctrlButtonSize.y), ctrlTexts[i], buttonStyle))
                {

                    MngCtrlPressedButton(i);
                }

                GUI.Label(new Rect(ctrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + ctrlButtonSize.y * j,
                        numberShortcutSize.x, ctrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

                j++;    // increments the internal counter

                // Refresh window size
                ctrlPopupRect.height = ctrlButtonSize.y * j + 2 * popupOffset;

            }//for

        }
        else if (Input.GetButton("Instructions3"))
        {
            j = 0;  // reset the internal counter

            // else if "Instructions3" button pressed, show third group of ATC instructions
            for (i = 5; i < ctrlTexts.Count; i++)
            {

                if (GUI.Button(new Rect(popupOffset, popupOffset + ctrlButtonSize.y * j,
                    ctrlButtonSize.x, ctrlButtonSize.y), ctrlTexts[i], buttonStyle))
                {

                    MngCtrlPressedButton(i);
                }

                GUI.Label(new Rect(ctrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + ctrlButtonSize.y * j,
                        numberShortcutSize.x, ctrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

                j++;    // increments the internal counter

                // Refresh window size
                ctrlPopupRect.height = ctrlButtonSize.y * j + 2 * popupOffset;

            }//for

        }
        else
        {
            j = 0;  // reset the internal counter

            // else if no "Instructions" button pressed, show all options
            for (i = 0; i < ctrlTexts.Count; i++)
            {

                if (GUI.Button(new Rect(popupOffset, popupOffset + ctrlButtonSize.y * i,
                    ctrlButtonSize.x, ctrlButtonSize.y), ctrlTexts[i], buttonStyle))
                {

                    MngCtrlPressedButton(i);
                }

                GUI.Label(new Rect(ctrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + ctrlButtonSize.y * j,
                        numberShortcutSize.x, ctrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

                j++;    // increments the internal counter

                // Refresh window size
                ctrlPopupRect.height = ctrlButtonSize.y * j + 2 * popupOffset;

            }//for

        }//if-else
    }
    */

    // Make the contents of the window for aircraft with 'incoming' status
    void DoNoCtrlPopup(int windowID)
    {
        for (var i = 0; i < noCtrlTexts.Count; i++)
        {

            if (GUI.Button(new Rect(popupOffset, popupOffset + ctrlButtonSize.y * i,
                ctrlButtonSize.x, ctrlButtonSize.y), noCtrlTexts[i], buttonStyle))
            {

                switch (i)
                {
                    case 0:
                        Debug.Log("Traffic " + acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber() + " is accepted under your control");

                        // Change status condition of the aircraft incoming or outgoing to be under your control
                        // if is Outgoing traffic
                        if (acftCtrl.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Outgoing)
                            acftCtrl.GetAircraft().SetFlightStatus(Aircraft.FlightStatus.Departure);
                        // if is Incoming traffic						
                        else
                            acftCtrl.GetAircraft().SetFlightStatus(Aircraft.FlightStatus.Arrival);
                        break;
                    default:
                        showNoCtrlGUI = false;
                        break;
                }
                showNoCtrlGUI = false;
                //			Debug.Log("Pressed " + noCtrlTexts[i]);

            }

            GUI.Label(new Rect(noCtrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + noCtrlButtonSize.y * i,
                    numberShortcutSize.x, noCtrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

        }//for

    }

    // Make the contents of the window for aircraft with 'tranferred' status
    void DoTransCtrlPopup(int windowID)
    {
        for (var i = 0; i < transCtrlTexts.Count; i++)
        {

            if (GUI.Button(new Rect(popupOffset, popupOffset + transCtrlButtonSize.y * i,
                transCtrlButtonSize.x, transCtrlButtonSize.y), transCtrlTexts[i], buttonStyle))
            {

                switch (i)
                {
                    case 0:
                        Debug.Log("Requested to come back with you to " + acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber());

                        // Change status condition of the aircraft from tranferred to be under your control
                        // if is Outgoing traffic
                        if (acftCtrl.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Outgoing)
                            acftCtrl.GetAircraft().SetFlightStatus(Aircraft.FlightStatus.Departure);
                        // if is Incoming traffic						
                        else
                            acftCtrl.GetAircraft().SetFlightStatus(Aircraft.FlightStatus.Arrival);
                        break;
                    default:
                        showTransCtrlGUI = false;
                        break;
                }
                showTransCtrlGUI = false;
                //			Debug.Log("Pressed " + noCtrlTexts[i]);

            }

            GUI.Label(new Rect(transCtrlPopupSize.x - popupOffset - numberShortcutSize.x, popupOffset + transCtrlButtonSize.y * i,
                    numberShortcutSize.x, transCtrlButtonSize.y), (i + 1).ToString(), numberShortcutStyle);

        }//for

    }


    // ####################
    // ##### SUBMENUS #####
    // ####################

    void PrepareSubmenuPopup(Vector3 mousePos)
    {
        // do popup rect where mouse is
        DrawATCPopup.submenuRect.x =
            (DrawATCPopup.submenuRect.width + mousePos.x > Screen.width ?
                mousePos.x - DrawATCPopup.submenuRect.width : mousePos.x);

        DrawATCPopup.submenuRect.y =
            (DrawATCPopup.submenuRect.height - mousePos.y + Screen.height > Screen.height ?
                -mousePos.y + Screen.height - DrawATCPopup.submenuRect.height : -mousePos.y + Screen.height);
    }

    void ShowHeadingPopup(Vector3 mousePos)
    {
        PrepareSubmenuPopup(mousePos);

        DrawATCPopup.showHeadingPopup = true;

        // Prepares to show the window
        setupSubmenu = true;
    }

    void ShowAltitudePopup(Vector3 mousePos)
    {
        PrepareSubmenuPopup(mousePos);

        DrawATCPopup.showAltitudePopup = true;

        // Prepares to show the window
        setupSubmenu = true;
    }

    void ShowSpeedPopup(Vector3 mousePos)
    {
        PrepareSubmenuPopup(mousePos);

        DrawATCPopup.showSpeedPopup = true;

        // Prepares to show the window
        setupSubmenu = true;
    }

    void ShowFlyToPopup(Vector3 mousePos)
    {
        PrepareSubmenuPopup(mousePos);

        DrawATCPopup.showFlyToPopup = true;

        // Prepares to show the window
        setupSubmenu = true;
    }

    void ShowProceduresPopup(Vector3 mousePos)
    {
        PrepareSubmenuPopup(mousePos);

        DrawATCPopup.showProceduresPopup = true;

        // Prepares to show the window
        setupSubmenu = true;
    }


    void ChangeNumber_HDG(short variation, ushort digit)
    {
        //Debug.Log("ChangeNumber_HDG(" + variation + ", " + digit + ")");

        tgtHdg = 0;

        ushort k = 0;        
        // Convert inputs to number
        for (k = 0; k<nDigits; k++)
        {
            if (k == digit)
            {
                //Debug.Log("k == digit: " + k);

                ushort.TryParse(submenuDigits[k], out aux[k]);
                aux[k] += (ushort) variation;
                //	 			if(aux[k] > maxHeading || aux[k] < minHeading){
                //	 				ushort.TryParse(maxHeading.ToString()[k].ToString(), aux[k]);
                //	 			}		
            }
            else
            {
                //Debug.Log("k != digit: " + k);

                ushort.TryParse(submenuDigits[k], out aux[k]);
            }

            aux[k] = (ushort) (aux[k] * Mathf.Pow(10, nDigits - k - 1));

            //Debug.Log("aux[k]: " + aux[k]);

            tgtHdg += aux[k];
            //Debug.Log("tgtHdg: " + tgtHdg);
        }

        tgtHdg = CheckRange_HDG(tgtHdg);

        // Convert number to inputs
        string str = string.Format("{0:D3}", tgtHdg);
        for (k = 0; k < nDigits; k++)
        {
            submenuDigits[k] = str[k].ToString();
        }
    } // ChangeNumber_HDG

    ushort CheckRange_HDG(ushort hdg_in)
    {
        ushort hdg_out;

        hdg_out = (ushort)(hdg_in % 360);
        hdg_out = (hdg_out == 0 ? (ushort)360 : hdg_out);

        //Debug.Log("CheckRange_HDG: " + hdg_out);
        return hdg_out;
    }

    // Sets commands to aircraft when heading is set and 'accept' button is pressed
    void AcceptPressed_HDG()
    {
        //Debug.Log("HDG: " + tgtHdg);
        if (tgtHdg != acftCtrl.GetAircraft().GetHeading())
        {

            showHeadingPopup = false;

            // update radar screen tag of this aircraft
            acftCtrl.GetAircraft().SetAuthoHdg(tgtHdg);
            DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.GetAircraft());

            // simulate the communication text between ATC and pilots
            string hdgStr = string.Format("{0:D3}", tgtHdg);
            string debugText = acftCtrl.GetAircraft().GetCallsign() + " " + TextUtils.Text2SpellFormat(acftCtrl.GetAircraft().GetFlightNumber()) + ", turn ";
            debugText += (submenuHeadingToolbarInt == 0) ? "left " : (submenuHeadingToolbarInt == 2) ? "right " : "";
            debugText += "to heading " + TextUtils.Text2SpellFormat(hdgStr);
            //Debug.LogWarning(debugText);
            MngDialogs.SetText(debugText, 0);

            // set commands to the aircraft
            acftCtrl.Turn(tgtHdg, submenuHeadingToolbarInt);
        }
        else
        {
            // heading and requested heading are equals
        }

    } // AcceptPressed_HDG





    // Make the contents of the window
    void DoHeadingPopup(int windowID)
    {

        /* Heading submenu
	     * _________________
	     * |     |<||<||<| |     XXX = {"Heading"}
	     * | XXX  N  N  N  |	   N = {0, 9}
	     * |_____|>||>||>|_|
	     * |_____ACCEPT____|
	     */

        ushort nDigits = (ushort) HDG_MAX.ToString().Length;

        // Show "Heading" text left to inputs
        string asideText = "Heading [º]";                  				
        GUIStyle textStyle = new GUIStyle(submenuAsideTextStyle);
        textStyle.alignment = TextAnchor.MiddleCenter;

        // Show aircraft callsign in upper left corner
        GUIStyle acftLabelStyle = new GUIStyle(submenuAsideTextStyle);
        acftLabelStyle.alignment = TextAnchor.UpperLeft;
        acftLabelStyle.fontSize = 9;
        GUI.Label(new Rect(3 * popupOffset, popupOffset, 3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber(), acftLabelStyle);

        // Set turning side buttons
        GUI.Label(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
                    3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                    asideText, textStyle);

        GUIStyle toolbarStyle = new GUIStyle("Button");
        toolbarStyle.margin = new RectOffset(0, 0, 0, 0);
        toolbarStyle.padding = new RectOffset(0, 0, 3, 3);

        submenuHeadingToolbarInt = GUI.Toolbar(new Rect(popupOffset, submenuSelButtonSize.y + submenuAsideTextSize.y,
                3 * submenuAsideTextSize.x, submenuAsideTextSize.y - 2 * popupOffset), submenuHeadingToolbarInt,
                submenuHeadingToolbarTextures, toolbarStyle);


        // Set input digits
        GUIStyle buttonWithoutPadding = new GUIStyle("Button");
        buttonWithoutPadding.padding = new RectOffset(4, 4, 4, 4);

        for (ushort i = 0; i < nDigits; i++)
        {

            // ##### Up buttons #####
            if (GUI.Button(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpIcon, buttonWithoutPadding))
            {
                //Debug.Log("UP_" + i);
                ChangeNumber_HDG(1, i);

            }//if-up button


            // ##### Input boxes #####
            // Name the input boxes to can be accessed by focus control
            GUI.SetNextControlName(inputsName + "_" + i);

            submenuDigits[i] = GUI.TextField(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y,
                        submenuInputBoxSize.x, submenuInputBoxSize.y),
                        submenuDigits[i], 1, submenuInputBoxStyle);
            
            // Control only digits						
            submenuDigits[i] = (submenuDigits[i] != "" && char.IsDigit(submenuDigits[i][0]) ? submenuDigits[i] : "0");

            // ##### Down buttons #####
            if (GUI.Button(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonDownIcon, buttonWithoutPadding))
            {
                //Debug.Log("DN_" + i);
                ChangeNumber_HDG(-1, i);

            }//if-down buttons

        }//for	

        // Set "Accept" button
        if (GUI.Button(new Rect(popupOffset, popupOffset + 2 * submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSize.x - popupOffset, submenuAcceptButtonSize.y),
                        acceptText, submenuAcceptButtonStyle) /*|| Input.GetButton("Accept")*/)
        {

            AcceptPressed_HDG();

        }// if-button

        // Keyboard input control
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.Tab && e.keyCode != KeyCode.Return && e.keyCode != KeyCode.KeypadEnter)
        {
            //changeNumber(0, -1, nDigits, aux);
            string currentInput = GUI.GetNameOfFocusedControl().Split("_"[0])[1];
            ushort n = 0;
            ushort.TryParse(currentInput, out n);
            n = (ushort) (n >= nDigits - 1 ? 0 : ++n);
            string nextInput = inputsName + "_" + n.ToString();
            //		Debug.Log("nextInput: " + inputsName + "_" + n);
            GUI.FocusControl(inputsName + "_" + n);
        }
        else if ((e.isKey && e.keyCode == KeyCode.Return) || (e.isKey && e.keyCode == KeyCode.KeypadEnter))
        {
            ushort n = acftCtrl.GetAircraft().GetHeading();
            ushort.TryParse(submenuDigits[0].ToString() + submenuDigits[1].ToString() + submenuDigits[2].ToString(), out n);
            //Debug.Log("n: " + submenuDigits[0].ToString() + submenuDigits[1].ToString() + submenuDigits[2].ToString());
            tgtHdg = CheckRange_HDG(n);
            AcceptPressed_HDG();
        }

        if (setupSubmenu)
        {
            GUI.FocusControl(inputsName + "_0");
            string currentHdg = string.Format("{0:D3}", acftCtrl.GetAircraft().GetHeading());
            for (ushort j = 0; j < currentHdg.Length; j++)
            {
                submenuDigits[j] = currentHdg[j].ToString();
            }
            submenuHeadingToolbarInt = 1;

            setupSubmenu = false;
        }

    }
    























    void ChangeNumber_ALT(short variation, ushort digit)
    {
        //Debug.Log("ChangeNumber_HDG(" + variation + ", " + digit + ")");

        ushort k = 0;
     
        tgtAlt = 0;
        // Convert inputs to number
        for (k = 0; k < nDigits; k++)
        {
            if (k == digit)
            {
                //Debug.Log("k == digit: " + k);

                ushort.TryParse(submenuDigits[k], out aux[k]);
                aux[k] += (ushort)variation;
            }
            else
            {
                //Debug.Log("k != digit: " + k);

                ushort.TryParse(submenuDigits[k], out aux[k]);
            }

            aux[k] = (ushort)(aux[k] * Mathf.Pow(10, nDigits - k - 1));

            //Debug.Log("aux[k]: " + aux[k]);

            tgtAlt += aux[k] * 100; // FL to ALT
            //Debug.Log("tgtAlt: " + tgtAlt);
        }

        tgtAlt = CheckRange_ALT(tgtAlt);

        // Convert number to inputs
        string str = string.Format("{0:D5}", tgtAlt);
        for (k = 0; k < nDigits; k++)
        {
            submenuDigits[k] = str[k].ToString();
        }
    } // ChangeNumber_ALT

    int CheckRange_ALT(int alt_in)
    {
        int alt_out;

        if (alt_in < ALT_MIN)
            alt_out = ALT_MIN;
        else if (alt_in > ALT_MAX)
            alt_out = ALT_MAX;
        else
            alt_out = alt_in;

        //Debug.Log("CheckRange_ALT: " + alt_out);
        return alt_out;
    }

    
    void AcceptPressed_ALT()
    {
        //Debug.Log("ALT: " + tgtAlt);
        if (tgtAlt != acftCtrl.GetAircraft().GetAltitude())
        {

            showAltitudePopup = false;

            // update radar screen tag of this aircraft
            acftCtrl.GetAircraft().SetAuthoAltitude(tgtAlt);
            DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.GetAircraft());

            // simulate the communication text between ATC and pilots
            string debugText = acftCtrl.GetAircraft().GetCallsign() + " " + TextUtils.Text2SpellFormat(acftCtrl.GetAircraft().GetFlightNumber())
                + ", " + (submenuIsAltSpeedUp ? "expedite " : "");

            if (acftCtrl.GetAircraft().GetAltitude() < tgtAlt)
            {
                // if aircraft is flying below authorized altitude, climb
                debugText += "climb to ";
            }
            else
            {
                // if aircraft is flying above authorized altitude, descend
                debugText += "descend to ";
            }
            debugText += (tgtAlt < CreateObjects.airport.GetTransAltitude() ? tgtAlt.ToString() + " feet" : "flight level " + TextUtils.Text2SpellFormat((tgtAlt/100).ToString()));
            //Debug.LogWarning(debugText);
            MngDialogs.SetText(debugText, 0);

            // set commands to the aircraft
            acftCtrl.ChangeLevel(tgtAlt, submenuIsAltSpeedUp);
        }
        else
        {
            // altitude and requested altitude are equals
        }

    }
    

    // Make the contents of the window

    void DoAltitudePopup(int windowID) {

        /* Heading submenu
	     * _________________
	     * |     |<||<||<| |     XXX = {ALT, FL}
	     * | XXX  N  N  N  |	   N = {0, 9}
	     * |_____|>||>||>|_|
	     * |_____ACCEPT____|
	     */

        ushort nDigits = (ushort) (ALT_MAX / 100).ToString().Length;

        // Show "Speed" text left to inputs
        string asideText = "ALT [ft]";                 // text to show at left of inputs
        GUIStyle textStyle = new GUIStyle(submenuAsideTextStyle);
        textStyle.alignment = TextAnchor.MiddleCenter;

        // Show aircraft callsign in upper left corner
        GUIStyle acftLabelStyle = new GUIStyle(submenuAsideTextStyle);
        acftLabelStyle.alignment = TextAnchor.UpperLeft;
        acftLabelStyle.fontSize = 9;
        GUI.Label(new Rect(3 * popupOffset, popupOffset, 3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber(), acftLabelStyle);

        // Set input digits
        GUI.Label(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
                    2 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                    asideText, textStyle);

        // Set "speed up" button
        submenuIsAltSpeedUp = GUI.Toggle(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y + submenuAsideTextSize.y,
                2 * submenuAsideTextSize.x, submenuAsideTextSize.y - 2 * popupOffset), submenuIsAltSpeedUp,
                "Fast", submenuAcceptButtonStyle);

        // Set input digits
        GUIStyle buttonWithoutPadding = new GUIStyle("Button");
        buttonWithoutPadding.padding = new RectOffset(4, 4, 4, 4);

        for (ushort i = 0; i < nDigits+2; i++)
        {

            if (i < 3)
            {
                // ##### Up buttons #####
                if (GUI.Button(new Rect(popupOffset + 2 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset,
                            submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpIcon, buttonWithoutPadding))
                {
                    //Debug.Log("UP_" + i);
                    ChangeNumber_ALT(1, i);

                }//if-up button

                // ##### Input boxes #####
                // Name the input boxes to can be accessed by focus control
                GUI.SetNextControlName(inputsName + "_" + i);

                submenuDigits[i] = GUI.TextField(new Rect(popupOffset + 2 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y,
                            submenuInputBoxSize.x, submenuInputBoxSize.y),
                            submenuDigits[i], 1, submenuInputBoxStyle);

                // Control only digits
                submenuDigits[i] = (submenuDigits[i] != "" && char.IsDigit(submenuDigits[i][0]) ? submenuDigits[i] : "0");

                // ##### Down buttons #####
                if (GUI.Button(new Rect(popupOffset + 2 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonDownIcon, buttonWithoutPadding))
                {
                    //Debug.Log("DN_" + i);
                    ChangeNumber_ALT(-1, i);

                }//if-down buttons
            }
            else
            {
                // Set hundred and tenth of feet to '0' to convert from FL to ALT
                GUI.Label(new Rect(popupOffset + 2 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y,
                            submenuInputBoxSize.x, submenuInputBoxSize.y),
                            "0", submenuInputBoxStyle);
            }

            

        }

        // Set "Accept" button
        if (GUI.Button(new Rect(popupOffset, popupOffset + 2 * submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSize.x - popupOffset, submenuAcceptButtonSize.y),
                        acceptText, submenuAcceptButtonStyle) /*|| Input.GetButton("Accept")*/)
        {

            AcceptPressed_ALT();

        }// if-button


        // Keyboard input control
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.Tab && e.keyCode != KeyCode.Return && e.keyCode != KeyCode.KeypadEnter)
        {
            //ChangeNumber_ALT(0, -1);
            string currentInput = GUI.GetNameOfFocusedControl().Split("_"[0])[1];
            ushort n = 0;
            ushort.TryParse(currentInput, out n);
            n = (ushort)(n >= nDigits - 1 ? 0 : ++n);
            string nextInput = inputsName + "_" + n.ToString();
            //		Debug.Log("nextInput: " + inputsName + "_" + n);
            GUI.FocusControl(inputsName + "_" + n);
        }
        else if ((e.isKey && e.keyCode == KeyCode.Return) || (e.isKey && e.keyCode == KeyCode.KeypadEnter))
        {
            ushort n = 0;
            ushort.TryParse(submenuDigits[0].ToString() + submenuDigits[1].ToString() + submenuDigits[2].ToString() + "00", out n);
            tgtAlt = CheckRange_ALT(n);
            AcceptPressed_ALT();
        }

        if (setupSubmenu)
        {
            GUI.FocusControl(inputsName + "_0");
            string currentAltitude = string.Format("{0:D5}", acftCtrl.GetAircraft().GetAltitude());

            /*for (ushort j = 0; j < currentAltitude.Length; j++)*/
            for (ushort j = 0; j < nDigits; j++)
            {
                submenuDigits[j] = currentAltitude[j].ToString();
            }
            submenuIsAltSpeedUp = false;

            setupSubmenu = false;
        }

    }



    // Sets commands to aircraft when heading is set and 'accept' button is pressed
    void AcceptPressed_Point()
    {
        FIX tgtPoint = CreateObjects.fixList.ElementAt(0).Value;   // TODO: select from list

        //Debug.Log("Point: " + tgtHdg);

        //showPointPopup = false;

        // update radar screen tag of this aircraft
        acftCtrl.GetAircraft().SetAuthoPoint(tgtPoint);
        DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.GetAircraft());

        // simulate the communication text between ATC and pilots
        //string debugText = acftCtrl.GetAircraft().GetCallsign() + " " + TextUtils.Text2SpellFormat(acftCtrl.GetAircraft().GetFlightNumber()) + ", fly to " + tgtPoint.GetId();
        string debugText = string.Empty;
        if (tgtPoint.GetType() == typeof(VOR))
        {
            ;
            //debugText = acftCtrl.GetAircraft().GetCallsign() + " " + TextUtils.Text2SpellFormat(acftCtrl.GetAircraft().GetFlightNumber()) + ", fly to " + (tgtPoint as VOR).GetName() + " V O R";
        }
        else
        {
            debugText = acftCtrl.GetAircraft().GetCallsign() + " " + TextUtils.Text2SpellFormat(acftCtrl.GetAircraft().GetFlightNumber()) + ", fly to " + tgtPoint.GetId();
        }

        //Debug.LogWarning(debugText);
        MngDialogs.SetText(debugText, 0);

        // set commands to the aircraft
        acftCtrl.FlyTo(tgtPoint);

    } // AcceptPressed_HDG

















    void ChangeNumber_SPD(short variation, ushort digit)
    {

        //Debug.Log("ChangeNumber_SPD(" + variation + ", " + digit + ")");

        ushort k = 0;

        tgtSpd = 0;
        // Convert inputs to number
        for (k = 0; k < nDigits; k++)
        {
            if (k == digit)
            {
                //Debug.Log("k == digit: " + k);

                ushort.TryParse(submenuDigits[k], out aux[k]);
                aux[k] += (ushort)variation;
                //	 			if(aux[k] > maxHeading || aux[k] < minHeading){
                //	 				ushort.TryParse(maxHeading.ToString()[k].ToString(), aux[k]);
                //	 			}		
            }
            else
            {
                //Debug.Log("k != digit: " + k);

                ushort.TryParse(submenuDigits[k], out aux[k]);
            }

            aux[k] = (ushort)(aux[k] * Mathf.Pow(10, nDigits - k - 1));

            //Debug.Log("aux[k]: " + aux[k]);

            tgtSpd += aux[k];
            //Debug.Log("tgtSpd: " + tgtSpd);
        }

        tgtSpd = CheckRange_SPD(tgtSpd);

        // Convert number to inputs
        string str = string.Format("{0:D3}", tgtSpd);
        for (k = 0; k < nDigits; k++)
        {
            submenuDigits[k] = str[k].ToString();
        }
    } // ChangeNumber_SPD

    ushort CheckRange_SPD(ushort spd_in)
    {
        ushort spd_out;
        if (spd_in < HDG_MIN)
            spd_out = HDG_MAX;
        else if (spd_in > HDG_MAX)
            spd_out = HDG_MIN;
        else
            spd_out = spd_in;

        //Debug.Log("CheckRange_SPD: " + spd_out);
        return spd_out;
    }

    // Sets commands to aircraft when heading is set and 'accept' button is pressed
    void AcceptPressed_SPD()
    {
        //Debug.Log("SPD: " + tgtSpd);
        if (tgtSpd != acftCtrl.GetAircraft().GetSpeedGS())
        {

            showSpeedPopup = false;

            // update radar screen tag of this aircraft
            string spdStr = string.Format("{0:D3}", tgtSpd);
            //acftCtrl.GetAircraft().SetAuthoSpeed(tgtSpd);
            DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.GetAircraft());

            // simulate the communication text between ATC and pilots
            string debugText = acftCtrl.GetAircraft().GetCallsign() + " " + TextUtils.Text2SpellFormat(acftCtrl.GetAircraft().GetFlightNumber()) 
                + ", " + (submenuIsSpeedSpeedUp ? "expedite " : "") + "speed ";
            debugText += spdStr + " knots";
            //Debug.LogWarning(debugText);
            MngDialogs.SetText(debugText, 0);

            // set commands to the aircraft
            //acftCtrl.GetAircraft().SetAuthoSpeed(tgtSpd);
            acftCtrl.ChangeSpeed(tgtSpd, submenuIsSpeedSpeedUp);
        }
        else
        {
            // heading and requested heading are equals
        }

    }



    // Make the contents of the window
    void DoSpeedPopup(int windowID) {

        /* Speed submenu
	     * _________________
	     * |     |<||<||<| |     XXXXX = {"Speed"}
	     * |XXXXX N  N  N  |	     N = {0, 9}
	     * |_____|>||>||>|_|
	     * |_____ACCEPT____|
	     */

        ushort nDigits = (ushort) SPD_MAX.ToString().Length;

        // Show "Speed" text left to inputs
        string asideText = "Speed [kt]";                    // text to show at left of inputs				
        GUIStyle textStyle = new GUIStyle(submenuAsideTextStyle);
        textStyle.alignment = TextAnchor.MiddleCenter;

        // Show aircraft callsign in upper left corner
        GUIStyle acftLabelStyle = new GUIStyle(submenuAsideTextStyle);
        acftLabelStyle.alignment = TextAnchor.UpperLeft;
        acftLabelStyle.fontSize = 9;
        GUI.Label(new Rect(3 * popupOffset, popupOffset, 3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber(), acftLabelStyle);

        // Set input digits
        GUI.Label(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
                    3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                    asideText, textStyle);
                
        // Set "speed up" button
        submenuIsSpeedSpeedUp = GUI.Toggle(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y + submenuAsideTextSize.y,
                3 * submenuAsideTextSize.x, submenuAsideTextSize.y - 2 * popupOffset), submenuIsSpeedSpeedUp,
                "Fast", submenuAcceptButtonStyle);

        // Set input digits
        GUIStyle buttonWithoutPadding = new GUIStyle("Button");
        buttonWithoutPadding.padding = new RectOffset(4, 4, 4, 4);

        for (ushort i = 0; i < nDigits; i++)
        {

            // ##### Up buttons #####
            if (GUI.Button(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpIcon, buttonWithoutPadding))
            {
                //Debug.Log("UP_" + i);
                ChangeNumber_SPD(1, i);

            }//if-up button


            // ##### Input boxes #####
            // Name the input boxes to can be accessed by focus control
            GUI.SetNextControlName(inputsName + "_" + i);

            submenuDigits[i] = GUI.TextField(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y,
                        submenuInputBoxSize.x, submenuInputBoxSize.y),
                        submenuDigits[i], 1, submenuInputBoxStyle);

            // Control only digits						
            submenuDigits[i] = (submenuDigits[i] != "" && char.IsDigit(submenuDigits[i][0]) ? submenuDigits[i] : "0");

            // ##### Down buttons #####
            if (GUI.Button(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonDownIcon, buttonWithoutPadding))
            {
                //Debug.Log("DN_" + i);
                ChangeNumber_SPD(-1, i);

            }//if-down buttons

        }//for	

        // Set "Accept" button
        if (GUI.Button(new Rect(popupOffset, popupOffset + 2 * submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSize.x - popupOffset, submenuAcceptButtonSize.y),
                        acceptText, submenuAcceptButtonStyle) /*|| Input.GetButton("Accept")*/)
        {

            AcceptPressed_SPD();

        }// if-button

        // Keyboard input control
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.Tab && e.keyCode != KeyCode.Return && e.keyCode != KeyCode.KeypadEnter)
        {
            //ChangeNumber_SPD(0, -1);
            string currentInput = GUI.GetNameOfFocusedControl().Split("_"[0])[1];
            ushort n = 0;
            ushort.TryParse(currentInput, out n);
            n = (ushort) (n >= nDigits - 1 ? 0 : ++n);
            string nextInput = inputsName + "_" + n.ToString();
            //		Debug.Log("nextInput: " + inputsName + "_" + n);
            GUI.FocusControl(inputsName + "_" + n);
        }
        else if ((e.isKey && e.keyCode == KeyCode.Return) || (e.isKey && e.keyCode == KeyCode.KeypadEnter))
        {
            ushort n = SPD_MIN;
            ushort.TryParse(submenuDigits[0].ToString() + submenuDigits[1].ToString() + submenuDigits[2].ToString(), out n);
            tgtSpd = CheckRange_SPD(n);
            AcceptPressed_SPD();
        }
    

        if (setupSubmenu)
        {
            GUI.FocusControl(inputsName + "_0");
            string currentSpeed = string.Format("{0:D3}", acftCtrl.GetAircraft().GetSpeedGS());
            for (ushort j = 0; j < currentSpeed.Length; j++)
            {
                submenuDigits[j] = currentSpeed[j].ToString();
            }
            submenuIsSpeedSpeedUp = false;

            setupSubmenu = false;
        }

    }


    // Make the contents of the window
    void DoFlyToPopup(int windowID)
    {
        // TODO
        Debug.Log("DoFlyToPopup");
    }// DoFlyToPopup

    // Make the contents of the window
    void DoProceduresPopup(int windowID)
    {
        // TODO
        Debug.Log("DoProceduresPopup");
    }// DoProceduresPopup






    void OnGUI()
    {
        // Enable control of key buttons when a TextField is focused
        /*Input.eatKeyPressOnTextFieldFocus = false;*/

        if (initGUI)
        {
            initGUI = false;

            // MENUS

            popupStyle = popup_guistyle.GetStyle("Box");
            //		popupOffset = 1; 	// offset in pixels
            buttonStyle = popup_guistyle.GetStyle("Button");
            buttonStyle.fontSize = 10;

            numberShortcutStyle = new GUIStyle("Label");
            numberShortcutStyle.fontSize = buttonStyle.fontSize;
            //numberShortcutStyle.normal.textColor.a = 0.6;

            numberShortcutSize = popup_guistyle.GetStyle("Label").CalcSize(new GUIContent("X"));

            // ### Controlled aircrafts popup ###		
            ushort maxButtonWidth = 0;
            ushort maxButtonHeight = 0;
            // Get size of all buttons to apply it to popup window
            foreach (string text in ctrlTexts)
            {
                // Get height of each button
                ctrlButtonSize = buttonStyle.CalcSize(new GUIContent(text));
                if (ctrlButtonSize.x > maxButtonWidth)
                    maxButtonWidth = (ushort) ctrlButtonSize.x;
                if (ctrlButtonSize.y > maxButtonHeight)
                    maxButtonHeight = (ushort) ctrlButtonSize.y;
            }

            ctrlButtonSize = new Vector2(maxButtonWidth, maxButtonHeight);

            ctrlPopupSize = new Vector2(ctrlButtonSize.x + 1.4f * numberShortcutSize.x + popupOffset * 2,
                                        ctrlButtonSize.y * ctrlTexts.Count + popupOffset * 2);
            ctrlPopupRect = new Rect(Screen.width / 2f, Screen.height / 2f, ctrlPopupSize.x, ctrlPopupSize.y);


            // ### Incoming but no controlled aircrafts popup ###	
            // Get size of all buttons to apply it to popup window
            foreach (string text in noCtrlTexts)
            {
                // Get height of each button
                noCtrlButtonSize = buttonStyle.CalcSize(new GUIContent(text));
                if (noCtrlButtonSize.x > maxButtonWidth)
                    maxButtonWidth = (ushort) noCtrlButtonSize.x;
                if (noCtrlButtonSize.y > maxButtonHeight)
                    maxButtonHeight = (ushort) noCtrlButtonSize.y;
            }

            noCtrlButtonSize = new Vector2(maxButtonWidth, maxButtonHeight);


            noCtrlPopupSize = new Vector2(noCtrlButtonSize.x + 1.4f * numberShortcutSize.x + popupOffset * 2,
                                        noCtrlButtonSize.y * noCtrlTexts.Count + popupOffset * 2);
            noCtrlPopupRect = new Rect(Screen.width / 2f, Screen.height / 2f, noCtrlPopupSize.x, noCtrlPopupSize.y);

            // ### Transferred aircrafts popup ###	
            // Get size of all buttons to apply it to popup window
            foreach (string text in transCtrlTexts)
            {
                // Get height of each button
                transCtrlButtonSize = buttonStyle.CalcSize(new GUIContent(text));
                if (transCtrlButtonSize.x > maxButtonWidth)
                    maxButtonWidth = (ushort) transCtrlButtonSize.x;
                if (transCtrlButtonSize.y > maxButtonHeight)
                    maxButtonHeight = (ushort) transCtrlButtonSize.y;
            }

            transCtrlButtonSize = new Vector2(maxButtonWidth, maxButtonHeight);

            transCtrlPopupSize = new Vector2(transCtrlButtonSize.x + 1.4f * numberShortcutSize.x + popupOffset * 2,
                                            transCtrlButtonSize.y * transCtrlTexts.Count + popupOffset * 2);
            transCtrlPopupRect = new Rect(Screen.width / 2f, Screen.height / 2f, transCtrlPopupSize.x, transCtrlPopupSize.y);




            // SUBMENUS

            submenuAsideTextStyle = new GUIStyle("Label");
            submenuAsideTextStyle.fontSize = 11;
            submenuInputBoxStyle = new GUIStyle("TextField");
            submenuInputBoxStyle.fontSize = 12;
            submenuAcceptButtonStyle = new GUIStyle("Button");
            submenuAcceptButtonStyle.fontSize = 9;

            submenuAsideTextSize = submenuAsideTextStyle.CalcSize(new GUIContent("XXX"));
            submenuInputBoxSize = submenuInputBoxStyle.CalcSize(new GUIContent("N"));
            submenuSelButtonSize = new Vector2(submenuInputBoxSize.x, submenuInputBoxSize.y / 1.5f);
            submenuAcceptButtonSize = submenuAcceptButtonStyle.CalcSize(new GUIContent(acceptText));

            /* General submenu appeareance
		     * _________________
		     * |     |<||<||<| |   
		     * | XXX  N  N  N  |	
		     * |_____|>||>||>|_|
		     * |_____ACCEPT____|
		     */

            submenuSize = new Vector2(2 * submenuAsideTextSize.x + submenuMaxNumberOfDigits * submenuInputBoxSize.x + popupOffset * 2,
                        submenuInputBoxSize.y + 2 * submenuSelButtonSize.y + submenuAcceptButtonSize.y + popupOffset * 2);
            submenuRect = new Rect(Screen.width / 2f, Screen.height / 2f, submenuSize.x, submenuSize.y);

        }



        // Event to manage mouse position and GUI or number pressed to select GUI option
        Event e = Event.current;

        bool isKeyDown = e.type == EventType.KeyDown;
        bool isDigit = char.IsDigit(e.character);

        // MENUS
        if (showCtrlGUI)
        {

            // Register the window. Notice the 3rd parameter 
            ctrlPopupRect = GUI.Window(ctrlId, ctrlPopupRect, DoCtrlPopup, "", popupStyle);

            // If user selects an option of GUI by number button
            if (isKeyDown && isDigit)
            {
                int n = int.Parse(e.character.ToString());
                if (n == 0)
                    MngCtrlPressedButton(9);        // button 0 is option 10 (9)
                else
                    MngCtrlPressedButton(n - 1);        // options starts from 0
            }

            // If user clicks away from GUI, hide it
            if (e.type == EventType.MouseDown && !ctrlPopupRect.Contains(e.mousePosition))
            {
                showCtrlGUI = false;
            }


        }
        else if (showNoCtrlGUI)
        {

            // Register the window. Notice the 3rd parameter 
            noCtrlPopupRect = GUI.Window(noCtrlId, noCtrlPopupRect, DoNoCtrlPopup, "", popupStyle);

            // If user clicks away from GUI, hide it
            if (e.type == EventType.MouseDown && !noCtrlPopupRect.Contains(e.mousePosition))
            {
                showNoCtrlGUI = false;
            }

        }
        else if (showTransCtrlGUI)
        {

            // Register the window. Notice the 3rd parameter 
            transCtrlPopupRect = GUI.Window(transCtrlId, transCtrlPopupRect, DoTransCtrlPopup, "", popupStyle);

            // If user clicks away from GUI, hide it
            if (e.type == EventType.MouseDown && !transCtrlPopupRect.Contains(e.mousePosition))
            {
                showTransCtrlGUI = false;
            }




            // SUBMENUS
        }
        else
        {
            if (showHeadingPopup)
            {
                submenuRect = GUI.Window(headingPopupId, submenuRect, DoHeadingPopup, "", popupStyle);

                // If user clicks away from GUI, hide it
                if (e.type == EventType.MouseDown && !submenuRect.Contains(e.mousePosition))
                {
                    showHeadingPopup = false;
                }//if	
            }
            else if (showAltitudePopup)
            {
                submenuRect = GUI.Window(altitudePopupId, submenuRect, DoAltitudePopup, "", popupStyle);

                // If user clicks away from GUI, hide it
                if (e.type == EventType.MouseDown && !submenuRect.Contains(e.mousePosition))
                {
                    showAltitudePopup = false;
                }//if
            }
            else if (showSpeedPopup)
            {
                submenuRect = GUI.Window(speedPopupId, submenuRect, DoSpeedPopup, "", popupStyle);

                // If user clicks away from GUI, hide it
                if (e.type == EventType.MouseDown && !submenuRect.Contains(e.mousePosition))
                {
                    showSpeedPopup = false;
                }//if
            }
            else if (showFlyToPopup)
            {
                submenuRect = GUI.Window(speedPopupId, submenuRect, DoFlyToPopup, "", popupStyle);

                // If user clicks away from GUI, hide it
                if (e.type == EventType.MouseDown && !submenuRect.Contains(e.mousePosition))
                {
                    showFlyToPopup = false;
                }//if
            }
            else if (showProceduresPopup)
            {
                submenuRect = GUI.Window(speedPopupId, submenuRect, DoProceduresPopup, "", popupStyle);

                // If user clicks away from GUI, hide it
                if (e.type == EventType.MouseDown && !submenuRect.Contains(e.mousePosition))
                {
                    showProceduresPopup = false;
                }//if
            }
            else
            {
                acftCtrl = null;
            }
        }//else


    }//OnGUI
    

}//class