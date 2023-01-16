using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawRadarScreen;

/**
 * Manages and draws graphical icons and others.
 *
 * @module Graphics
 * @main Graphics
 * @class DrawRadarScreen
 * @date December 11th, 2022
 * @author Jaime Valle Alonso
 */
public class DrawRadarScreen : MonoBehaviour
{

    private static DrawRadarScreen instance;
 
    public enum IconsColorEnum
    {
        Gray,
        Black,
        White
    };
    public enum AcftLabelPos
    {
        UpperLeft = 0,
        UpperCenter = 1,
        UpperRight = 2,
        BottomRight = 3,
        BottomCenter = 4,
        BottomLeft = 5
    };
    public enum AcftLabelFontSize
    {
        Small = 7,
        Medium = 8,
        Large = 9
    };

    static AcftLabelPos prefAcftLabelPos;
    Rect acftLabelRect;
    float acftLabelHeight;
    //static ushort acftLabelFontSize;
    static AcftLabelFontSize acftLabelFontSize;

    private static IconsColorEnum _defIconsColor_navaids;
    private static IconsColorEnum _defIconsColor_aircrafts;

    public static Color labelLineColor;
 
    public static Dictionary<string, Texture2D> icons;

    static LineRenderer lineRenderer;

    private static bool showGUI = false;
    bool initGUI = true;

    Vector2 fixGOSize;
    Vector2 vorGOSize;
    Vector2 vorRoseGOSize;
    Vector2 acftGOSize;
    static float maxIconSize;
    static Vector2 defCallsignSize;

    Vector2 labelSize;
    static float rwyWidth;

    private static Color _color_circles;
    private static Color _color_limit_circles;
    private static Color _color_grid;
    private static Color _color_runways;

    private static GUISkin labelStyle_navaids;
    private static GUISkin labelStyle_aircrafts;

    //var lengthOfLineRenderer : int = 2;
    private static Material defaultMaterial;

    static Vector3 airportPosition = new Vector3(-3.560833f, 40.472222f, 2000.0f);
    static float minAltitude = airportPosition.z;

    private static int ringsSeparation;
    private static int gridSeparation = ringsSeparation;
    private static ushort ringsVertexs = 48;
    private static uint twr_app_limit;
    private static uint app_ctr_limit;

