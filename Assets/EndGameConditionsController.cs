using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using Lean.Localization;

public class EndGameConditionsController : MonoBehaviour
{
    [SerializeField] private List<EndGameCondition> _levelConditions = new List<EndGameCondition>();
    [SerializeField] private List<Factory> _factories;
    [Space]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _actionText;
    [SerializeField] private TextMeshProUGUI _amountRemainingText;
    [SerializeField] private TextMeshProUGUI _ingredientNameText;
    //[SerializeField] private Image _image;
    [Space]
    [Header("Other")]
    //[SerializeField] private L
    private EndGameCondition _actualCondition;

    public UnityAction OnConditionComplited;

    private void OnEnable()
    {
        foreach (var factory in _factories)
        {
            factory.OnProcessed += UpdateCondition;
        }
    }

    private void OnDisable()
    {
        foreach (var factory in _factories)
        {
            factory.OnProcessed -= UpdateCondition;
        }
    }

    public void LoadCondition(int level)
    {
        _actualCondition = _levelConditions[level % _levelConditions.Count];
        _actualCondition.OnConditionComplited += ConditionComplited;
        _actualCondition.OnConditionUpdated += UpdateConditionData;
        UpdateConditionData();
    }


    private void UpdateCondition(InGameEvent gameEvent)
    {
        _actualCondition.UpdateCondition(gameEvent);
    }

    private void ConditionComplited()
    {
        OnConditionComplited.Invoke();
    }

    private void UpdateConditionData()
    {
        //_image.sprite = _actualCondition.IngredientSprite;
        _actionText.text = GetActionText();
        _ingredientNameText.text = _actualCondition.Name;
        _amountRemainingText.text = _actualCondition.AmountRemaining.ToString();
    }

    private string GetActionText()
    {
        var actionTex = "_Produce";

        switch (_actualCondition.Action)
        {
            case InGameEvenType.Produce:
                actionTex = "_Produce";
                break;
            case InGameEvenType.EarnMoney:
                actionTex = "_Earn";
                break;
            case InGameEvenType.Sell:
                actionTex = "_Sell";
                break;
        }


        var text = LeanLocalization.GetTranslationText(actionTex);
        if(text == null || text == "")
        {
            text = actionTex.Replace("_", "");
        }
        return text;
    }
}


[System.Serializable]
public struct EndGameCondition
{
    public InGameEvenType Action { get => _whatToTrack; }
    //public Sprite IngredientSprite { get => _whatIngredient.ingredientSprite; }
    public int AmountRemaining { get => _howMuchRequered; }
    public string Name { get => _whatIngredient.Name; }

    [SerializeField] private InGameEvenType _whatToTrack;
    [SerializeField] private IngredientSO _whatIngredient;
    [SerializeField] private int _howMuchRequered;

    public UnityAction OnConditionComplited;
    public UnityAction OnConditionUpdated;

    public void UpdateCondition(InGameEvent eventData){
        if(_whatToTrack == eventData.EventType)
        {
            if(_whatToTrack == InGameEvenType.EarnMoney || _whatIngredient.Name == eventData.ObjectID)
            {
                _howMuchRequered -= eventData.Amount;
                OnConditionUpdated?.Invoke();
            }
        }

        if(_howMuchRequered <= 0)
        {
            _howMuchRequered = 0;
            OnConditionComplited?.Invoke();
        }
    }
}


public class InGameEvent
{
    public InGameEvenType EventType;
    public string ObjectID = "";
    public int Amount = 0;

    public InGameEvent(InGameEvenType type, string id, int amount)
    {
        EventType = type;
        ObjectID = id;
        Amount = amount;
    }
}


public enum InGameEvenType
{
    Produce,
    Sell,
    EarnMoney,
}
