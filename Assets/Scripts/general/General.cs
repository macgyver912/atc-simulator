using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class General : MonoBehaviour
{
    /*
     * The following items can be modified from Unity Editor
     */
    

    public static int windowId = 0;

    // Start is called as soon as script is loaded
    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        DrawGUI.Init();
        DrawRadarScreen.Init();


        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        MngCamera.Init();
        //	LoadData.Init();
        CreateObjects.Init();
        Measurement.Init();

        MngScreen.Init();
        CreateObjects.PrepareDraw();

        DrawGUI.StartDraw();
        DrawRadarScreen.StartDraw();
        //DrawATCPopup.Init();

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
