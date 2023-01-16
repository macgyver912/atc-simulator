using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATCPopupCtrl : MonoBehaviour
{

    public AircraftCtrl controller;

#if !UNITY_ANDROID && !UNITY_IOS
    void OnMouseOver()
    {
        //	if(Input.GetMouseButtonDown(0) && Input.GetButton("RotateLabel"))
        if (Input.GetMouseButtonDown(0))
            controller.GetAircraft().SetLabelPos(DrawRadarScreen.ChangeAcftLabelPos(controller.GetAircraft().GetLabelPos()));
        //	else if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
        if (Input.GetMouseButtonDown(1))
        {
            DrawATCPopup.acftCtrl = null;

            Vector3 mousePos = Input.mousePosition;

            // if aircraft is controlled by player
            if (controller.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Arrival ||
                    controller.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Departure)
            {
                // do popup rect where mouse is
                DrawATCPopup.ctrlPopupRect.x =
                    (DrawATCPopup.ctrlPopupRect.width + mousePos.x > Screen.width ?
                        mousePos.x - DrawATCPopup.ctrlPopupRect.width : mousePos.x);

                DrawATCPopup.ctrlPopupRect.y =
                    (DrawATCPopup.ctrlPopupRect.height - mousePos.y + Screen.height > Screen.height ?
                        -mousePos.y + Screen.height - DrawATCPopup.ctrlPopupRect.height : -mousePos.y + Screen.height);

                DrawATCPopup.showCtrlGUI = true;

                // if aircraft is not controlled by player and is incoming traffic	
            }
            else if (controller.GetAircraft().GetFlightStatus() == Aircraft.FlightStatus.Incoming)
            {
                // do popup rect where mouse is
                DrawATCPopup.noCtrlPopupRect.x =
                    (DrawATCPopup.noCtrlPopupRect.width + mousePos.x > Screen.width ?
                        mousePos.x - DrawATCPopup.noCtrlPopupRect.width : mousePos.x);

                DrawATCPopup.noCtrlPopupRect.y =
                    (DrawATCPopup.noCtrlPopupRect.height - mousePos.y + Screen.height > Screen.height ?
                        -mousePos.y + Screen.height - DrawATCPopup.noCtrlPopupRect.height : -mousePos.y + Screen.height);

                DrawATCPopup.showNoCtrlGUI = true;

                // if aircraft is not controlled by player and is outgoing traffic	
            }
            else
            {
                // do popup rect where mouse is
                DrawATCPopup.transCtrlPopupRect.x =
                    (DrawATCPopup.transCtrlPopupRect.width + mousePos.x > Screen.width ?
                        mousePos.x - DrawATCPopup.transCtrlPopupRect.width : mousePos.x);

                DrawATCPopup.transCtrlPopupRect.y =
                    (DrawATCPopup.transCtrlPopupRect.height - mousePos.y + Screen.height > Screen.height ?
                        -mousePos.y + Screen.height - DrawATCPopup.transCtrlPopupRect.height : -mousePos.y + Screen.height);

                DrawATCPopup.showTransCtrlGUI = true;
            }

        }


        if (DrawATCPopup.acftCtrl == null)
            DrawATCPopup.acftCtrl = controller;

    }
    #endif
}
