using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGUI : MonoBehaviour
{
    private static bool showGUI = false;
    private static bool initGUI;

    private static int id;

    // Window for Flight Progress Strips
    public GUISkin windowFPS_guistyle;

    //static string windowFPSDefaultText;
    static float windowFPSWidth;
    static Rect windowFPS;
    string windowFPSTitle = "Flight Progress Strips";
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


    float distanceAssistantValue = -1.0f;
    bool existsDistRings = false;
    bool existsDistGrid = false;

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

            arr_dep_panelHeight = (Screen.height - fpsTitleSize.y) / 2;

            stripSize = windowFPS_guistyle.GetStyle("Arrivals").CalcSize(new GUIContent("AAAXXXX AXXX\nFLXXX AAAAA"));
        }

        if (showGUI)
        {
            // Register the window. Notice the 3rd parameter 
            windowFPS = GUI.Window(id, windowFPS, DoWindowFPS, windowFPSTitle, windowFPS_guistyle.GetStyle("Window"));

            //		if(GUI.Button(Rect(Screen.width/2, 5, 50, 20), "Circles")){
            //			DrawRadarScreen.DrawCircles();
            //		}

            float aux = distanceAssistantValue;
            distanceAssistantValue = GUI.HorizontalSlider(new Rect(Screen.width / 2.0f, 5.0f, 30.0f, 10.0f),
                        Mathf.Round(distanceAssistantValue), -1.0f, 1.0f);

            if (distanceAssistantValue != aux)
            {

                switch (distanceAssistantValue)
                {
                    case -1:
                        existsDistGrid = false;
                        existsDistRings = false;
                        DrawRadarScreen.HideGrid();
                        DrawRadarScreen.HideCircles();
                        break;
                    case 0:
                        existsDistGrid = true;
                        existsDistRings = false;
                        DrawRadarScreen.DrawGrid();
                        DrawRadarScreen.HideCircles();
                        break;
                    case 1:
                        existsDistGrid = true;
                        existsDistRings = true;
                        //DrawRadarScreen.DrawGrid();
                        DrawRadarScreen.DrawCircles();
                        break;
                }// switch
            }// if

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
                                    new Rect(0, 0, windowFPSWidth - 20, CreateObjects.aircraftList.Count * stripSize.y)
                                );

        ushort authFL;
        string authFLStr;
        string strip;


        List<Aircraft> arrivalAcfts = new List<Aircraft>();
        List<Aircraft> departureAcfts = new List<Aircraft>();

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
                                authFLStr + " " + acft.GetAuthoPoint();
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
                                authFLStr + " " + acft.GetAuthoPoint();
            GUI.Label(new Rect(0f, stripSize.y * i, 100f, stripSize.y), strip, windowFPS_guistyle.GetStyle("Departures"));
            i++;
        }
        
        GUI.EndScrollView();

        GUI.EndGroup();
    }


    public static float GetWindowFPSWidth() { return windowFPSWidth; }
    public static Rect GetWindowFPS() { return windowFPS; }

}// end-class
