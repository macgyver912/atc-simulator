

using UnityEngine;

namespace Assets.Scripts
{


    /**
     * Aircraft characteristics and flight data.
     *
     * @module Aircraft
     * @main Aircraft
     * @class Aircraft
     * @date December 10th, 2022
     * @author Jaime Valle Alonso
     */
    public class Aircraft : ScriptableObject
    {

        /**
        * Aircraft turbulence categories.
        * @attribute Category
        * @type {enum}
        */
        public enum Category
        {
            Light,
            Medium,
            Heavy
        };

        public enum FlightStatus
        {
            Incoming,           // Incoming traffic before contact it
            Arrival,            // Incoming traffic with player
            Departure,          // Outgoing traffic with player
            Transferred         // Outgoing traffic handled off by player	
        };

        // Turn rates
        //enum TurnAngle {AcftLight = 30, AcftMedium = 25, AcftHeavy = 20};
        enum TurnRate : ushort { AcftLight = 6, AcftMedium = 3, AcftHeavy = 2 };
        // Vertical speed rates
        enum VSRate_Std : ushort { AcftLight = 2600, AcftMedium = 1600, AcftHeavy = 900 };			// jets (no Cessna and similar)
        enum VSRate_Max : ushort { AcftLight = 4000, AcftMedium = 2600, AcftHeavy = 1400 };			// jets (no Cessna and similar)
		// Speed rates
        enum SpeedRate_Brake : ushort { AcftLight = 8, AcftMedium = 7, AcftHeavy = 5 };				// jets (no Cessna and similar)
        enum SpeedRate_TO : ushort { AcftLight = 7, AcftMedium = 5, AcftHeavy = 3 };				// jets (no Cessna and similar)
        enum SpeedRate_Air_Std : ushort { AcftLight = 2, AcftMedium = 1, AcftHeavy = 1 };			// jets (no Cessna and similar)
        enum SpeedRate_Air_Max : ushort { AcftLight = 4, AcftMedium = 2, AcftHeavy = 2 };			// jets (no Cessna and similar)

