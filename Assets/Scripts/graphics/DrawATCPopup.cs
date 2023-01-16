
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    GUISkin popup_guistyle;

    public Texture buttonUpText;
    public Texture buttonDownText;
    public Texture buttonLeftText;
    public Texture buttonRightText;
    public Texture turnLeftText;
    public Texture turnText;
    public Texture turnRightText;

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


    short heading;
    int altitude;
    short speed;

    public void Awake()
    {
        submenuHeadingToolbarTextures = new Texture[] { buttonLeftText, buttonUpText, buttonRightText };
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
        ctrlTexts.Add("Climb");
        ctrlTexts.Add("Descend");
        ctrlTexts.Add("Increase Speed");
        ctrlTexts.Add("Reduce Speed");
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
                // Do something
                //			acftCtrl.Turn(045);
                //			acftCtrl.TurnRight(090);
                ShowHeadingPopup(Input.mousePosition);
                break;
            case 1:
                // Do something
                ShowAltitudePopup(Input.mousePosition);
                break;
            case 2:
                // Do something
                ShowSpeedPopup(Input.mousePosition);
                //			acftCtrl.TurnLeft(270);
                break;
            case 3:
                acftCtrl.Climb(12000, false);
                break;
            case 4:
                acftCtrl.Descend(3000, true);
                break;
            case 5:
                acftCtrl.IncreaseSpeed(280, true);
                break;
            case 6:
                acftCtrl.ReduceSpeed(180, false);
                break;
            default:
                showCtrlGUI = false;
                break;
        }
        showCtrlGUI = false;
        //		Debug.Log("Pressed " + ctrlTexts[i]);

    }

    // Make the contents of the window
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

    // Make the contents of the window
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
                        // Do something
                        Debug.Log("Pressed");
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

    // Make the contents of the window
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
                        // Do something
                        Debug.Log("Pressed");
                        // if is Outgoing traffic
                        if (acftCtrl.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Transferred)
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


    // Make the contents of the window
    
    void DoHeadingPopup(int windowID)
    {

        /* Heading submenu
	     * _________________
	     * |     |<||<||<| |     XXX = {ALT, FL}
	     * | XXX  N  N  N  |	 NNN = {0, 9}
	     * |_____|>||>||>|_|
	     * |_____ACCEPT____|
	     */

        //	string number;						// number introduced through inputs
        //	ushort heading;						// store the number after conversion from String


        ushort minHeading = 001;   // in degrees
        ushort maxHeading = 360;   // in degrees	
        ushort nDigits = (ushort) maxHeading.ToString().Length;
        ushort[] aux = new ushort[nDigits];
        ushort k = 0;
        /*
        var changeNumber = function(variation: short, digit: short){
            heading = 0;
            // Convert inputs to number
            for (k = 0; k < nDigits; k++)
            {
                if (k == digit)
                {
                    ushort.TryParse(submenuDigits[k], aux[k]);
                    aux[k] += variation;
                    //	 			if(aux[k] > maxHeading || aux[k] < minHeading){
                    //	 				ushort.TryParse(maxHeading.ToString()[k].ToString(), aux[k]);
                    //	 			}		
                }
                else
                {
                    ushort.TryParse(submenuDigits[k], aux[k]);
                }

                aux[k] = aux[k] * Mathf.Pow(10, nDigits - k - 1);

                heading += aux[k];
            }

            if (heading < minHeading)
                heading = maxHeading;
            else if (heading > maxHeading)
                heading = minHeading;
            else
                heading = heading % 360;
            heading = (heading == 0 ? 360 : heading);

            // Convert number to inputs
            var str = String.Format("{0:D3}", heading);
            for (k = 0; k < nDigits; k++)
            {
                submenuDigits[k] = str[k].ToString();
            }
        }; //changeNumber
        */
        /*
        var acceptPressed = function(){
            Debug.Log("HDG: " + heading);
            if (heading != acftCtrl.aircraft.heading)
            {

                showHeadingPopup = false;

                switch (submenuHeadingToolbarInt)
                {
                    case 0:
                        acftCtrl.TurnLeft(heading);
                        break;
                    case 1:
                        acftCtrl.Turn(heading);
                        break;
                    case 2:
                        acftCtrl.TurnRight(heading);
                        break;
                }

                var hdgStr = String.Format("{0:D3}", heading);
                acftCtrl.aircraft.authoPoint = "H" + hdgStr;
                DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.aircraft);

            }
            else
            {
                // heading and requested heading are equals
            }

        };
        */



        string asideText = "Heading";                  // text to show at left of inputs				
        GUIStyle textStyle = new GUIStyle(submenuAsideTextStyle);
        textStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle acftLabelStyle = new GUIStyle(submenuAsideTextStyle);
        acftLabelStyle.alignment = TextAnchor.UpperLeft;
        acftLabelStyle.fontSize = 9;

        GUI.Label(new Rect(3 * popupOffset, popupOffset, 3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber(), acftLabelStyle);

        GUI.Label(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
                    3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                    asideText, textStyle);

        GUIStyle toolbarStyle = new GUIStyle("Button");
        toolbarStyle.margin = new RectOffset(0, 0, 0, 0);
        toolbarStyle.padding = new RectOffset(0, 0, 3, 3);

        submenuHeadingToolbarInt = GUI.Toolbar(new Rect(popupOffset, submenuSelButtonSize.y + submenuAsideTextSize.y,
                3 * submenuAsideTextSize.x, submenuAsideTextSize.y - 2 * popupOffset), submenuHeadingToolbarInt,
                submenuHeadingToolbarTextures, toolbarStyle);

        GUIStyle buttonWithoutPadding = new GUIStyle("Button");
        buttonWithoutPadding.padding = new RectOffset(4, 4, 4, 4);


        for (short i = 0; i < nDigits; i++)
        {

            // ##### Up buttons #####
            if (GUI.Button(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpText, buttonWithoutPadding))
            {


                //changeNumber(1, i, nDigits, aux);

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
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonDownText, buttonWithoutPadding))
            {

                //changeNumber(-1, i, nDigits, aux);

            }//if-down buttons

        }//for	

        if (GUI.Button(new Rect(popupOffset, popupOffset + 2 * submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSize.x - popupOffset, submenuAcceptButtonSize.y),
                        acceptText, submenuAcceptButtonStyle) || Input.GetButton("Accept"))
        {

            //acceptPressed();

        }// if-button

        // Keyboard input control
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.Tab)
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

        if (setupSubmenu)
        {
            GUI.FocusControl(inputsName + "_0");
            string currentHdg = string.Format("{0:D3}", acftCtrl.GetAircraft().GetHeading());
            for (var j = 0; j < currentHdg.Length; j++)
            {
                submenuDigits[j] = currentHdg[j].ToString();
            }
            submenuHeadingToolbarInt = 1;

            setupSubmenu = false;
        }

    }
    /*
    void changeNumber(short variation, short digit, ushort nDigits, ushort[] aux)
    {
        ushort altitude = 0;

        ushort k = 0; 
        // Convert inputs to number
        for (k = 0; k < nDigits; k++)
        {
            if (k == digit)
            {
                if (k < ctrlInputIndex)
                {
                    int.TryParse(submenuDigits[k], out aux[k]);
                    aux[k] += variation;
                }
                else
                {
                    aux[k] = 0;
                }//if-ctrlInputIndex
            }
            else
            {
                int.TryParse(submenuDigits[k], out aux[k]);
            }

            if (submenuIsAltitude)
                aux[k] = aux[k] * Mathf.Pow(10, nDigits - k - 1);
            else
                aux[k] = aux[k] * Mathf.Pow(10, (nDigits - k - 1) + 2);

            altitude += aux[k];

            //			Debug.Log("submenuDigits[" + k + "]: " + submenuDigits[k] + " (" + aux[k] + ")");
        }


        if (altitude < minAltitude)
        {
            altitude = minAltitude;
            Debug.Log("alt < min (" + minAltitude + ")");
        }
        else if (altitude > maxAltitude)
        {
            altitude = maxAltitude;
            Debug.Log("alt > max (" + maxAltitude + ")");
        }


        // Convert number to inputs
        ushort aux_j = (altitude >= 10000 ? 0 : 1);
        string currentAltitude = (submenuIsAltitude ? altitude.ToString() : altitude.ToString() + "00");

        if (aux_j > 0)
            submenuDigits[0] = "0";
        ushort limit = (submenuIsAltitude ? nDigits - 1 : nDigits);
        for (ushort j = 0; j < limit; j++)
        {
            Debug.Log("number(" + (j + aux_j) + "): " + submenuDigits[j + aux_j]);
            submenuDigits[j + aux_j] = (currentAltitude.ToString())[j].ToString();
        }


        Debug.Log("altitude: " + altitude);
    }; //changeNumber
    */
    /*
    void acceptPressed()
    {
        Debug.Log("ALT: " + altitude);

        if (altitude != acftCtrl.aircraft.GetAltitude())
        {

            showAltitudePopup = false;

            acftCtrl.ChangeLevel(altitude, submenuIsAltSpeedUp);
            acftCtrl.aircraft.SetAuthoAltitude(altitude);
            DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.GetAircraft());

        }
        else
        {
            // altitude and requested altitude are equals
        }

    };
    */

    // Make the contents of the window

    void DoAltitudePopup(int windowID) {

        /* Heading submenu
	     * _________________
	     * |     |<||<||<| |     XXX = {ALT, FL}
	     * | XXX  N  N  N  |	 NNN = {0, 9}
	     * |_____|>||>||>|_|
	     * |_____ACCEPT____|
	     */


        //	submenuIsAltitude = GUI.Toggle(Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
        //				2*submenuAsideTextSize.x + sizeCtrl, submenuAsideTextSize.y), submenuIsAltitude,
        //				asideText, submenuAcceptButtonStyle);

        submenuIsAltitude = (altitude <= CreateObjects.airport.GetTransAltitude() ? true : false);
        // number of digits for input (here to avoid problems with change between A and FL)
        ushort nDigits = (ushort) (submenuIsAltitude ? 5 : 3);

        ushort ctrlInputIndex = (ushort) (submenuIsAltitude ? nDigits - 2 : nDigits);       // How many input boxs can be modified?

        string asideText = (submenuIsAltitude ? "ALT" : "FL");                 // text to show at left of inputs
        ushort sizeCtrl = (ushort) (submenuIsAltitude ? 0 : 2 * submenuInputBoxSize.x); // change the size of asideText to fit number of inputs to window
        string number;                                                // number introduced through inputs

        //	Debug.Log("ctrlInputIndex: " + ctrlInputIndex);					
        GUIStyle toolbarStyle = new GUIStyle("Button");
        toolbarStyle.margin = new RectOffset(0, 0, 0, 0);
        toolbarStyle.padding = new RectOffset(0, 0, 3, 3);

        submenuIsAltSpeedUp = GUI.Toggle(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y + submenuAsideTextSize.y,
                2 * submenuAsideTextSize.x + sizeCtrl, submenuAsideTextSize.y - 2 * popupOffset), submenuIsAltSpeedUp,
                "Fast", submenuAcceptButtonStyle);



        GUIStyle acftLabelStyle = new GUIStyle(submenuAsideTextStyle);
        acftLabelStyle.alignment = TextAnchor.UpperLeft;
        acftLabelStyle.fontSize = 9;

        GUI.Label(new Rect(3 * popupOffset, popupOffset, 3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber(), acftLabelStyle);

        GUIStyle textStyle = new GUIStyle(submenuAsideTextStyle);
        textStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
                    2 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                    asideText, textStyle);

        GUIStyle buttonWithoutPadding = new GUIStyle("Button");
        buttonWithoutPadding.padding = new RectOffset(4, 4, 4, 4);


        int minAltitude = 00000;      // in feet
        int maxAltitude = 55000;      // in feet	
                                      //	var nDigits = maxHeading.ToString().Length;
        int[] aux = new int[maxAltitude.ToString().Length];
        

        for (ushort i = 0; i < nDigits; i++)
        {

            // ##### Up buttons #####
            //		GUI.DrawTexture(Rect(popupOffset + 2*submenuAsideTextSize.x + sizeCtrl + i*submenuInputBoxSize.x, popupOffset,
            //					submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpText, ScaleMode.ScaleToFit, true);
            if (GUI.Button(new Rect(popupOffset + 2 * submenuAsideTextSize.x + sizeCtrl + i * submenuInputBoxSize.x, popupOffset,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpText, buttonWithoutPadding))
            {

                //changeNumber(1, i);

            }//if-up button


            // ##### Input boxes #####
            // Name the input boxes to can be accessed by focus control
            GUI.SetNextControlName(inputsName + "_" + i);

            submenuDigits[i] = GUI.TextField(new Rect(popupOffset + 2 * submenuAsideTextSize.x + sizeCtrl + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y,
                        submenuInputBoxSize.x, submenuInputBoxSize.y),
                        submenuDigits[i], 1, submenuInputBoxStyle);


            // Control only digits
            submenuDigits[i] = (submenuDigits[i] != "" && char.IsDigit(submenuDigits[i][0]) ? submenuDigits[i] : "0");

            // ##### Down buttons #####
            if (GUI.Button(new Rect(popupOffset + 2 * submenuAsideTextSize.x + sizeCtrl + i * submenuInputBoxSize.x, popupOffset + submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonDownText, buttonWithoutPadding))
            {

                //changeNumber(-1, i, nDigits);

            }//if-down buttons

        }

        //	number = (submenuIsAltitude ? number : number + "00");	
        //	
        //	ushort.TryParse(number, altOrFl);			// Convert String to ushort

        //	GUI.Label(Rect(0, 0, submenuSize.x, 20), altOrFl.ToString() + " ft");

        if (GUI.Button(new Rect(popupOffset, popupOffset + 2 * submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSize.x - popupOffset, submenuAcceptButtonSize.y),
                        acceptText, submenuAcceptButtonStyle) || Input.GetButton("Accept"))
        {

            //acceptPressed();

        }// if-button


        // Keyboard input control
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.Tab)
        {
            //changeNumber(0, -1);
            string currentInput = GUI.GetNameOfFocusedControl().Split("_"[0])[1];
            ushort n = 0;
            ushort.TryParse(currentInput, out n);
            n = (ushort) (n >= 2 ? 0 : ++n);
            var nextInput = inputsName + "_" + n.ToString();
            //		Debug.Log("nextInput: " + inputsName + "_" + n);
            GUI.FocusControl(inputsName + "_" + n);
        }

        if (setupSubmenu)
        {
            GUI.FocusControl(inputsName + "_0");

            var currentAltitude = acftCtrl.GetAircraft().GetAltitude();

            altitude = currentAltitude;

            var aux_j = (currentAltitude >= 10000 ? 0 : 1);
            if (aux_j > 0)
                submenuDigits[0] = "0";
            var limit = (submenuIsAltitude ? nDigits - 1 : nDigits);
            for (var j = 0; j < limit; j++)
            {
                submenuDigits[j + aux_j] = (currentAltitude.ToString())[j].ToString();
            }
            submenuIsAltSpeedUp = false;

            setupSubmenu = false;
        }

    }


    // Make the contents of the window
    void DoSpeedPopup(int windowID) {

        /* Speed submenu
	     * _________________
	     * |     |<||<||<| |     XXXXX = {SPEED}
	     * |XXXXX N  N  N  |	 NNN = {0, 9}
	     * |_____|>||>||>|_|
	     * |_____ACCEPT____|
	     */

        ushort minSpeed = 100;     // in kts
        ushort maxSpeed = 500;     // in kts	
        ushort nDigits = (ushort) maxSpeed.ToString().Length;
        ushort[] aux = new ushort[nDigits];
        ushort k = 0;
        /*
        var changeNumber = function(variation: short, digit: short){
            speed = 0;
            // Convert inputs to number
            for (k = 0; k < nDigits; k++)
            {
                if (k == digit)
                {
                    ushort.TryParse(submenuDigits[k], aux[k]);
                    aux[k] += variation;
                    //	 			if(aux[k] > maxHeading || aux[k] < minHeading){
                    //	 				ushort.TryParse(maxHeading.ToString()[k].ToString(), aux[k]);
                    //	 			}		
                }
                else
                {
                    ushort.TryParse(submenuDigits[k], aux[k]);
                }

                aux[k] = aux[k] * Mathf.Pow(10, nDigits - k - 1);

                speed += aux[k];
            }

            if (speed < minSpeed)
                speed = maxSpeed;
            else if (speed > maxSpeed)
                speed = minSpeed;

            // Convert number to inputs
            var str = String.Format("{0:D3}", speed);
            for (k = 0; k < nDigits; k++)
            {
                submenuDigits[k] = str[k].ToString();
            }
        }; //changeNumber
        */
        /*
        var acceptPressed = function(){
            Debug.Log("SPD: " + speed);
            if (speed != acftCtrl.aircraft.speedIAS)
            {

                showSpeedPopup = false;

                acftCtrl.ChangeSpeed(speed, submenuIsSpeedSpeedUp);
                acftCtrl.aircraft.authoSpeed = speed;
                DrawRadarScreen.UpdateAcftAuthLabel(acftCtrl.aircraft);

            }
            else
            {
                // speed and requested speed are equals
            }

        };
        */
        string asideText = "Speed";                    // text to show at left of inputs				
        GUIStyle textStyle = new GUIStyle(submenuAsideTextStyle);
        textStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle acftLabelStyle = new GUIStyle(submenuAsideTextStyle);
        acftLabelStyle.alignment = TextAnchor.UpperLeft;
        acftLabelStyle.fontSize = 9;

        GUI.Label(new Rect(3 * popupOffset, popupOffset, 3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                acftCtrl.GetAircraft().GetCallsignCode() + acftCtrl.GetAircraft().GetFlightNumber(), acftLabelStyle);

        GUI.Label(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y,
                    3 * submenuAsideTextSize.x, submenuAsideTextSize.y),
                    asideText, textStyle);

        GUIStyle buttonWithoutPadding = new GUIStyle("Button");
        buttonWithoutPadding.padding = new RectOffset(4, 4, 4, 4);

        submenuIsSpeedSpeedUp = GUI.Toggle(new Rect(popupOffset, popupOffset + submenuSelButtonSize.y + submenuAsideTextSize.y,
                3 * submenuAsideTextSize.x, submenuAsideTextSize.y - 2 * popupOffset), submenuIsSpeedSpeedUp,
                "Fast", submenuAcceptButtonStyle);


        for (ushort i = 0; i < nDigits; i++)
        {

            // ##### Up buttons #####
            if (GUI.Button(new Rect(popupOffset + 3 * submenuAsideTextSize.x + i * submenuInputBoxSize.x, popupOffset,
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonUpText, buttonWithoutPadding))
            {


                //changeNumber(1, i);

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
                        submenuSelButtonSize.x, submenuSelButtonSize.y), buttonDownText, buttonWithoutPadding))
            {

                //changeNumber(-1, i);

            }//if-down buttons

        }//for	

        if (GUI.Button(new Rect(popupOffset, popupOffset + 2 * submenuSelButtonSize.y + submenuInputBoxSize.y,
                        submenuSize.x - popupOffset, submenuAcceptButtonSize.y),
                        acceptText, submenuAcceptButtonStyle) || Input.GetButton("Accept"))
        {

            //acceptPressed();

        }// if-button

        // Keyboard input control
        Event e = Event.current;

        if (e.isKey && e.keyCode != KeyCode.Tab)
        {
            //changeNumber(0, -1);
            string currentInput = GUI.GetNameOfFocusedControl().Split("_"[0])[1];
            ushort n = 0;
            ushort.TryParse(currentInput, out n);
            n = (ushort) (n >= nDigits - 1 ? 0 : ++n);
            var nextInput = inputsName + "_" + n.ToString();
            //		Debug.Log("nextInput: " + inputsName + "_" + n);
            GUI.FocusControl(inputsName + "_" + n);
        }

        if (setupSubmenu)
        {
            GUI.FocusControl(inputsName + "_0");
            string currentSpeed = string.Format("{0:D3}", acftCtrl.GetAircraft().GetSpeedIAS());
            for (var j = 0; j < currentSpeed.Length; j++)
            {
                submenuDigits[j] = currentSpeed[j].ToString();
            }
            submenuIsSpeedSpeedUp = false;

            setupSubmenu = false;
        }

    }
    




    void OnGUI()
    {
        // Enable control of key buttons when a TextField is focused
        Input.eatKeyPressOnTextFieldFocus = false;

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
            else
            {
                acftCtrl = null;
            }
        }//else


    }//OnGUI
    

}//class