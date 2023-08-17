using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private FactoryEnable _factoryEnable;

    public void BuyFabric(int fabricPrice)
    {
        if (_wallet.TryTakeMoney(fabricPrice) == false)
            return;

        _factoryEnable.BuyFactory();

        Debug.Log($"Фабрика куплена!");

        // need call some logic to instantiate new ingradient on scene
    }

    public void SellIngredient(IngredientSO ingredient)
    {
        _wallet.AddMoney(ingredient.Price);
    }

    public void BuyIngradient(IngredientSO ingredient)
    {
        if (_wallet.TryTakeMoney(ingredient.Price) == false)
            return;

        Debug.Log($"{ingredient.Name} куплен!");

        // need call some logic to instantiate new ingradient on scene
    }
}