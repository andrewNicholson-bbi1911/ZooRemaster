using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEnable : MonoBehaviour
{
    [SerializeField] private Factory[] _factories;

    private Queue<Factory> _factoriesQueue = new Queue<Factory>();
    //private float _time; // for test

    private void Start()
    {
        //_time = 0; // for test

        foreach (Factory factory in _factories)
        {
            _factoriesQueue.Enqueue(factory);
            factory.gameObject.SetActive(false);
        }
    }

    //private void Update() // for test
    //{
    //    _time += Time.deltaTime;

    //    if (_time > 5f)
    //    {
    //        BuyFactory();
    //        _time = 0;
    //    }
    //}

    public void BuyFactory()
    {
        if (_factoriesQueue.Count > 0)
        {
            Factory factory = _factoriesQueue.Dequeue();
            factory.gameObject.SetActive(true);
        }
    }
}
