using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGUI : MonoBehaviour
{
    private static bool showGUI = false;
    private static bool showSIDs = true;
    private static bool showSTARs = true;
    private static bool showGrid = false;
    private static bool showRings = true;

    private static bool initGUI;

    private static int id;

    // Window for Flight Progress Strips
    public GUISkin windowFPS_guistyle;

    //static string windowFPSDefaultText;
    static float windowFPSWidth;
    static Rect windowFPS;
    //string windowFPSTitle = "Flight Progress Strips";
    string windowFPSTitle = "Flight Strips";
    Vector2 fpsTitleSize;
    Vector2 stripSize;
    
    GUIStyle scroll_guistyle;

    Vector2 arrivals_scrollPos = Vector2.zero;
    Vector2 departures_scrollPos = Vector2.zero;
    Vector2 arrivals_titleSize = Vector2.zero;
    Vector2 departures_titleSize = Vector2.zero;
    string arrivals_title = "Arrivals";
    string departures_title = "Departures";
    float arr_dep_panelHeight;

    float camera_size;
    int zoom_selection = 1;
    /*
    float distanceAssistantValue = -1.0f;
    bool existsDistRings = false;
    bool existsDistGrid = false;
    */

    List<Aircraft> arrivalAcfts;
    List<Aircraft> departureAcfts;

    public void Awake()
    {
        arrivalAcfts = new List<Aircraft>();
        departureAcfts = new List<Aircraft>();
    }

    public static void Init()
    {
        initGUI = true;
        id = General.AssignWindowId();
        windowFPSWidth = 90.0f;
        windowFPS = new Rect(0.0f, 0.0f, windowFPSWidth, Screen.height);
    }

    public static void StartDraw()
    {
        showGUI = true;
    }

    void OnGUI()
    {
        if (initGUI)
        {
            initGUI = false;

            fpsTitleSize = windowFPS_guistyle.GetStyle("Window").CalcSize(new GUIContent(windowFPSTitle));
            fpsTitleSize.y = fpsTitleSize.y + (windowFPS_guistyle.GetStyle("Window").padding.top);

            arrivals_titleSize = windowFPS_guistyle.GetStyle("Window").CalcSize(new GUIContent(arrivals_title));
            departures_titleSize = windowFPS_guistyle.GetStyle("Window").CalcSize(new GUIContent(departures_title));

            arr_dep_panelHeight = (Screen.height - fpsTitleSize.y) * 0.5f;

            stripSize = windowFPS_guistyle.GetStyle("Arrivals").CalcSize(new GUIContent("AAAXXXX AXXX\nFLXXX AAAAA"));
        }

        if (showGUI)
        {
            // Register the window. Notice the 3rd parameter 
            windowFPS = GUI.Window(id, windowFPS, DoWindowFPS, windowFPSTitle, windowFPS_guistyle.GetStyle("Window"));

            // Show airport info
            GUI.BeginGroup(new Rect(windowFPSWidth + 20f, 5f, Screen.width * 0.35f, 60f));
                // First line
                GUI.Label(new Rect(0f, 0f, Screen.width * 0.20f, 20f), 
                    "Airport: " + CreateObjects.airport.GetName() + " (" + CreateObjects.airport.GetCodeICAO() + " / " + CreateObjects.airport.GetCodeIATA() + ")");
                GUI.Label(new Rect(Screen.width * 0.20f, 0f, Screen.width * 0.15f, 20f),
                    "Location: " + CreateObjects.airport.GetCity() + ", " + CreateObjects.airport.GetCountry());

                // Second line
                GUI.Label(new Rect(0, 20f, Screen.width * 0.10f, 20f), 
                    "Lat.: " + CreateObjects.airport.GetLat().ToString("#.0000") + " ºN");       
                GUI.Label(new Rect(Screen.width * 0.10f, 20f, Screen.width * 0.10f, 20f),
                    "Trans. Level: FL" + CreateObjects.airport.GetTransLevel());
                GUI.Label(new Rect(Screen.width * 0.20f, 20f, Screen.width * 0.10f, 20f),
                        "Elev.: " + CreateObjects.airport.GetElevation() + " ft");               

                // Third line
                GUI.Label(new Rect(0f, 40f, Screen.width * 0.10f, 20f),
                        "Lon.: " + CreateObjects.airport.GetLon().ToString("#.0000") + " ºE");
                GUI.Label(new Rect(Screen.width * 0.10f, 40f, Screen.width * 0.10f, 20f),
                    "Trans. Alt.: " + CreateObjects.airport.GetTransAltitude() + " ft");
            GUI.EndGroup();

            // Control of draw for Grid, Rings, SIDs and STARs
            showGrid = GUI.Toggle(new Rect(Screen.width * 0.5f - 70f, 05f, 50f, 20f), showGrid, "Grid");
            showRings   = GUI.Toggle(new Rect(Screen.width * 0.5f - 70f, 25f, 50f, 20f), showRings, "Rings");

            showSIDs    = GUI.Toggle(new Rect(Screen.width * 0.5f + 20f, 05f, 50f, 20f), showSIDs, "SID");
            showSTARs   = GUI.Toggle(new Rect(Screen.width * 0.5f + 20f, 25f, 50f, 20f), showSTARs, "STAR");

            // Avoid to call every frame, only if selected option is not the drawing one
            if (showGrid && !DrawRadarScreen.is_showing_Grid)
                DrawRadarScreen.DrawGrid();
            else if (!showGrid && DrawRadarScreen.is_showing_Grid)
                DrawRadarScreen.HideGrid();

            if (showRings && !DrawRadarScreen.is_showing_Rings)
                DrawRadarScreen.DrawRings();
            else if (!showRings && DrawRadarScreen.is_showing_Rings)
                DrawRadarScreen.HideRings();

            if (showSIDs && !DrawRadarScreen.is_showing_SIDs)
                DrawRadarScreen.DrawSIDs();
            else if (!showSIDs && DrawRadarScreen.is_showing_SIDs)
                DrawRadarScreen.HideSIDs();

            if (showSTARs && !DrawRadarScreen.is_showing_STARs)
                DrawRadarScreen.DrawSTARs();
            else if (!showSTARs && DrawRadarScreen.is_showing_STARs)
                DrawRadarScreen.HideSTARs();

            // Zoom controls
            zoom_selection = GUI.Toolbar(new Rect(Screen.width * 0.75f -125, 5f, 250f, 20f), zoom_selection, new string[] { "Far", "AUTO", "Near", "Nearest" });
            switch (zoom_selection)
            {
                case 0:
                    camera_size = 120;
                    break;
                case 1:
                    camera_size = 100;
                    break;
                case 2:
                    camera_size = 80;
                    break;
                case 3:
                    camera_size = 60;
                    break;
                default:
                    camera_size = 100;
                    break;
            }
            MngCamera.SetCameraSize(camera_size);

            // Close button
            if (GUI.Button(new Rect(Screen.width - 100f, 5f, 50f, 20f), new GUIContent("Close", "Ctrl + Q")) ||
                ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.Q)))
            {
                Debug.Log("Application.Quit()");
                Application.Quit();
            }
            // Tooltip for close button
            GUI.Label(new Rect(Screen.width - 100f, 25f, 50f, 20f), GUI.tooltip);
        }// if
    }// OnGUI


    // Make the contents of the window
    void DoWindowFPS(int windowID)
    {
        //		GUI.Label(new Rect (10.0f, 20.0f, 100.0f, 20.0f), windowFPSDefaultText);
        GUI.BeginGroup(new Rect(0, fpsTitleSize.y, windowFPSWidth, arr_dep_panelHeight));

        GUI.Label(new Rect(0, 0, windowFPSWidth, arrivals_titleSize.y), arrivals_title, windowFPS_guistyle.GetStyle("TitleLabel"));
        arrivals_scrollPos = GUI.BeginScrollView(
                                    new Rect(0, arrivals_titleSize.y, windowFPSWidth, arr_dep_panelHeight - arrivals_titleSize.y),
                                    arrivals_scrollPos,
                                    new Rect(0, 0, windowFPSWidth - 20, CreateObjects.aircraftList.Count * stripSize.y), 
                                    GUIStyle.none, GUIStyle.none
                                );

        ushort authFL;
        string authFLStr;
        string strip;

        arrivalAcfts.Clear();
        departureAcfts.Clear();

        foreach (Aircraft acft in CreateObjects.aircraftList)
        {
            if (acft.GetFlightStatus() == Aircraft.FlightStatus.Arrival)
            {
                arrivalAcfts.Add(acft);
            }
            else if(acft.GetFlightStatus() == Aircraft.FlightStatus.Departure)
            {
                departureAcfts.Add(acft);
            }
            else
            {
                arrivalAcfts.Remove(acft);
                departureAcfts.Remove(acft);
            }
        }


        ushort i = 0;
        foreach (Aircraft acft in arrivalAcfts)
        {
            if (acft.GetAuthoAltitude() > CreateObjects.airport.GetTransAltitude())
            {
                authFL = (ushort) Mathf.Ceil(acft.GetAuthoAltitude() / 100f);
                authFLStr = (authFL < 100 ? "0" + authFL.ToString() : authFL.ToString());
                authFLStr = (authFL < 10 ? authFLStr + "0" : authFLStr);
                authFLStr = "FL" + authFLStr;
            }
            else
            {
                authFLStr = "A" + acft.GetAuthoAltitude().ToString();
            }

            strip = acft.GetCallsignCode() + acft.GetFlightNumber() + " " + acft.GetAircraftModelCode() + "\n" +
                                authFLStr + " " + acft.GetAuthoPoint().GetId();
            GUI.Label(new Rect(0f, stripSize.y * i, 100f, stripSize.y), strip, windowFPS_guistyle.GetStyle("Arrivals"));
            i++;
        }
        
        GUI.EndScrollView();

        GUI.EndGroup();


        GUI.BeginGroup(new Rect(0.0f, fpsTitleSize.y + arr_dep_panelHeight, windowFPSWidth, arr_dep_panelHeight));

        GUI.Label(new Rect(0.0f, 0.0f, windowFPSWidth, departures_titleSize.y), departures_title, windowFPS_guistyle.GetStyle("TitleLabel"));
        arrivals_scrollPos = GUI.BeginScrollView(
                                    new Rect(0.0f, departures_titleSize.y, windowFPSWidth, arr_dep_panelHeight - departures_titleSize.y),
                                    arrivals_scrollPos,
                                    new Rect(0.0f, 0.0f, windowFPSWidth - 20.0f, CreateObjects.aircraftList.Count * stripSize.y)
                                );
       
        i = 0;
        foreach (Aircraft acft in departureAcfts)
        {
            if (acft.GetAuthoAltitude() > CreateObjects.airport.GetTransAltitude())
            {
                authFL = (ushort) Mathf.Ceil(acft.GetAuthoAltitude() / 100);
                authFLStr = (authFL < 100 ? "0" + authFL.ToString() : authFL.ToString());
                authFLStr = (authFL < 10 ? authFLStr + "0" : authFLStr);
                authFLStr = "FL" + authFLStr;
            }
            else
            {
                authFLStr = "A" + acft.GetAuthoAltitude().ToString();
            }

            strip = acft.GetCallsignCode() + acft.GetFlightNumber() + " " + acft.GetAircraftModelCode() + "\n" +
                                authFLStr + " " + acft.GetAuthoPoint().GetId();
            GUI.Label(new Rect(0f, stripSize.y * i, 100f, stripSize.y), strip, windowFPS_guistyle.GetStyle("Departures"));
            i++;
        }
        
        GUI.EndScrollView();

        GUI.EndGroup();
    }


    public static float GetWindowFPSWidth() { return windowFPSWidth; }
    public static Rect GetWindowFPS() { return windowFPS; }

}// end-class
