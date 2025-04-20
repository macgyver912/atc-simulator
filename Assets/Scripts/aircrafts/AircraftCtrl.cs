
//private static AircraftCtrl instance;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class AircraftCtrl : MonoBehaviour
{ 

    public Aircraft aircraft;

    public ushort nTrails = 5;
    //var trailsList : List.<GameObject>;

    //var target : Transform;
    //var strength = 0.5;
    //var str : float;
    //var targetRotation : Quaternion;

    // Speed control


    // Turn control
    enum TurningDirection { Left, Right };
    private static ushort turnAngle;
    private static ushort turnRate;

    private IEnumerator coroutine_HDG_Left;
    private IEnumerator coroutine_HDG_Right;
    private IEnumerator coroutine_SPD_Increase;
    private IEnumerator coroutine_SPD_Decrease;
    private IEnumerator coroutine_ALT_Climb;
    private IEnumerator coroutine_ALT_Descend;


    //function Awake(){
    //	instance = this;
    //}

    void Start()
    {
        SetTrails();
        InvokeRepeating("UpdateAcftData", 0, Config.aircraftDataPeriod);
    }

    void UpdateAcftData()
    {
        Forward();

        aircraft.GetGO().transform.rotation = Quaternion.Euler(90 + aircraft.GetHeading(), 90, 270);
    }

    void SetTrails()
    {
        GameObject trailsGO = new GameObject(aircraft.GetGO().name + "_Trails");
        //trailsGO.transform.parent = aircraft.GetGO().transform;

        //	trailsList = new List.<GameObject>();
        //	positionsList = new List.<Vector2>();

        for (ushort i = 0; i < nTrails; i++)
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            plane.GetComponent<Renderer>().material.color = DrawRadarScreen.labelLineColor;
            plane.GetComponent<Renderer>().material.shader = Config.object_shader;
            plane.name = aircraft.GetGO().name + "_trail#" + i;
            plane.transform.parent = GameObject.Find(aircraft.GetGO().name + "_Trails").transform;
            plane.transform.position = aircraft.GetGO().transform.position;
        }

        InvokeRepeating("RefreshTrails", 0, Config.radarPeriod * 4f);
    }

    void RefreshTrails()
    {

        GameObject gameObject = GameObject.Find(aircraft.GetGO().name + "_Trails");
        List<Transform> children = new List<Transform>();

        foreach(Transform transform in gameObject.transform)
        {
            children.Add(transform);
        }

        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                children[i].position = aircraft.GetGO().transform.position;
                children[i].rotation = aircraft.GetGO().transform.rotation;
            }
            else
            {
                children[i].position = children[i - 1].position;
                children[i].rotation = children[i - 1].rotation;
            }


            //    	Debug.Log("\t\t"+children[i].name+": " + children[i].position.ToString());
        }

    }


    void Forward()
    {
        // Calculate how many nautical miles has covered in the update period (s = v * t)
        float delta_s = aircraft.GetSpeedGS() * Config.aircraftDataPeriod / 3600f; // in nautical miles (nm)

        // Maximum displacement if heading is x or y
        Vector2 delta_degree = delta_s * Measurement.GetNM_Degree();          // in degrees

        // Displacement factor in axis x and y
        Vector2 hdg_factor;
        hdg_factor.x = Mathf.Sin(aircraft.GetHeading() * Mathf.Deg2Rad);
        hdg_factor.y = Mathf.Cos(aircraft.GetHeading() * Mathf.Deg2Rad);

        // Displacement dependent of heading (maximum displacement * heading factor)
        Vector2 delta_degree_hdg = Vector2.Scale(delta_degree, hdg_factor);

        aircraft.SetPosition(aircraft.GetPosition() + new Vector3(delta_degree_hdg.x, delta_degree_hdg.y, 0f));
        aircraft.SetScreenPosition(MngScreen.RadarScreenPosRelToAirport(aircraft.GetPosition().x, aircraft.GetPosition().y, 0));
        aircraft.GetGO().transform.position = aircraft.GetScreenPosition();
	
        //	RefreshTrails();
    }

    // #### SPEED ####

    public void ChangeSpeed(ushort targetSpeed, bool fast){
        string debugText = string.Empty;

        // Cancel previous commands
        StopCoroutine("IncreaseSpeed");
        StopCoroutine("ReduceSpeed");

        if (targetSpeed > aircraft.GetSpeedGS())
        {
            debugText += "Increasing";
            StartCoroutine(IncreaseSpeed(targetSpeed, fast));
        }
        else
        {
            debugText += "Reducing";
            StartCoroutine(ReduceSpeed(targetSpeed, fast));
        }

        debugText += " speed to " + targetSpeed + " knots" + (fast ? " as soon as possible" : "") +  ", " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());
        Debug.LogWarning(debugText);
    }

    private IEnumerator IncreaseSpeed(ushort targetSpeed, bool fast)
    {
        ushort speedRate = (fast ? aircraft.GetSpeedRateAirMax() : aircraft.GetSpeedRateAirStd());
        float auxSpeed = aircraft.GetSpeedGS() + (speedRate * Config.aircraftDataPeriod) + Random.Range(-2, 2);
        if (auxSpeed >= targetSpeed && aircraft.GetSpeedGS() < targetSpeed)
        {
            // Target speed is reached, maintain target speed
            Debug.Log("Target speed is reached, maintain target speed");
            aircraft.SetSpeedGS((ushort) targetSpeed);
            StopCoroutine("IncreaseSpeed");
        }
        else
        {
            // Target speed is not reached yet, maintain increasing speed
            Debug.Log("Target speed is not reached yet, maintain increasing speed");
            aircraft.SetSpeedGS((ushort) auxSpeed);

            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            StartCoroutine(IncreaseSpeed(targetSpeed, fast));
        }
    }

    private IEnumerator ReduceSpeed(ushort targetSpeed, bool fast)
    {
        ushort speedRate = (fast ? aircraft.GetSpeedRateAirMax() : aircraft.GetSpeedRateAirStd());
        float auxSpeed = aircraft.GetSpeedGS() - (speedRate * Config.aircraftDataPeriod) + Random.Range(-2, 2);
        if (auxSpeed <= targetSpeed && aircraft.GetSpeedGS() > targetSpeed)
        {
            // Target speed is reached, maintain target speed
            Debug.Log("Target speed is reached, maintain target speed");
            aircraft.SetSpeedGS((ushort) targetSpeed);
            StopCoroutine("ReduceSpeed");
        }
        else
        {
            // Target speed is not reached yet, maintain reducing speed
            Debug.Log("Target speed is not reached yet, maintain reducing speed");
            aircraft.SetSpeedGS((ushort) auxSpeed);

            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            StartCoroutine(ReduceSpeed(targetSpeed, fast));
        }
    }

    // #### ALTITUDE ####

    public void ChangeLevel(int targetAltitude, bool fast){
        if (targetAltitude > aircraft.GetAltitude())
            Climb(targetAltitude, fast);
        else
            Descend(targetAltitude, fast);
    }

    private IEnumerator Climb(int targetAltitude, bool fast) 
    {
        ushort vsRate = (fast ? aircraft.GetVSRateMax() : aircraft.GetVSRateStd());
        int auxRate = vsRate + Random.Range(-50, 50);       // feet per minute
        ushort auxRateSec = (ushort)(auxRate * Config.aircraftDataPeriod / 60f);           // feet per second

        aircraft.SetAltitude((ushort) (aircraft.GetAltitude() + auxRateSec));

        if (aircraft.GetAltitude() >= targetAltitude)
        {
            aircraft.SetVS(0);
            aircraft.SetAltitude(targetAltitude);
        }
        else
        {
            aircraft.SetVS((short) auxRate);

            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            Climb(targetAltitude, fast);
        }
    }

    private IEnumerator Descend(int targetAltitude, bool fast)
    {
        ushort vsRate = (fast ? aircraft.GetVSRateMax() : aircraft.GetVSRateStd());
        int auxRate = -vsRate + Random.Range(-50, 50);       // feet per minute
        ushort auxRateSec = (ushort)(auxRate * Config.aircraftDataPeriod / 60f);           // feet per second

        aircraft.SetAltitude((ushort)(aircraft.GetAltitude() + auxRateSec));

        if (aircraft.GetAltitude() <= targetAltitude)
        {
            aircraft.SetVS(0);
            aircraft.SetAltitude(targetAltitude);
        }
        else
        {
            aircraft.SetVS((short) auxRate);

            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            Descend(targetAltitude, fast);
        }
    }

    // #### HEADING ####

    public void Turn(ushort targetHeading, int side){
        string debugText = "Turn ";

        if (side == 0)
        {
            debugText += "left";
            StartCoroutine(TurnLeft(targetHeading));
        }
        else if (side == 2)
        {
            debugText += "right";
            StartCoroutine(TurnRight(targetHeading));
        }
        else
        {
            // Calculates the shortest difference between two given angles.
            // E.g. (350, 090) = 100 -> TurnRight
            // E.g. (150, 090) = -60 -> TurnLeft
            if (Mathf.DeltaAngle(aircraft.GetHeading(), targetHeading) > 0)
            {
                debugText += "right";
                //TurnRight(targetHeading);
                StartCoroutine(TurnRight(targetHeading));
            }
            else
            {
                debugText += "left";
                //TurnLeft(targetHeading);
                StartCoroutine(TurnLeft(targetHeading));
            }
        }
       
        debugText += " to heading " + TextUtils.Text2SpellFormat(string.Format("{0:D3}", targetHeading)) + ", " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());

        //debugText += " to heading " + targetHeading + ", " + aircraft.GetCallsignCode() + aircraft.GetFlightNumber();
        Debug.LogWarning(debugText);
    }


    private IEnumerator TurnLeft(ushort targetHeading)
    {

        ushort prevHdg = aircraft.GetHeading();
        float auxHdg = aircraft.GetHeading() - (aircraft.GetTurnRate() * Config.aircraftDataPeriod);

        if (auxHdg < 0f)
            auxHdg = 360f - auxHdg;

        if (auxHdg <= targetHeading && prevHdg > targetHeading)
        {
            // Target heading is reached, stop turn
            Debug.Log("Target heading is reached, stop turning left");

            aircraft.SetHeading((ushort) targetHeading);
            StopCoroutine("TurnLeft");
        }
        else
        {
            // Target heading is not reached yet, continue turn
            Debug.Log("Target heading is not reached yet, continue turning left");

            aircraft.SetHeading((ushort) auxHdg);
            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            StartCoroutine(TurnLeft(targetHeading));
        }
    }

    private IEnumerator TurnRight(ushort targetHeading)
    {

        ushort prevHdg = aircraft.GetHeading();
        float auxHdg = prevHdg + (aircraft.GetTurnRate() * Config.aircraftDataPeriod);

        if (auxHdg > 360f)
            auxHdg = auxHdg - 360f;

        if (auxHdg >= targetHeading && prevHdg < targetHeading)
        {
            // Target heading is reached, stop turn
            Debug.Log("Target heading is reached, stop turning right");

            aircraft.SetHeading((ushort) targetHeading);
            StopCoroutine("TurnRight");
        }
        else
        {
            // Target heading is not reached yet, continue turn
            Debug.Log("Target heading is not reached yet, continue turning right");

            aircraft.SetHeading((ushort) auxHdg);
            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            StartCoroutine(TurnRight(targetHeading));
        }
    }

    void FlyTo()
    {


        //		Debug.Log("FlyTo");

        //		targetRotation = Quaternion.LookRotation(target.position - this.gameObject.transform.position);
        //		this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, targetRotation, str);
        //		str = Mathf.Min(strength * Time.deltaTime, 1);
        //		this.gameObject.transform.rotation.y = 180;
        //		this.gameObject.transform.rotation.z = 0;

        //		this.gameObject.transform.LookAt(target);
        //		var eulerAngles = this.gameObject.transform.rotation.eulerAngles;
        //		eulerAngles.y = 0;
        //		eulerAngles.z = 0;
        ////		
        //		this.gameObject.transform.rotation = Quaternion.Euler(eulerAngles);

        //	this.gameObject.transform.LookAt(target);
    }


    public Aircraft GetAircraft() { return aircraft; }

}//class