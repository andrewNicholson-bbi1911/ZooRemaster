using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using RSG;
public class SellingFactory : Factory
{

    [SerializeField] private int _moneyCollected = 0;

    public override bool TryProcessProduct(RecipeSO recipe)
    {
        foreach(var ingredient in _ingredientsContained)
        {
            Sell(ingredient);
        }
        UseIngredinents(new List<Ingredient>(_ingredientsContained));
        return true;
    }

    public override bool TryTakeGroup(List<Node> nodes)
    {
        List<Node> sortedNodes = nodes.OrderBy(item => Vector3.Distance(item.transform.position, transform.position)).ToList();
        List<Ingredient> newIngredients = new List<Ingredient>();
        for (int i = 0; i < sortedNodes.Count; i++)
        {
            Node node = sortedNodes[i];
            if (node.IsBusy)
                newIngredients.Add(node.Animal);

            node.Deselect();
            node.Clear();
        }
        OpenDoor();

        //bool sameAnimals = _ingredients.Count == 0 || newIngredients[0].ID == _ingredients[_ingredients.Count - 1].ID;
        StartCoroutine(AddAnimalsLoop(newIngredients, true));

        return true;
    }


    protected override void ReactOnNewIngredients(List<Ingredient> newAnimals)
    {
        if (newAnimals.Count > 4)
        {
            DoOnVeryNiceMove();
        }
        else if (newAnimals.Count >= 1)
        {
            DoOnNiceMove();
        }

        TryProcessProduct(null);

        DoInterracted(this);

    }

    private void Sell(Ingredient ingredient)
    {
        var amount = ingredient.Price;
        DoOnProcessed(new InGameEvent(InGameEvenType.Sell, ingredient.Name, 1));
        DoOnProcessed(new InGameEvent(InGameEvenType.EarnMoney, ingredient.Name, amount));
        _moneyCollected += amount;
    }
}
