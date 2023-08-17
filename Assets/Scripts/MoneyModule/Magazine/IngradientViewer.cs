using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngradientViewer : MonoBehaviour
{
    [SerializeField] private IngredientSO _ingredientSO;

    [SerializeField] private Shop _shop;

    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;
    [SerializeField] private TMP_Text _nameLabel;

    public event Action<IngredientSO, IngradientViewer> OnBuyButtonClicked;
    public event Action<IngredientSO, IngradientViewer> OnSellButtonClicked;

    private void Start()
    {
        _nameLabel.text = $"Buy Animal: 25$";
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(RaiseAnimalBuyButtonClicked);

        //_buyButton.onClick.AddListener(RaiseBuyButtonClicked);
        //_sellButton.onClick.AddListener(RaiseSellButtonClicked);
    }

    private void OnDisable()
    {
        //_buyButton.onClick.RemoveListener(RaiseBuyButtonClicked);
        //_sellButton.onClick.RemoveListener(RaiseSellButtonClicked);
    }

    private void RaiseAnimalBuyButtonClicked()
    {
        _shop.BuyAnimal(25);
    }

    private void RaiseBuyButtonClicked()
    {
        OnBuyButtonClicked?.Invoke(_ingredientSO, this);
    }

    private void RaiseSellButtonClicked()
    {
        OnSellButtonClicked?.Invoke(_ingredientSO, this);
    }
}