    public void Awake()
    {
        instance = this;
    }
    void OnGUI()
    {
        if (initGUI)
        {
            initGUI = false;

            labelStyle_aircrafts.GetStyle("Label").fontSize = (int) acftLabelFontSize;

            defCallsignSize = labelStyle_aircrafts.GetStyle("Label").CalcSize(new GUIContent("AAAXXXX"));
            acftLabelHeight = labelStyle_aircrafts.GetStyle("Label").CalcSize(new GUIContent(CreateObjects.aircraftList[0].GetLabel())).y;
            
            fixGOSize = MngScreen.GetScreenSizeOfGameObject((CreateObjects.fixList[0]).GetGO());
            /*
            VOR auxVor = CreateObjects.vorList.Find(function(_vor: VOR){ return _vor.hasDME == false; });
            if (auxVor != null)
                vorGOSize = MngScreen.GetScreenSizeOfGameObject(auxVor.go);

            var auxVorRose : VOR = CreateObjects.vorList.Find(function(_vor: VOR){ return _vor.hasDME == true; });
            if (auxVorRose != null)
                vorRoseGOSize = MngScreen.GetScreenSizeOfGameObject(auxVorRose.go);

            acftGOSize = MngScreen.GetScreenSizeOfGameObject((CreateObjects.aircraftList[0]).go);
            */
            //		SetAcftLabelPos();

        }// if initGUI

        if (showGUI)
        {
            // ######### Show name of navaids #########

            // FIX
            foreach (FIX fix in CreateObjects.fixList)
            {
                labelSize = labelStyle_navaids.GetStyle("Label").CalcSize(new GUIContent(fix.GetName()));
                Vector2 fixPos = MngScreen.ScreenPosAbsolute(fix.GetScreenPosition());
                // Below GameObject
                GUI.Label(new Rect(fixPos.x - labelSize.x / 2f, fixPos.y + fixGOSize.y / 2f, labelSize.x, labelSize.y), fix.GetName(), labelStyle_navaids.GetStyle("Label"));
            }//for

            // VOR
            foreach (VOR vor in CreateObjects.vorList)
            {
                labelSize = labelStyle_navaids.GetStyle("Label").CalcSize(new GUIContent(vor.GetName()));

                Vector2 vorPos = MngScreen.ScreenPosAbsolute(vor.GetScreenPosition());
                // Below GameObject
                if (vor.HasDME())
                    //				GUI.Label(Rect(vorPos.x-labelSize.x/2, vorPos.y+vorRoseGOSize.y/2, labelSize.x, labelSize.y), vor.id, labelStyle_navaids.GetStyle("Label"));
                    GUI.Label(new Rect(vorPos.x - labelSize.x / 2f, vorPos.y - vorRoseGOSize.y / 2f - labelSize.y, labelSize.x, labelSize.y), vor.GetID(), labelStyle_navaids.GetStyle("Label"));
                else
                    //				GUI.Label(Rect(vorPos.x-labelSize.x/2, vorPos.y+vorGOSize.y/2, labelSize.x, labelSize.y), vor.id, labelStyle_navaids.GetStyle("Label"));
                    GUI.Label(new Rect(vorPos.x - labelSize.x / 2f, vorPos.y - vorGOSize.y / 2f - labelSize.y, labelSize.x, labelSize.y), vor.GetID(), labelStyle_navaids.GetStyle("Label"));
            }

            // ######### Show aircrafts labels #########
            // Aircraft
            foreach (Aircraft acft in CreateObjects.aircraftList)
            {
                SetAcftLabelPos(acft);
                Vector2 acftPos = MngScreen.ScreenPosAbsolute(acft.GetScreenPosition());
                // Below GameObject

                var rect = new Rect(acftLabelRect.x + acftPos.x, acftLabelRect.y + acftPos.y, acftLabelRect.width, acftLabelRect.height);
                acft.SetLabelScreenPos(MngScreen.GetScreenToWorldPoint(new Vector3(rect.x, rect.y, 0)));
                GUI.Label(rect, acft.GetLabel(), labelStyle_aircrafts.GetStyle("Label"));
            }//for

        }// if showGUI
    }

