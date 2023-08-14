using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "Product/New Recipe", order = 54)]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<IngredientSO> _ingredient;
    [SerializeField] private IngredientSO _finalProduct;

    public List<IngredientSO> Ingredient { get => _ingredient; }
    public IngredientSO FinalProduct { get => _finalProduct; }
}