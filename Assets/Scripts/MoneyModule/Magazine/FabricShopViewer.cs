using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FabricShopViewer : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TMP_Text _nameLabel;

    [SerializeField] private int _fabricPrice = 100;

    public event Action<int> OnBuyButtonClicked;
    //public event Action<> OnSellButtonClicked;

    private void Start()
    {
        //_nameLabel.text = $"Fabric: {_fabricPrice}";
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(RaiseBuyButtonClicked);
        //_sellButton.onClick.AddListener(RaiseSellButtonClicked);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(RaiseBuyButtonClicked);
        //_sellButton.onClick.RemoveListener(RaiseSellButtonClicked);
    }

    private void RaiseBuyButtonClicked()
    {
        OnBuyButtonClicked?.Invoke(_fabricPrice);
    }

    //private void RaiseSellButtonClicked()
    //{
    //    OnSellButtonClicked?.Invoke();
    //}
}