    public static void Init()
    {

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Config.background_color;

        

        _color_circles = Config.color_circles;
        _color_limit_circles = Config.color_limit_circles;
        _color_grid = Config.color_grid;
        _color_runways = Config.color_runways;

        _defIconsColor_navaids = Config.defIconsColor_navaids;
        _defIconsColor_aircrafts = Config.defIconsColor_aircrafts;

        twr_app_limit = (uint)Config.twr_range;
        app_ctr_limit = (uint)Config.app_range;
        ringsSeparation = Config.rings_separation;

        defaultMaterial = new Material(Shader.Find("Diffuse"));

        labelStyle_navaids = Config.labelStyle_navaids;
        labelStyle_aircrafts = Config.labelStyle_aircrafts;

        labelLineColor = labelStyle_aircrafts.GetStyle("Label").normal.textColor;

        

        //	lineRenderer = instance.gameObject.AddComponent(LineRenderer);

        prefAcftLabelPos = AcftLabelPos.BottomLeft;
        string prefLabelPos = PlayerPrefs.GetString("prefAcftLabelPos", null);
        if (prefLabelPos != string.Empty)
        {
            prefAcftLabelPos = (AcftLabelPos) System.Enum.Parse(typeof(AcftLabelPos), prefLabelPos);
        }

        // Set navaids icon color (choose folder for load icons)
        string folderNavaids = _defIconsColor_navaids.ToString();
        string prefs_navaidsColor = PlayerPrefs.GetString("iconsColor_navaids", null);
        if (prefs_navaidsColor != string.Empty)
        {
            folderNavaids = prefs_navaidsColor;
        }

        // Set aircraft icon color (choose folder for load icons)
        string folderAircraft = _defIconsColor_aircrafts.ToString();
        string prefs_aircraftsColor = PlayerPrefs.GetString("iconsColor_aircrafts", null);
        if (prefs_aircraftsColor != string.Empty)
        {
            folderAircraft = prefs_aircraftsColor;
        }

        acftLabelFontSize = AcftLabelFontSize.Medium;
        string prefs_acftLabelFontSize = PlayerPrefs.GetString("acftLabelFontSize", null);
        if (prefs_acftLabelFontSize != string.Empty)
        {
            acftLabelFontSize = (AcftLabelFontSize) System.Enum.Parse(typeof(AcftLabelFontSize), prefs_acftLabelFontSize);
        }

        icons = new Dictionary<string, Texture2D>();

        icons.Add("aircraft", Resources.Load("icons/aeronautical/" + folderAircraft + "/Aircraft") as Texture2D);
        icons.Add("aerodrome_civil", Resources.Load("icons/aeronautical/" + folderNavaids + "/Aerodrome_Civil") as Texture2D);
        icons.Add("aerodrome_civil_(no facilities)", Resources.Load("icons/aeronautical/" + folderNavaids + "/Aerodrome_Civil_(no facilities)") as Texture2D);
        icons.Add("aerodrome_government_civil", Resources.Load("icons/aeronautical/" + folderNavaids + "/Aerodrome_Government_Civil") as Texture2D);
        icons.Add("aerodrome_government", Resources.Load("icons/aeronautical/" + folderNavaids + "/Aerodrome_Government") as Texture2D);
        icons.Add("vor", Resources.Load("icons/aeronautical/" + folderNavaids + "/VOR") as Texture2D);
        icons.Add("vor_dme", Resources.Load("icons/aeronautical/" + folderNavaids + "/VOR_DME") as Texture2D);
        icons.Add("vor_dme_rose", Resources.Load("icons/aeronautical/" + folderNavaids + "/VOR_DME_ROSE") as Texture2D);
        icons.Add("dme", Resources.Load("icons/aeronautical/" + folderNavaids + "/DME") as Texture2D);
        icons.Add("fix_empty", Resources.Load("icons/aeronautical/" + folderNavaids + "/FIX_EMPTY") as Texture2D);
        icons.Add("fix_filled", Resources.Load("icons/aeronautical/" + folderNavaids + "/FIX_FILLED") as Texture2D);
    }

    public static void StartDraw()
    {
        rwyWidth = 0.7f * MngScreen.GetPixelRatio();
        //	Debug.Log("rwyWidth: " + rwyWidth);
        DrawAirport();
        showGUI = true;
        instance.InvokeRepeating("UpdateRadarScreen", 0, Config.radarPeriod);
    }

    void UpdateRadarScreen()
    {
        // ######### Show aircrafts labels #########

        
        ushort fl;
        string flStr;
        ushort speed;
        ushort autAlt;
        string autAltStr;
        

        // Aircraft
        foreach (Aircraft acft in CreateObjects.aircraftList)
        {
            string vsLabel = "=";     // label for vertical speed
            if (acft.GetVerticalSpeed() != 0)
                vsLabel = (acft.GetVerticalSpeed() > 0 ? "+" : "-");

            fl = (ushort) Mathf.Ceil(acft.GetAltitude() / 100);
            flStr = (fl < 100 ? "0" + fl.ToString() : fl.ToString());
            flStr = (fl < 10 ? flStr + "0" : flStr);
            autAlt = (ushort) Mathf.Ceil(acft.GetAuthoAltitude() / 100);
            autAltStr = (autAlt < 100 ? "0" + autAlt.ToString() : autAlt.ToString());
            //		speed = Mathf.Ceil(acft.speedGS / 10f)*10;
            speed = acft.GetSpeedGS();

            acft.SetLabel(acft.GetCallsignCode() + acft.GetFlightNumber() + " " + (acft.GetCategory() == Aircraft.Category.Heavy ? "H" : "") + "\n" +
                        flStr + vsLabel + " " + autAltStr + "\n" +
                        speed + " " + acft.GetAuthoPoint());



            DrawLabelLine(acft);
        }


    }


