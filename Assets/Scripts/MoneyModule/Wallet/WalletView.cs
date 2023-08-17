using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _walletAmountText;

    private void Start()
    {
        _walletAmountText.text = _wallet.Amount.ToString();
    }

    private void OnEnable()
    {
        _wallet.OnAmountChanged += UpdateView;
    }

    private void OnDisable()
    {
        _wallet.OnAmountChanged -= UpdateView;
    }

    public void UpdateView(int amount)
    {
        _walletAmountText.text = amount.ToString();
    }
}
