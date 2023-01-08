using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInstance : MonoBehaviour
{
    Transform ILS_Instance;
    static Transform _ILS_Instance;

    void Awake()
    {
        _ILS_Instance = ILS_Instance;
    }

    public static Transform CreateILSInstance()
    {
        //	Debug.Log(_ILS_Instance);
        return Instantiate(_ILS_Instance, Vector3.zero, Quaternion.identity);
    }

}// end-class