    static void UpdateAcftAuthLabel(Aircraft acft)
    {
        // ######### Show aircrafts labels #########

        ushort fl;
        string flStr;
        ushort speed;
        ushort autAlt;
        string autAltStr;
        
        // Aircraft
        string vsLabel = "=";     // label for vertical speed
        if (acft.GetVerticalSpeed() != 0)
            vsLabel = (acft.GetVerticalSpeed() > 0 ? "+" : "-");

        fl = (ushort) Mathf.Ceil(acft.GetAltitude() / 100.0f);
        flStr = (fl < 100 ? "0" + fl.ToString() : fl.ToString());
        flStr = (fl < 10 ? flStr + "0" : flStr);
        autAlt = (ushort) Mathf.Ceil(acft.GetAuthoAltitude() / 100.0f);
        autAltStr = (autAlt < 100 ? "0" + autAlt.ToString() : autAlt.ToString());
        //		speed = Mathf.Ceil(acft.speedGS / 10f)*10;
        speed = acft.GetSpeedGS();

        acft.SetLabel(acft.GetCallsignCode() + acft.GetFlightNumber() + " " + (acft.GetCategory() == Aircraft.Category.Heavy ? "H" : "") + "\n" +
                    flStr + vsLabel + " " + autAltStr + "\n" +
                    speed + " " + acft.GetAuthoPoint()
                    );

    }
    
    static void DrawLabelLine(Aircraft acft)
    {
        LineRenderer lineRenderer;

        if (acft.GetGO().GetComponentsInChildren<LineRenderer>().Length == 0)
        {
            GameObject lineGO = new GameObject("LineLabel_" + acft.GetCallsignCode() + acft.GetFlightNumber());
            LineRenderer lr = lineGO.AddComponent<LineRenderer>() as LineRenderer;
            lineGO.transform.parent = acft.GetGO().transform;

            LineRenderer auxLineRenderer = lineGO.GetComponent<LineRenderer>() as LineRenderer;
            Color c1 = labelLineColor;
            auxLineRenderer.material = defaultMaterial;
            auxLineRenderer.material.color = c1;
            auxLineRenderer.SetColors(c1, c1);
            auxLineRenderer.SetWidth(0.5f, 0.5f);
        }

        lineRenderer = acft.GetGO().GetComponentsInChildren<LineRenderer>()[0];

        lineRenderer.SetPosition(0, MngScreen.RadarScreenPosRelToAirport(acft.GetPosition()));
        //lineRenderer.SetPosition(1, acft.GetLabelScreenPos() + acft.GetLabelLineOffset());
        lineRenderer.SetPosition(1, new Vector3(
            acft.GetLabelScreenPos().x + acft.GetLabelLineOffset().x,
            acft.GetLabelScreenPos().y + acft.GetLabelLineOffset().y,
            acft.GetLabelScreenPos().z
            ));

    }
    
    static void DrawAirport()
    {
        //	for(var rwy in CreateObjects.airport.runways){
        //		DrawRunway(rwy);
        //	}

        Runway[] auxRwy = new Runway[2];

        // i+2 because draw a physical runway is implicitily draw two runways
        for (ushort i = 0; i < CreateObjects.airport.GetRunways().Length; i += 2)
        {
            auxRwy[0] = CreateObjects.airport.GetRunways()[i];
            auxRwy[1] = CreateObjects.airport.GetRunways()[i+1];
            DrawRunway(auxRwy);
        }

        DrawAppLimits();
    }

