
//private static AircraftCtrl instance;

using System;
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

    private Coroutine coroutine_HDG_Left;
    private Coroutine coroutine_HDG_Right;
    private Coroutine coroutine_SPD_Increase;
    private Coroutine coroutine_SPD_Decrease;
    private Coroutine coroutine_ALT_Climb;
    private Coroutine coroutine_ALT_Descend;
    private Coroutine coroutine_fly_to;

    private bool is_changing_hdg;
    private bool is_changing_spd;
    private bool is_changing_alt;
    private bool is_flying_to;


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
        CheckNextPoint();

        aircraft.GetGO().transform.rotation = Quaternion.Euler(90 + aircraft.GetHeading(), 90, 270);
    }

    void SetTrails()
    {
        GameObject trailsGO = new GameObject(aircraft.GetGO().name + "_Trails");
        //trailsGO.transform.parent = aircraft.GetGO().transform;

        // Locate GameObject inside "Trails" GameObject
        GameObject parentGO = GameObject.Find("Trails");
        if (parentGO == null)
        {
            parentGO = new GameObject("Trails");
        }
        trailsGO.transform.parent = parentGO.transform;

        //	trailsList = new List.<GameObject>();
        //	positionsList = new List.<Vector2>();

        for (ushort i = 0; i < nTrails; i++)
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
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
        if (is_changing_spd)
        {
            if (coroutine_SPD_Decrease != null)
            {
                StopCoroutine(coroutine_SPD_Decrease);
                coroutine_SPD_Decrease = null;
            }
            if (coroutine_SPD_Increase != null)
            {
                StopCoroutine(coroutine_SPD_Increase);
                coroutine_SPD_Increase = null;
            }
        }

        if (targetSpeed > aircraft.GetSpeedGS())
        {
            debugText += "Increasing";
            coroutine_SPD_Increase = StartCoroutine(IncreaseSpeed(targetSpeed, fast));
        }
        else
        {
            debugText += "Reducing";
            coroutine_SPD_Decrease = StartCoroutine(ReduceSpeed(targetSpeed, fast));
        }

        debugText += " speed to " + targetSpeed + " knots" + (fast ? " as soon as possible" : "") +  ", " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());
        //Debug.LogWarning(debugText);
        MngDialogs.SetText(debugText, 1);
    }

    private IEnumerator IncreaseSpeed(ushort targetSpeed, bool fast)
    {
        ushort speedRate = (fast ? aircraft.GetSpeedRateAirMax() : aircraft.GetSpeedRateAirStd());
        float auxSpeed = aircraft.GetSpeedGS() + (speedRate * Config.aircraftDataPeriod) + UnityEngine.Random.Range(-2, 2);
        if (auxSpeed >= targetSpeed && aircraft.GetSpeedGS() < targetSpeed)
        {
            // Target speed is reached, maintain target speed
            //Debug.Log("Target speed is reached, maintain target speed");
            is_changing_spd = false;

            aircraft.SetSpeedGS((ushort) targetSpeed);
            if (coroutine_SPD_Increase != null)
            {
                StopCoroutine(coroutine_SPD_Increase);
                coroutine_SPD_Increase = null;
            }
        }
        else
        {
            // Target speed is not reached yet, continue increasing speed
            //Debug.Log("Target speed is not reached yet, continue increasing speed");
            is_changing_spd = true;
            
            aircraft.SetSpeedGS((ushort) auxSpeed);
            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            coroutine_SPD_Increase = StartCoroutine(IncreaseSpeed(targetSpeed, fast));
        }
    }

    private IEnumerator ReduceSpeed(ushort targetSpeed, bool fast)
    {
        ushort speedRate = (fast ? aircraft.GetSpeedRateAirMax() : aircraft.GetSpeedRateAirStd());
        float auxSpeed = aircraft.GetSpeedGS() - (speedRate * Config.aircraftDataPeriod) + UnityEngine.Random.Range(-2, 2);
        if (auxSpeed <= targetSpeed && aircraft.GetSpeedGS() > targetSpeed)
        {
            // Target speed is reached, maintain target speed
            //Debug.Log("Target speed is reached, maintain target speed");
            is_changing_spd = false;

            aircraft.SetSpeedGS((ushort) targetSpeed);
            if (coroutine_SPD_Decrease != null)
            {
                StopCoroutine(coroutine_SPD_Decrease);
                coroutine_SPD_Decrease = null;
            }
        }
        else
        {
            // Target speed is not reached yet, continue reducing speed
            //Debug.Log("Target speed is not reached yet, continue reducing speed");
            is_changing_spd = true;

            aircraft.SetSpeedGS((ushort) auxSpeed);
            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            coroutine_SPD_Decrease = StartCoroutine(ReduceSpeed(targetSpeed, fast));
        }
    }

    // #### ALTITUDE ####

    public void ChangeLevel(int targetAltitude, bool fast)
    {
        string debugText = string.Empty;

        if (is_changing_alt)
        {
            // Cancel previous commands
            if (coroutine_ALT_Climb != null)
            {
                StopCoroutine(coroutine_ALT_Climb);
                coroutine_ALT_Climb = null;
            }
            if (coroutine_ALT_Descend != null)
            {
                StopCoroutine(coroutine_ALT_Descend);
                coroutine_ALT_Descend = null;
            }
        }

        if (targetAltitude > aircraft.GetAltitude())
        {
            debugText += "Climb";
            coroutine_ALT_Climb = StartCoroutine(Climb(targetAltitude, fast));
        }
        else
        {
            debugText += "Descend";
            coroutine_ALT_Descend = StartCoroutine(Descend(targetAltitude, fast));
        }
        
        debugText += (fast ? " as soon as possible" : "") + " to " 
            + (targetAltitude < CreateObjects.airport.GetTransAltitude() ? targetAltitude.ToString() + " feet" : "flight level " + TextUtils.Text2SpellFormat((targetAltitude / 100).ToString())) 
            + ", " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());
        //Debug.LogWarning(debugText);
        MngDialogs.SetText(debugText, 1);
    }

    private IEnumerator Climb(int targetAltitude, bool fast) 
    {
        ushort vsRate = (fast ? aircraft.GetVSRateMax() : aircraft.GetVSRateStd());
        int auxRate = vsRate + UnityEngine.Random.Range(-50, 50);       // feet per minute
        ushort auxRateSec = (ushort)(auxRate * Config.aircraftDataPeriod / 60f);           // feet per second

        aircraft.SetAltitude((ushort) (aircraft.GetAltitude() + auxRateSec));

        if (aircraft.GetAltitude() >= targetAltitude)
        {
            // Target altitude or flight level is reached, maintain it
            //Debug.Log("Target altitude or flight level is reached, maintain it");
            is_changing_alt = false;

            aircraft.SetVS(0);
            aircraft.SetAltitude(targetAltitude);
            if (coroutine_ALT_Climb != null)
            {
                StopCoroutine(coroutine_ALT_Climb);
                coroutine_ALT_Climb = null;
            }
        }
        else
        {
            // Target altitude or flight level is not reached yet, maintain climbing
            //Debug.Log("Target altitude or flight level is not reached yet, maintain climbing");
            is_changing_alt = true;

            aircraft.SetVS((short) auxRate);
            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            coroutine_ALT_Climb = StartCoroutine(Climb(targetAltitude, fast));
        }
    }

    private IEnumerator Descend(int targetAltitude, bool fast)
    {
        ushort vsRate = (fast ? aircraft.GetVSRateMax() : aircraft.GetVSRateStd());
        int auxRate = -vsRate + UnityEngine.Random.Range(-50, 50);       // feet per minute
        ushort auxRateSec = (ushort)(auxRate * Config.aircraftDataPeriod / 60f);           // feet per second

        aircraft.SetAltitude((ushort)(aircraft.GetAltitude() + auxRateSec));

        if (aircraft.GetAltitude() <= targetAltitude)
        {
            // Target altitude or flight level is reached, maintain it
            //Debug.Log("Target altitude or flight level is reached, maintain it");
            is_changing_alt = false;

            aircraft.SetVS(0);
            aircraft.SetAltitude(targetAltitude);
            if (coroutine_ALT_Descend != null)
            {
                StopCoroutine(coroutine_ALT_Descend);
                coroutine_ALT_Descend = null;
            }
        }
        else
        {
            // Target altitude or flight level is not reached yet, maintain descending
            //Debug.Log("Target altitude or flight level is not reached yet, maintain descending");
            is_changing_alt = true;

            aircraft.SetVS((short) auxRate);
            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            coroutine_ALT_Descend = StartCoroutine(Descend(targetAltitude, fast));
        }
    }

    // #### HEADING ####

    public void Turn(ushort targetHeading, int side){
        string debugText = "";

        if (aircraft.GetAuthoPoint() == null)
        {
            is_flying_to = false;
        }
        // Cancel previous commands
        //if (is_changing_hdg || is_flying_to)
        //{
            if (coroutine_HDG_Left != null)
            {
                StopCoroutine(coroutine_HDG_Left);
                coroutine_HDG_Left = null;
            }
            if (coroutine_HDG_Right != null)
            {
                StopCoroutine(coroutine_HDG_Right);
                coroutine_HDG_Right = null;
            }
        //}
        
        if (side == 0)
        {
            debugText += "Turn left";
            coroutine_HDG_Left = StartCoroutine(TurnLeft(targetHeading));
        }
        else if (side == 2)
        {
            debugText += "Turn right";
            coroutine_HDG_Right = StartCoroutine(TurnRight(targetHeading));
        }
        else
        {
            // Calculates the shortest difference between two given angles.
            // E.g. (350, 090) = 100 -> TurnRight
            // E.g. (150, 090) = -60 -> TurnLeft
            if (Mathf.DeltaAngle(aircraft.GetHeading(), targetHeading) > 0)
            {
                debugText += "Turn right";
                coroutine_HDG_Right = StartCoroutine(TurnRight(targetHeading));
        }
            else
            {
                debugText += "Turn left";
                coroutine_HDG_Left = StartCoroutine(TurnLeft(targetHeading)); 
            }
        }

        if (aircraft.GetAuthoPoint() == null)
        {
            debugText += " to heading " + TextUtils.Text2SpellFormat(string.Format("{0:D3}", targetHeading)) + ", " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());
        }
        else
        {
            if (aircraft.GetAuthoPoint().GetType() == typeof(VOR))
            {
                debugText += " to fly to " + (aircraft.GetAuthoPoint() as VOR).GetName() + " V O R, " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());
            }
            else
            {
                debugText += " to fly to " + aircraft.GetAuthoPoint().GetId() + ", " + aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber());
            }
        }

        //debugText += " to heading " + targetHeading + ", " + aircraft.GetCallsignCode() + aircraft.GetFlightNumber();
        //Debug.LogWarning(debugText);
        MngDialogs.SetText(debugText, 1);
    }


    private IEnumerator TurnLeft(ushort targetHeading)
    {
        bool tgt_hdg_is_reached = false;

        // If lateral navigation mode is flying to point, refresh every time to avoid deviation by wind, etc.
        if (is_flying_to == true)
        {
            targetHeading = (ushort)GetHeadingToTarget(aircraft.GetAuthoPoint());
        }

        float prevHdg = aircraft.GetHeading();
        float deltaHdg = -(aircraft.GetTurnRate() * Config.aircraftDataPeriod);
        float nextHdg = aircraft.GetHeading() + deltaHdg;

        // Avoid negative and pass through 0       
        if (nextHdg < 0f)
            nextHdg = nextHdg + 360f;
        else if (nextHdg > 360f)
            nextHdg = nextHdg - 360f;

        if ((prevHdg + deltaHdg) < 0f)
            prevHdg = prevHdg + 360f;
        /*
        Debug.Log("prevHdg: " + prevHdg);
        Debug.Log("nextHdg: " + nextHdg);
        Debug.Log("targetHeading: " + targetHeading);
        */
        // Target heading is reached, set heading as target heading
        if (nextHdg <= targetHeading && prevHdg > targetHeading)
        {
            tgt_hdg_is_reached = true;
        }
        else
        {
            tgt_hdg_is_reached = false;
        }

        if (tgt_hdg_is_reached)
        {
            //Debug.Log("Target heading is reached")
            aircraft.SetHeading((ushort)targetHeading);

            if (coroutine_HDG_Left != null && is_flying_to == false)
            {
                //Debug.Log("NAV mode is HDG, then stop coroutine");
                StopCoroutine(coroutine_HDG_Left);
                coroutine_HDG_Left = null;

                is_changing_hdg = false;
            }
            else if (is_flying_to)
            {
                //Debug.Log("NAV mode is flying to, not turn but maintain coroutine");
                // TODO: implement to get corrections by wind...
            }
        }
        else
        {
            // Target heading is not reached yet, continue turn
            //Debug.Log("Target heading is not reached yet, continue turning left to heading: " + targetHeading);
            is_changing_hdg = true;
            aircraft.SetHeading((ushort)nextHdg);

            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            coroutine_HDG_Left = StartCoroutine(TurnLeft(targetHeading));
        }

    }

    private IEnumerator TurnRight(ushort targetHeading)
    {
        bool tgt_hdg_is_reached = false;

        // If lateral navigation mode is flying to point, refresh every time to avoid deviation by wind, etc.
        if (is_flying_to == true)
        {
            targetHeading = (ushort)GetHeadingToTarget(aircraft.GetAuthoPoint());
        }

        float prevHdg = aircraft.GetHeading();
        float deltaHdg = (aircraft.GetTurnRate() * Config.aircraftDataPeriod);
        float nextHdg = aircraft.GetHeading() + deltaHdg;

        // Avoid negative and pass through 0
        if (nextHdg < 0f)
            nextHdg = nextHdg + 360f;
        else if (nextHdg > 360f)
            nextHdg = nextHdg - 360f;

        if ((prevHdg + deltaHdg) > 360f )
            prevHdg = prevHdg - 360f;
        /*
        Debug.Log("prevHdg: " + prevHdg);
        Debug.Log("nextHdg: " + nextHdg);
        Debug.Log("targetHeading: " + targetHeading);
        */
        // Target heading is reached, set heading as target heading
        if (nextHdg >= targetHeading && prevHdg < targetHeading)
        {
            tgt_hdg_is_reached = true;
        }
        else
        {
            tgt_hdg_is_reached = false;
        }

        if (tgt_hdg_is_reached)
        {
            //Debug.Log("Target heading is reached")
            aircraft.SetHeading((ushort)targetHeading);

            if (coroutine_HDG_Right != null && is_flying_to == false)
            {
                //Debug.Log("NAV mode is HDG, then stop coroutine");
                StopCoroutine(coroutine_HDG_Right);
                coroutine_HDG_Right = null;

                is_changing_hdg = false;
            }
            else if (is_flying_to)
            {
                //Debug.Log("NAV mode is flying to, not turn but maintain coroutine");
                // TODO: implement to get corrections by wind...
            }
        }
        else
        {
            // Target heading is not reached yet, continue turn
            //Debug.Log("Target heading is not reached yet, continue turning left to heading: " + targetHeading);
            is_changing_hdg = true;
            aircraft.SetHeading((ushort)nextHdg);

            yield return new WaitForSeconds(Config.aircraftDataPeriod);
            coroutine_HDG_Right = StartCoroutine(TurnRight(targetHeading));
        }

    }

    public void FlyTo(Navaid target)
    {
        //Debug.Log("FlyTo: " + target.GetId());

        is_flying_to = true;
        aircraft.SetAuthoPoint(target);

        // simulate the communication text between ATC and pilots
        //string debugText = aircraft.GetCallsign() + " " + TextUtils.Text2SpellFormat(aircraft.GetFlightNumber()) + ", fly to " + target.GetName();
        //Debug.LogWarning(debugText);
        //MngDialogs.SetText(debugText, 1);

        // set commands to the aircraft
        Turn((ushort)GetHeadingToTarget(target), 1);
         
    }

    public float GetHeadingToTarget(Navaid target)
    {
        
        if (target != null)
        {
            //Debug.Log("Acft pos: " + this.gameObject.transform.position.ToString());
            //Debug.Log("Point pos: " + target.GetGO().transform.position.ToString());
            /*
            float diff_angle = Vector3.SignedAngle(
                new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0f),
                new Vector3(target.GetGO().transform.position.x, target.GetGO().transform.position.y, 0f), 
                new Vector3(0f, 0f, -1f));
            */

            float diff_angle = (float) Math.Atan2(this.gameObject.transform.position.y - target.GetGO().transform.position.y,
                this.gameObject.transform.position.x - target.GetGO().transform.position.x) * Mathf.Rad2Deg;
            /*
            float diff_angle = (float)Math.Atan2(target.GetGO().transform.position.y - this.gameObject.transform.position.y,
                target.GetGO().transform.position.x - this.gameObject.transform.position.x) * Mathf.Rad2Deg;
            */
            /*
            float diff_angle = Vector3.SignedAngle(
                this.gameObject.transform.position,
                target.GetGO().transform.position,
                new Vector3(0f, 0f, -1f));
            */
            //Debug.Log("diff_angle: " + diff_angle);

            float hdg_fly_to = 270 - diff_angle;

            hdg_fly_to = (ushort)(hdg_fly_to % 360);
            hdg_fly_to = (hdg_fly_to == 0 ? (ushort)360 : hdg_fly_to);

            //Debug.Log("hdg_fly_to (post): " + hdg_fly_to);
            return hdg_fly_to;
        }
        else
        { 
            return -1f; 
        }
    }
    Navaid CheckNextPoint()
    {
        Navaid nextPoint = null;
        if (aircraft.GetAuthoStdProcedure() != null)
        {
            Navaid authoPoint = aircraft.GetAuthoPoint();
            float distToNextPoint = Measurement.Distance_DD_NM(aircraft.GetLat(), aircraft.GetLon(), authoPoint.GetLat(), authoPoint.GetLon());

            //Debug.Log("distToNextPoint = " + distToNextPoint + " nm");

            // If aircraft is near enough to authoPoint, remove it and set next one
            if (distToNextPoint < 2.0f) // nm
            {
                //aircraft.GetAuthoStdProcedure()
                //aircraft.SetAuthoPoint(aircraft.GetAuthoStdProcedure().GetNavaids()[0]);
                Debug.Log("Next point");
            }

        }
        return nextPoint;
    }

    public Aircraft GetAircraft() { return aircraft; }

}//class