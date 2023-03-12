using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _intance;

    public static T Intance
    {
        get
        {
            if (_intance == null)
            {
                _intance = FindObjectOfType<T>();
                if (_intance == null)
                {
                    GameObject obj = GameObject.Find("Manager");
                    if(obj == null)
                    {
                        obj = new GameObject("Manager");
                    }
                    _intance = obj.AddComponent<T>();
                }
            }
            return _intance;
        }
    }
}
