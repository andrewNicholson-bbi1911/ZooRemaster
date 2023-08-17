using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wallet", menuName = "Wallet/Create New Wallet", order = 54)]
public class WalletSO : ScriptableObject
{
    [SerializeField] private int _amount;


    public int Amount { get => _amount; private set => _amount = value; }
}