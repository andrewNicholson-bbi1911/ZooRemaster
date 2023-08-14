using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "Product/New Recipe", order = 54)]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private List<IngredientSO> _ingredients;
    [SerializeField] private IngredientSO _finalProduct;

    public List<IngredientSO> Ingredients { get => _ingredients; }
    public IngredientSO FinalProduct { get => _finalProduct; }
}