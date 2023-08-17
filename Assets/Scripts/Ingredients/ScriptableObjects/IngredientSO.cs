using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "Product/New Ingredient", order = 54)]
public class IngredientSO : ScriptableObject
{
    public Sprite ingredientSprite;

    [SerializeField] private GameObject _ingredientRefference;
    [SerializeField] private int _price;
    [SerializeField] private string _name;

    public GameObject IngredientRefference { get => _ingredientRefference; }
    public int Price { get => _price; }
    public string Name { get => _name; }
}
