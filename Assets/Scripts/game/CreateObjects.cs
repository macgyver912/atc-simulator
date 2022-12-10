using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts;

public class CreateObjects : MonoBehaviour
{

    static List<Company> companyList;
    static List<Aircraft> aircraftList;
    public static void Init()
    {
        Debug.Log("Hello World!");

        companyList = new List<Company>();
        aircraftList = new List<Aircraft>();

        companyList.Add(new Company("Iberia Lineas Aereas de España", "IBERIA", "IBE"));
      
        aircraftList.Add(new Aircraft(
            "Airbus A320-214",
            "A320",
            Aircraft.Category.Medium,
            companyList[0],
            "5472",
            null,
            4257,
            //Measurement.DMS2DD(40, 47, 00),
            //Measurement.DMS2DD(-3, 56, 00),
            40.0f,
            -3.0f,
            360,
            0,
            240,
            0,
            0,
            0,
            6000,
            0,
            0,
            15000,
            220,
            "PDT",
            Aircraft.FlightStatus.Arrival
        ));

        //var aircraft = ScriptableObject.CreateInstance<Aircraft>();

        Debug.Log(companyList[0].GetCompanyName() + " | " + companyList[0].GetCallsign() + " | " + companyList[0].GetCallsignCode());
        //Debug.Log(aircraftList[0]);
    }

    
}
