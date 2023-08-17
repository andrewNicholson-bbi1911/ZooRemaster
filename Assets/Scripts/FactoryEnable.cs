using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEnable : MonoBehaviour
{
    [SerializeField] private Factory[] _factories;

    private void Start()
    {
        foreach (Factory factory in _factories)
        {
            Debug.Log(factory.name);
        }
    } 
}
