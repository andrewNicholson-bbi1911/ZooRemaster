using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RSG;

public class Node : MonoBehaviour
{
    private int _index;
    private bool _onEdge = false;
    private Node[] _connectedNodes;
    private Ingredient _ingredient;
    private IPromiseTimer _timer = new PromiseTimer();

    public int Index => _index;
    public bool IsBusy => _ingredient != null;
    public bool OnEdge => _onEdge;
    public Ingredient Animal => _ingredient;
    public Node[] Connected => _connectedNodes;
    public int Row { get; private set; } = 0;

    public void Init(int index, int row, bool edge)
    {
        _index = index;
        _onEdge = edge;
        Row = row;
    }

    public void SetConnected(Node[] nodes)
    {
        _connectedNodes = nodes;
    }

    public void MakeBusy(Ingredient animal)
    {
        _ingredient = animal;
    }

    public void MakeBusy(Ingredient animal, float delay, bool fromAviary)
    {
        _ingredient = animal;
        if (fromAviary)
        {
            _ingredient.MoveFromAviary(transform.position);
        }
        else
        {
            _timer.WaitFor(delay).Then(() =>
            {
                _ingredient.Go(transform.position, 0.5f);
            });
        }
    }

    public void Clear()
    {
        _ingredient = null;
    }

    public bool TryGetPreferedNode(out Node prefered)
    {
        int min = _index;
        prefered = null;
        foreach (Node node in _connectedNodes)
        {
            if (!node.IsBusy && node.Index < min)
            {
                min = node.Index;
                prefered = node;
            }
        }

        return prefered != null;
    }

    public bool TryGetFarNode(out Node farNode)
    {
        int max = _index;
        farNode = null;
        foreach (Node node in _connectedNodes)
        {
            if (!node.IsBusy && node.Index > max)
            {
                max = node.Index;
                farNode = node;
            }
        }

        return farNode != null;
    }

    public void Select()
    {
        _ingredient?.Select();
    }

    public void Deselect()
    {
        _ingredient?.Unselect();
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }
}
