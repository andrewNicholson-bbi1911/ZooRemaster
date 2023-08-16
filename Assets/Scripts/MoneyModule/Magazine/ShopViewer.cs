using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopViewer : MonoBehaviour
{
    [SerializeField] private List<IngradientViewer> _ingradientViewers;
    [SerializeField] private Shop _shop;

    private void OnEnable()
    {
        foreach (var item in _ingradientViewers)
        {
            item.OnBuyButtonClicked += OnBuyButtonViewerClicked;
            item.OnSellButtonClicked += OnSellButtonViewerClicked;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _ingradientViewers)
        {
            item.OnBuyButtonClicked -= OnBuyButtonViewerClicked;
            item.OnSellButtonClicked -= OnSellButtonViewerClicked;
        }
    }

    public void OnBuyButtonViewerClicked(IngredientSO ingredientSO, IngradientViewer ingradientViewer)
    {
        _shop.Buy(ingredientSO);
    }

    public void OnSellButtonViewerClicked(IngredientSO ingredientSO, IngradientViewer ingradientViewer)
    {
        _shop.SellIngredient(ingredientSO);

        _ingradientViewers.Remove(ingradientViewer);
    }
}