        /**
         * Aircraft model, <i>e.g. A320, B738</i>.
         * @attribute aircraftModelCode
         * @type {string}
         */
        string aircraftModelCode;
        /**
		 * Aircraft model name, <i>e.g. Airbus A320-214, Boeing 737-800</i>.
		 * @attribute aircraftModelName
		 * @type {string}
		 */
        string aircraftModelName;
        /**
		 * Aircraft turbulence category.
		 * @attribute category
		 * @type {Category}
		 */
        Category category;
        /**
		 * Aircraft company, <i>e.g. Iberia, British Airways, Ryanair, American Airlines</i>.
		 * @attribute company
		 * @type {Company}
		 */
        Company company;
        /**
		 * Airline callsign, <i>e.g. Iberia, Speedbird, Ryanair, American</i>.
		 * @attribute callsign
		 * @type {string}
		 */
        string callsign;
        /**
		 * Callsign is an unique identifier alphanumeric code to represent the callsign and flight number as ICAO format, i.e.
		 * 3 letters representing airline followed by flight number. <i>E.g. IBE2451, BAW487, RYR2032, AAL538</i>.
		 * Note that callsing and flight number may be different. This occurs when an aircraft is sharing
		 * more than one flight in the same travel or aircraft operates a flight of other company.
		 * <br>
		 * Moreover, some airlines has different codes for callsign and flight number as default. For example,
		 * Iberia has the same code but reprensented as IATA format (2 letters representing airline followed by
		 * flight number), while Ryanair has different code. Some examples:
		 *
		 * <br>
		 *
		 * <table>
		 *	<caption>Aircraft callsign and flight number when operates a unique and own flight</caption>
		 * 	<tr>
		 *		<td><b>Callsign</b></td><td><b>Callsign code</b></td><td><b>Flight number</b></td>
		 *	</tr>
		 * 	<tr>
		 *		<td>Iberia 2451</td><td>IBE2451</td><td>IB2451</td>
		 *	</tr>
		 * 	<tr>
		 *		<td>Ryanair 2032</td><td>RYR2032</td><td>FR2032</td>
		 *	</tr>
		 * 	<tr>
		 *		<td>Vueling 8991</td><td>VLG8991</td><td>VY8991</td>
		 *	</tr>
		 * </table>
		 *
		 * <br>
		 *
 		 * <table>
		 *	<caption>Aircraft callsign and flight number when is sharing two flights</caption>
		 * 	<tr>
		 *		<td><b>Callsign</b></td><td><b>Callsign code</b></td><td><b>Flight number</b></td>
		 *	</tr>
		 * 	<tr>
		 *		<td>Iberia 6275</td><td>IBE6275</td><td>AA5683/BA4271</td>
		 *	</tr>
		 * </table>
		 *
		 * @attribute callsignCode
		 * @type {string}
		 */
        string callsignCode;
        /**
 		 * Flight number registered in flight plan.
		 * @attribute flightNumber
		 * @type {string}
		 */
        string flightNumber;
        /**
		 * Registration or tail number is the alphanumeric code painted on an aircraft, frequently on the tail.
		 * @attribute registration
		 * @type {string}
		 */
        string registration;	// not in use
		/**
		 * Squawk refers to 4-digint octal number to identify the aircraft by SSR (Secondary Surveillance Radar).
		 * Must be unique within same controlled area and can be recicled under ATC (Air Traffic Controller) request.
		 * @attribute squawk
		 * @type {ushort}
		 */
		ushort squawk;
        /**
		 * Aircraft position: latitude, longitude coordinates in degrees and altitude in feet.
		 * @attribute position
		 * @type {Vector3}
		 */
        Vector3 position;
        /**
		 * Position in screen (in pixels): latitude, longitude.
		 * @attribute screenPosition
		 * @type {Vector3}
		 */
        Vector3 screenPosition;
		/**
		 * Heading refers to direction that aircraft's nose is pointing.
		 * @attribute heading
		 * @type {ushort}
		 */
		ushort heading;
        /**
		 * Track refers to real direction that aircraft is following. The main difference between track and heading
		 * is crosswind that creates a new force component in different course that engines are pushing the aircraft.
		 * @attribute track
		 * @type {ushort}
		 */
        ushort track;
		/**
		 * GS or Ground Speed is aircraft's velocity referring to Earth (ground). All aeronautical speeds are
		 * measured in knots (nautical miles for hour), that is represented by <i>kt</i> or <i>kts</i>.
		 * <br><br>
		 * <table>
		 *	<caption>Distance conversion</caption>
		 * 	<tr>
		 *		<td><b>Kilometers</b></td><td><b>Nautical miles</b></td><td><b>Statute miles</b></td>
		 *	</tr>
		 * 	<tr>
		 *		<td>1.852 km</td><td>1 nm</td><td>1.151 sm</td>
		 *	</tr>
		 * </table>
		 *
		 * @attribute speedGS
		 * @type {ushort}
		 */
		ushort speedGS;
		/**
		 * IAS or Indicated Air Speed is aircraft's velocity referring to the surronding air.
		 * Is the most useful airspeed measurement because indicates the aircraft performance and capacities
		 * due to wing's lift and aerodynamics. As all airspeeds is measured with a Pitot Tube and the quantity
		 * measured depends of aircraft's altitude. The higher the altitude the lower the IAS while flying the
		 * same TAS.
		 * @attribute speedIAS
		 * @type {ushort}
		 */
		ushort speedIAS;
		/**
		 * CAS or Calibrated Air Speed is the same that IAS but correcting the measurement for standard errors,
		 * as pitch angle.
		 * @attribute speedCAS
		 * @type {ushort}
		 */
		ushort speedCAS;
        /**
		 * TAS or True Air Speed is the same that CAS but correcting the measurement for air temperature and
		 * pressure altitude. Thus, GS and TAS are the same quantity only when wind speed is zero. In other cases,
		 * relationship between GS and TAS is the addition or substract of their vectorial magnitudes.
		 * @attribute speedTAS
		 * @type {ushort}
		 */
        ushort speedTAS;
		/**
		 * Altitude is the measured distance between the aircraft and the mean sea level. Because this measurement is
		 * variable with air pressure, the distance from aircraft to reference point differs along travel althought
		 * flight level/altitude is the same.
		 * @attribute altitude
		 * @type {ushort}
		 */
		ushort altitude;
		/**
		 * Height is defined as distance between the aircraft and the obstacles below it.
		 * @attribute height
		 * @type {ushort}
		 */
		ushort height;
		/**
		 * VS or Vertical Speed is the climb or descent rate in feet per minute of aircraft.
		 * <br>
		 * <table>
		 *	<caption>Altitude conversion</caption>
		 * 	<tr>
		 *		<td><b>Feet</b></td><td><b>Meters</b></td>
		 *	</tr>
		 * 	<tr>
		 *		<td>1 ft</td><td>0.3048 m</td>
		 *	</tr>
		 * 	<tr>
		 *		<td><b>Meters</b></td><td><b>Feet</b></td>
		 *	</tr>
		 * 	<tr>
		 *		<td>1 m</td><td>3.28 ft</td>
		 *	</tr>
		 * </table>
		 *
		 * @attribute verticalSpeed
		 * @type {ushort}
		 */
		short verticalSpeed;
        /**
		 * Icon to represent objects of this class.
		 * @attribute icon
		 * @type {Texture2D}
		 */
        Texture2D icon;
        /**
		 * GameObject to represent graphically this class.
		 * @attribute go
		 * @type {GameObject}
		 */
        GameObject go;
        /**
		 * Script to control this Aircraft.
		 * @attribute script
		 * @type {Aircraft}
		 */
        Aircraft script;

