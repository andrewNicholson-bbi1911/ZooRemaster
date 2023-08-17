using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private FactoryEnable _factoryEnable;
    [SerializeField] private IngredientSpawner _spawner;

    public void BuyFabric(int fabricPrice)
    {
        if (_wallet.TryTakeMoney(fabricPrice) == false)
            return;

        _factoryEnable.BuyFactory();

        Debug.Log($"������� �������!");

        // need call some logic to instantiate new ingradient on scene
    }

    public void SellIngredient(IngredientSO ingredient)
    {
        _wallet.AddMoney(ingredient.Price);
    }

    public void BuyAnimal(int animalPrice)
    {
        if (_wallet.TryTakeMoney(animalPrice) == false)
            return;

        _spawner.TestSpawn();

        Debug.Log($"�������� ������!");
    }

    public void BuyIngradient(IngredientSO ingredient)
    {
        if (_wallet.TryTakeMoney(ingredient.Price) == false)
            return;

        Debug.Log($"{ingredient.Name} ������!");

        // need call some logic to instantiate new ingradient on scene
    }
}