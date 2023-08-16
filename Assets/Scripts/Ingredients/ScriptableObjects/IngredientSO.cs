using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "Product/New Ingredient", order = 54)]
public class IngredientSO : ScriptableObject
{
    [SerializeField] private MeshFilter _ingredientModel;
    [SerializeField] private MeshRenderer _ingredientMaterial;
    [SerializeField] private int _price;
    [SerializeField] private string _name;

    public MeshFilter IngredientModel { get => _ingredientModel; }
    public MeshRenderer IngredientMaterial { get => _ingredientMaterial; }
    public int Price { get => _price; }
    public string Name { get => _name; }
}
