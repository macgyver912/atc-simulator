using System.Collections;
using UnityEngine;

public class MngCamera : MonoBehaviour
{

    static Camera mainCamera;
    void Awake()
    {
        //	instance = this;
    }
    static void Init()
    {
        mainCamera = Camera.main;
    }
    public static Camera GetCamera() { return mainCamera; }
}