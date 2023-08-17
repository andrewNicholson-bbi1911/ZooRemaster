using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    public void SellIngredient(IngredientSO ingredient)
    {
        _wallet.AddMoney(ingredient.Price);
    }

    public void Buy(IngredientSO ingredient)
    {
        if (_wallet.TryTakeMoney(ingredient.Price) == false)
            return;

        Debug.Log($"{ingredient.Name} куплен!");

        // need call some logic to instantiate new ingradient on scene

    }
}