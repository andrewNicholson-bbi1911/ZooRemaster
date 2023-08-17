using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopViewer : MonoBehaviour
{
    [SerializeField] private List<IngradientViewer> _ingradientViewers;

    [SerializeField] private FabricShopViewer _fabricShopViewer;
    [SerializeField] private Shop _shop;

    private void OnEnable()
    {
        foreach (var item in _ingradientViewers)
        {
            item.OnBuyButtonClicked += OnBuyIngradientButtonViewerClicked;
            item.OnSellButtonClicked += OnSellIngradientButtonViewerClicked;
        }

        _fabricShopViewer.OnBuyButtonClicked += OnBuyFabricButtonViewerClicked;
    }

    private void OnDisable()
    {
        foreach (var item in _ingradientViewers)
        {
            item.OnBuyButtonClicked -= OnBuyIngradientButtonViewerClicked;
            item.OnSellButtonClicked -= OnSellIngradientButtonViewerClicked;
        }

        _fabricShopViewer.OnBuyButtonClicked -= OnBuyFabricButtonViewerClicked;
    }

    public void OnBuyFabricButtonViewerClicked(int fabricPrice)
    {
        _shop.BuyFabric(fabricPrice);
    }

    public void OnBuyIngradientButtonViewerClicked(IngredientSO ingredientSO, IngradientViewer ingradientViewer)
    {
        _shop.BuyIngradient(ingredientSO);
    }

    public void OnSellIngradientButtonViewerClicked(IngredientSO ingredientSO, IngradientViewer ingradientViewer)
    {
        _shop.SellIngredient(ingredientSO);

        _ingradientViewers.Remove(ingradientViewer);
    }
}