    static void DrawRunway(Runway[] rwys)
    {
        GameObject rwyGO = new GameObject("Runway_" + rwys[0].GetID() + "/" + rwys[1].GetID());
        LineRenderer lr = rwyGO.AddComponent<LineRenderer>() as LineRenderer;
        rwyGO.transform.parent = CreateObjects.airport.GetGO().transform;

        LineRenderer lineRenderer = rwyGO.GetComponent<LineRenderer>() as LineRenderer;

        //	Debug.Log("Runway 0: " + rwys[0].GetThrLon() + " - " + rwys[0].GetThrLat());
        //	Debug.Log("Runway 1: " + rwys[1].GetThrLon() + " - " + rwys[1].GetThrLat());
        Color c1 = _color_runways;
        lineRenderer.material = defaultMaterial;
        lineRenderer.material.color = c1;
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(rwyWidth, rwyWidth);

        //	lineRenderer.SetPosition(0, MngScreen.ScreenPosRelToAirport(rwys[0].GetThrLon(), rwys[0].GetThrLat(), CreateObjects.airport.GetPosition().z/100));
        //	lineRenderer.SetPosition(1, MngScreen.ScreenPosRelToAirport(rwys[1].GetThrLon(), rwys[1].GetThrLat(), CreateObjects.airport.GetPosition().z/100));
        lineRenderer.SetPosition(0, MngScreen.RadarScreenPosRelToAirport(rwys[0].GetThrLon(), rwys[0].GetThrLat(), CreateObjects.airport.GetPosition().z / 100f));
        lineRenderer.SetPosition(1, MngScreen.RadarScreenPosRelToAirport(rwys[1].GetThrLon(), rwys[1].GetThrLat(), CreateObjects.airport.GetPosition().z / 100f));
    }

    public static void DrawCircles()
    {
        // Check if GameObject exists (if it has been created before)
        GameObject go = GameObject.Find("Rings");
        if (go == null)
        {
            GameObject circlesGO = new GameObject("Rings");
            circlesGO.transform.position = CreateObjects.airport.GetGO().transform.position;

            //		var deltaDistance = ringsSeparation * Measurement.GetNM_Degree().y * MngScreen.ratio.y;
            float deltaDistance = ringsSeparation * Measurement.GetNM_Degree().y * MngScreen.GetRatio().y / MngScreen.GetPixelRatio();
            float distance = deltaDistance;

            Debug.Log("Measurement.nm2degree.x: " + Measurement.GetNM_Degree().y);
            Debug.Log("MngScreen.ratio.x: " + MngScreen.GetRatio().y);
            Debug.Log("distance: " + distance);

            float stopCondition = (Screen.width > Screen.height ? Screen.width / 2f : Screen.height / 2f);

            while (distance < stopCondition)
            {
                DrawCircle(distance, ringsVertexs);
                distance += deltaDistance;
            }
        }
        else
        {
            //		go.SetActive(true);
        }
    }

    public static void HideCircles()
    {
        GameObject go = GameObject.Find("Rings");
        if (go != null)
        {
            //		go.SetActive(false);
            Destroy(go);
        }
    }

