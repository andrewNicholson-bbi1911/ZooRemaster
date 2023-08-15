using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Aviaries : MonoBehaviour
{
    [SerializeField] private ComboContainer _comboContainer;
    [SerializeField] private Factory[] _factories;

    public event UnityAction<List<Ingredient>> ReleasedAnimals;
    public event UnityAction Interacted;
    public event UnityAction GoodAction;
    public event UnityAction BadAction;

    private void OnEnable()
    {
        foreach (var factory in _factories)
        {
            factory.ReleasedIngredient += OnReleasedAnimals;
            factory.Interacted += OnAviaryInteracted;
            factory.BadMove += OnBadAction;
            factory.NiceMove += OnGoodAction;
            factory.VeryNiceMove += OnGoodAction;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _factories)
        {
            item.ReleasedIngredient -= OnReleasedAnimals;
            item.Interacted -= OnAviaryInteracted;
            item.BadMove -= BadAction;
            item.NiceMove -= GoodAction;
            item.VeryNiceMove -= GoodAction;
        }
    }

    private void Start()
    {
        foreach (var item in _factories)
            item.Init(_comboContainer);
    }

    private void OnReleasedAnimals(List<Ingredient> animals)
    {
        ReleasedAnimals?.Invoke(animals);
    }

    private void OnAviaryInteracted(Factory factory)
    {
        Interacted?.Invoke();
    }

    private void OnBadAction()
    {
        BadAction?.Invoke();
    }

    private void OnGoodAction()
    {
        GoodAction?.Invoke();
    }
}