        /**
		 * Authorized altitude (in feet).
		 * @attribute authoAltitude
		 * @type {ushort}
		 */
        ushort authoAltitude;
        /**
		 * Authorized speed (in kts).
		 * @attribute authoSpeed
		 * @type {ushort}
		 */
        ushort authoSpeed;
        /**
		 * Name of authorized FIX, VOR, etc.
		 * @attribute authoPoint
		 * @type {string}
		 */
        string authoPoint;
        /**
		 * Indicates if traffic is incoming (Arrival) or outcoming (Departure)
		 * @attribute flightStatus
		 * @type {FlightStatus}
		 */
        FlightStatus flightStatus;


        string label;
        //AcftLabelPos labelPos;
		Vector3 labelScreenPos;
		Vector2 labelLineOffset;
        ushort turnRate;
        ushort vsRate_Std;
		ushort vsRate_Max;
		ushort speedRate_Brake;
		ushort speedRate_TO;
		ushort speedRate_Air_Std;
		ushort speedRate_Air_Max;


        /**
		 * @class Aircraft 
		 * @constructor
		 * @param {String} modelName Aircraft model name, <i>e.g. Airbus A320-214, Boeing 737-800</i>.
 		 * @param {String} modelCode Aircraft model, <i>e.g. A320, B738</i>.
 		 * @param {Category} category Aircraft turbulence category.
 		 * @param {Company} company Aircraft company, <i>e.g. Iberia, British Airways, Ryanair, American Airlines</i>.
 		 * @param {String} flightNumber Flight number registered in flight plan.
 		 * @param {String} registration Registration or tail number is the alphanumeric code painted on an aircraft, frequently on the tail.
 		 * @param {ushort} squawk Transponder code.
		 * @param {float} lat Latitude coordinates in degrees. 
		 * @param {float} lon Longitude coordinates in degrees.
 		 * @param {ushort} heading Heading refers to direction that aircraft's nose is pointing.
 		 * @param {ushort} track Track refers to real direction that aircraft is following.
 		 * @param {ushort} speedGS GS or Ground Speed is aircraft's velocity referring to Earth (ground).
 		 * @param {ushort} speedIAS IAS or Indicated Air Speed is aircraft's velocity referring to the surronding air.
 		 * @param {ushort} speedCAS CAS or Calibrated Air Speed is the same that IAS but correcting the measurement for standard errors.
 		 * @param {ushort} speedTAS TAS or True Air Speed is the same that CAS but correcting the measurement.
 		 * @param {ushort} altitude Altitude is the measured distance between the aircraft and the mean sea level.
 		 * @param {ushort} height Height is defined as distance between the aircraft and the obstacles below it.
 		 * @param {short} verticalSpeed VS or Vertical Speed is the climb or descent rate in feet per minute of aircraft.
 		 * @param {short} authoAltitude Authorized altitude (in feet).
 		 * @param {short} authoSpeed Authorized speed (in kts).
 		 * @param {String} authoPoint Name of authorized FIX, VOR, etc.
 		 * @param {FlightStatus} flightStatus Indicates if traffic is incoming (Arrival) or outcoming (Departure).
		 */
        public Aircraft(string modelName, string modelCode, Category category, Company company,
					string flightNumber, string registration, ushort squawk, float lat, float lon,
					ushort heading, ushort track, ushort speedGS, ushort speedIAS, ushort speedCAS,
					ushort speedTAS, ushort altitude, ushort height, short verticalSpeed,
					ushort authoAltitude, ushort authoSpeed, string authoPoint, FlightStatus flightStatus)
		{
            this.aircraftModelName = modelName;
            this.aircraftModelCode = modelCode;
            this.category = category;
            this.company = company;
            this.callsign = company.GetCallsign();
            this.callsignCode = company.GetCallsignCode();
            this.flightNumber = flightNumber;
            this.registration = registration;
            this.squawk = squawk;
            this.position = new Vector3(lon, lat, altitude);
            this.heading = heading;
            this.track = track;
            this.speedGS = speedGS;
            //		this.speedIAS = speedIAS;
            this.speedIAS = speedGS;
            this.speedCAS = speedCAS;
            this.speedTAS = speedTAS;
            this.altitude = altitude;
            this.height = height;
            this.verticalSpeed = verticalSpeed;

            this.authoAltitude = authoAltitude;
            this.authoSpeed = authoSpeed;
            this.authoPoint = authoPoint;

            this.flightStatus = flightStatus;

            
            //this.labelPos = DrawRadarScreen.prefAcftLabelPos;
						
            //this.icon = DrawRadarScreen.icons["aircraft"];
			
            this.go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            this.go.name = this.GetType() + "_" /*+ this.company.callsignCode*/ + this.flightNumber;
            this.go.GetComponent<Renderer>().material.mainTexture = this.icon;
            this.go.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            //		this.go.renderer.material.color = new Color(1,1,1,1);
            //this.go.GetComponent<Renderer>().material.color.a = 1;
            this.go.transform.rotation = Quaternion.Euler(90 + this.heading, 90, 270);
            this.go.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
			/*
            this.go.AddComponent(AircraftCtrl);
            this.go.GetComponent(AircraftCtrl).aircraft = this;

            this.go.AddComponent(ATCPopupCtrl);
            this.go.GetComponent(ATCPopupCtrl).controller = this.go.GetComponent(AircraftCtrl);
            //		this.go.GetComponent(ATCPopupCtrl).aircraft = this;
			*/

            if (this.category == Aircraft.Category.Light)
            {
                this.turnRate = (ushort) TurnRate.AcftLight;
                this.vsRate_Std = (ushort)VSRate_Std.AcftLight;
                this.vsRate_Max = (ushort) VSRate_Max.AcftLight;
                this.speedRate_Brake = (ushort) SpeedRate_Brake.AcftLight;
                this.speedRate_TO = (ushort) SpeedRate_TO.AcftLight;
                this.speedRate_Air_Std = (ushort) SpeedRate_Air_Std.AcftLight;
                this.speedRate_Air_Max = (ushort) SpeedRate_Air_Max.AcftLight;
            }
            else if (this.category == Aircraft.Category.Medium)
            {
                this.turnRate = (ushort) TurnRate.AcftMedium;
                this.vsRate_Std = (ushort) VSRate_Std.AcftMedium;
                this.vsRate_Max = (ushort) VSRate_Max.AcftMedium;
                this.speedRate_Brake = (ushort) SpeedRate_Brake.AcftMedium;
                this.speedRate_TO = (ushort) SpeedRate_TO.AcftMedium;
                this.speedRate_Air_Std = (ushort) SpeedRate_Air_Std.AcftMedium;
                this.speedRate_Air_Max = (ushort) SpeedRate_Air_Max.AcftMedium;
            }
            else
            {
                this.turnRate = (ushort) TurnRate.AcftHeavy;
                this.vsRate_Std = (ushort) VSRate_Std.AcftHeavy;
                this.vsRate_Max = (ushort) VSRate_Max.AcftHeavy;
                this.speedRate_Brake = (ushort) SpeedRate_Brake.AcftHeavy;
                this.speedRate_TO = (ushort) SpeedRate_TO.AcftHeavy;
                this.speedRate_Air_Std = (ushort) SpeedRate_Air_Std.AcftHeavy;
                this.speedRate_Air_Max = (ushort) SpeedRate_Air_Max.AcftHeavy;
            }
        }

		/*
        public void SetGameObjectPos()
        {
            // this.screenPosition = MngScreen.ScreenPosRelToAirport(this.position.x, this.position.y, 0);
            this.screenPosition = MngScreen.RadarScreenPosRelToAirport(this.position.x, this.position.y, 0);
            this.go.transform.position = this.screenPosition;
            // this.position = this.go.transform.position;
        }
		*/

    }// end-class

}// end-namespace