    static void DrawCircle(float radius, ushort vertexCount)
    {

        GameObject newGO = new GameObject("Ring");
        newGO.transform.parent = GameObject.Find("Rings").gameObject.transform;

        LineRenderer lineRenderer = newGO.AddComponent<LineRenderer>() as LineRenderer;
        Color c1 = _color_circles;
        lineRenderer.material = new Material(defaultMaterial);
        lineRenderer.material.color = c1;
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(0.5f, 0.5f);
        lineRenderer.SetVertexCount(vertexCount + 1);

        float deltaTheta = (2.0f * Mathf.PI) / vertexCount;
        float theta = 0;

        Vector2 offset;
        offset.x = MngScreen.GetRadarScreenOffset().x + (Screen.width - MngScreen.GetRadarScreenOffset().x) / 2f;
        offset.y = MngScreen.GetRadarScreenOffset().y + Screen.height / 2f;

        for (int i = 0; i < vertexCount + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos;
            pos.x = x + offset.x;
            pos.y = y + offset.y;
            pos.z = 0;

            pos = MngScreen.GetScreenToWorldPoint(pos);

            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

    }

    static void DrawAppLimits()
    {
        // Check if GameObject exists (if it has been created before)
        GameObject go = GameObject.Find("AppLimits");
        if (go == null)
        {
            GameObject circlesGO = new GameObject("AppLimits");
            circlesGO.transform.position = CreateObjects.airport.GetGO().transform.position;

            float twr_app_limit_dst = twr_app_limit * Measurement.GetNM_Degree().y * MngScreen.GetRatio().y / MngScreen.GetPixelRatio();
            float app_ctr_limit_dst = app_ctr_limit * Measurement.GetNM_Degree().y * MngScreen.GetRatio().y / MngScreen.GetPixelRatio();

            DrawAppLimit(twr_app_limit_dst, ringsVertexs);
            DrawAppLimit(app_ctr_limit_dst, ringsVertexs);
        }
        else
        {
            //		go.SetActive(true);
        }
    }

    static void HideAppLimits()
    {
        var go = GameObject.Find("AppLimits");
        if (go != null)
        {
            //		go.SetActive(false);
            Destroy(go);
        }
    }

    static void DrawAppLimit(float radius, ushort vertexCount)
    {

        GameObject newGO = new GameObject("Limit");
        newGO.transform.parent = GameObject.Find("AppLimits").gameObject.transform;

        LineRenderer lineRenderer = newGO.AddComponent<LineRenderer>() as LineRenderer;
        Color c1 = _color_limit_circles;
        lineRenderer.material = new Material(defaultMaterial);
        lineRenderer.material.color = c1;
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(0.5f, 0.5f);
        lineRenderer.SetVertexCount(vertexCount + 1);

        float deltaTheta = (2.0f * Mathf.PI) / vertexCount;
        float theta = 0;

        Vector2 offset;
        offset.x = MngScreen.GetRadarScreenOffset().x + (Screen.width - MngScreen.GetRadarScreenOffset().x) / 2f;
        offset.y = MngScreen.GetRadarScreenOffset().y + Screen.height / 2f;

        for (int i = 0; i < vertexCount + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos;
            pos.x = x + offset.x;
            pos.y = y + offset.y;
            pos.z = 0;

            pos = MngScreen.GetScreenToWorldPoint(pos);

            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

    }


    public static void DrawGrid()
    {
        // Check if GameObject exists (if it has been created before)
        GameObject go = GameObject.Find("Grid");
        if (go == null)
        {
            GameObject circlesGO = new GameObject("Grid");
            circlesGO.transform.position = CreateObjects.airport.GetGO().transform.position;

            //		var deltaDistance = gridSeparation * Measurement.nm2degree.y * MngScreen.ratio.y;
            float deltaDistance = gridSeparation * Measurement.GetNM_Degree().y * MngScreen.GetRatio().y / MngScreen.GetPixelRatio();
            float distance = 0;

            float stopCondition = (Screen.width > Screen.height ? Screen.width / 2f : Screen.height / 2f);

            while (distance < stopCondition)
            {
                // 'i' represents 4 screen borders (top, right, bottom, left)
                for (ushort i = 0; i < 4; i++)
                {
                    DrawGridLine(distance, i);
                    DrawGridLine(-distance, i);
                }

                distance += deltaDistance;
            }

        }
        else
        {
            //		go.SetActive(true);
        }
    }

    public static void HideGrid()
    {
        GameObject go = GameObject.Find("Grid");
        if (go != null)
        {
            //		go.SetActive(false);
            Destroy(go);
        }
    }

    static void DrawGridLine(float posDist, ushort border)
    {

        GameObject newGO = new GameObject("GridLine");
        newGO.transform.parent = GameObject.Find("Grid").gameObject.transform;

        LineRenderer lineRenderer = newGO.AddComponent<LineRenderer>() as LineRenderer;
        Color c1 = _color_grid;
        lineRenderer.material = new Material(defaultMaterial);
        lineRenderer.material.color = c1;
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(0.5f, 0.5f);
        lineRenderer.SetVertexCount(2);

        Vector2 offset;
        offset.x = MngScreen.GetRadarScreenOffset().x + (Screen.width - MngScreen.GetRadarScreenOffset().x) / 2f;
        offset.y = MngScreen.GetRadarScreenOffset().y + Screen.height / 2f;

        Vector3 pos;
        pos.z = 0;

        // 'border' represents 4 screen borders (top, right, bottom, left)    
        switch (border)
        {
            case 0: // top
                pos.x = posDist + offset.x;
                pos.y = -Screen.height / 2f + offset.y;
                pos = MngScreen.GetScreenToWorldPoint(pos);
                lineRenderer.SetPosition(0, pos);
                lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y - 10.0f, pos.z));
                break;
            case 1: // right
                pos.x = (Screen.width - MngScreen.GetRadarScreenOffset().x) / 2f + offset.x;
                pos.y = posDist + offset.y;
                pos = MngScreen.GetScreenToWorldPoint(pos);
                lineRenderer.SetPosition(0, pos);
                lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y - 10.0f, pos.z));
                break;
            case 2: // bottom
                pos.x = posDist + offset.x;
                pos.y = Screen.height / 2f + offset.y;
                pos = MngScreen.GetScreenToWorldPoint(pos);
                lineRenderer.SetPosition(0, pos);
                lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y + 10.0f, pos.z));
                break;
            case 3: // left
                pos.x = -(Screen.width - MngScreen.GetRadarScreenOffset().x) / 2 + offset.x;
                pos.y = posDist + offset.y;
                pos = MngScreen.GetScreenToWorldPoint(pos);
                lineRenderer.SetPosition(0, pos);
                lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y + 10.0f, pos.z));
                break;
        }

    }

    void SetAcftLabelPos(Aircraft acft)
    {
        AcftLabelPos pos = acft.GetLabelPos();
        // label position
        if (pos == AcftLabelPos.UpperLeft)
        {
            acftLabelRect = new Rect(-defCallsignSize.x,
                                -acftGOSize.y / 3f - acftLabelHeight,
                                 defCallsignSize.x,
                                 defCallsignSize.y);
            acft.SetLabelLineOffset(new Vector2(defCallsignSize.x / 4f, -labelSize.y));
        }
        else if (pos == AcftLabelPos.UpperCenter)
        {
            acftLabelRect = new Rect(-defCallsignSize.x / 2f,
                            -acftGOSize.y / 2f - acftLabelHeight,
                             defCallsignSize.x,
                             defCallsignSize.y);
        }
        else if (pos == AcftLabelPos.UpperRight)
        {
            acftLabelRect = new Rect(defCallsignSize.x / 4f,
                            -acftGOSize.y / 3f - acftLabelHeight,
                             defCallsignSize.x,
                             defCallsignSize.y);
        }
        else if (pos == AcftLabelPos.BottomLeft)
        {
            acftLabelRect = new Rect(-defCallsignSize.x,
                            acftGOSize.y + defCallsignSize.y / 2f,
                             defCallsignSize.x,
                             defCallsignSize.y);
            acft.SetLabelLineOffset(new Vector2(defCallsignSize.x / 4f, defCallsignSize.y * 0.5f));
        }
        else if (pos == AcftLabelPos.BottomCenter)
        {
            acftLabelRect = new Rect(-defCallsignSize.x / 2f,
                            acftGOSize.y + defCallsignSize.y,
                             defCallsignSize.x,
                             defCallsignSize.y);
            acft.SetLabelLineOffset(new Vector2(defCallsignSize.x / 4f, defCallsignSize.y * 0.5f));
        }
        else if (pos == AcftLabelPos.BottomRight)
        {
            acftLabelRect = new Rect(defCallsignSize.x / 4f,
                        acftGOSize.y + defCallsignSize.y / 2f,
                         defCallsignSize.x,
                         defCallsignSize.y);
            acft.SetLabelLineOffset(new Vector2(defCallsignSize.x / 4f, defCallsignSize.y * 0.5f));
        }

        DrawLabelLine(acft);

    }
    
    public static AcftLabelPos ChangeAcftLabelPos(AcftLabelPos pos)
    {
        AcftLabelPos newPos;
        if(pos < AcftLabelPos.BottomLeft)
        {
		    newPos = pos+1;
	    }
        else
        {
            newPos = 0;
        }

        return newPos;
    }




}// end-class