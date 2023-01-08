using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class General : MonoBehaviour
{
    /*
     * The following items can be modified from Unity Editor
     */
    [Header("Airport Preferences")]
    public ushort ILS_range = 15;  // nm

    [Header("Radar Preferences")]
    public float _radarPeriod = 5.0f;
    public static float radarPeriod;

    [Header("Aircraft Preferences")]
    public float _aircraftDataPeriod = 5.0f;
    public static float aircraftDataPeriod;

    public static int windowId = 0;

    // Start is called as soon as script is loaded
    private void Awake()
    {
        radarPeriod = _radarPeriod;
        aircraftDataPeriod= _aircraftDataPeriod;
    }


    // Start is called before the first frame update
    void Start()
    {
        DrawGUI.Init();
        DrawRadarScreen.Init();

        CreateObjects.Init();
        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        /*MngCamera.Init();
        //	LoadData.Init();
        CreateObjects.Init();
        Measurement.Init();

        MngScreen.Init();
        CreateObjects.PrepareDraw();

        DrawGUI.StartDraw();
        DrawRadarScreen.StartDraw();
        DrawATCPopup.Init();*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int AssignWindowId() 
    { 
        return windowId++; 
    }
}
