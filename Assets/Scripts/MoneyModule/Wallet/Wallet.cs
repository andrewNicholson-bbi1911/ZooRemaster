using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _amount;
    [SerializeField] private WalletSO _walletSO;

    public int Amount { get => _amount; private set => _amount = value; }

    public event Action<int> OnAmountChanged;

    private void Awake()
    {
        _amount = _walletSO.Amount;
    }

    private void OnEnable()
    {
        OnAmountChanged?.Invoke(Amount);
        SellingFactory.IngredientSelled += AddMoney;
    }

    private void OnDisable()
    {
        SellingFactory.IngredientSelled -= AddMoney;
    }

    public bool TryTakeMoney(int removalCount)
    {
        if(removalCount > _amount)
        {
            Debug.LogWarning($" оличество денег в кошельке: {_amount} меньше чем требует списать {removalCount}");
            return false;
        }
        else if(removalCount <= 0)
        {
            Debug.LogWarning($"Ќевозможно списать {removalCount} с кошелька");
            return false;
        }

        _amount -= removalCount;
        OnAmountChanged?.Invoke(_amount);

        return true;
    }

    public void AddMoney(int additionsCount)
    {
        _amount += additionsCount;

        OnAmountChanged?.Invoke(_amount);
    }
}