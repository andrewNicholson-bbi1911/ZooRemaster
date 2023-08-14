using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIngradientSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _ingadient;

    private void Start()
    {
        Instantiate(_ingadient);
    }
}