using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SpawnAnimals
{

}

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] [Range(0,1)] private float _AnimalRatio;
    [SerializeField] private Net _net;
    [Space]
    [SerializeField] private IngredientSO _testIngredient;
    private AnimalSet _set;
    private List<Ingredient> _ingredients = new List<Ingredient>();
    private int[] _spawned;

    private void OnEnable()
    {
        _game.LevelStarted += ChangeAnimalSet;
    }

    private void OnDisable()
    {
        _game.LevelStarted -= ChangeAnimalSet;
    }

    public void TestSpawn()
    {
        SpawnIngredient(_testIngredient);
    }

    public Ingredient SpawnIngredient(IngredientSO ingredientSO)
    {
        var index = NewAnimalIndex();
        var newNod = _net.GetFreeNode();
        if(newNod == null)
        {
            return null;
        }
        Vector3 position = newNod.transform.position;
        var animalObj = Instantiate(ingredientSO.IngredientRefference, position, Quaternion.LookRotation(Vector3.back, Vector3.up));
        var animal = animalObj.GetComponent<Ingredient>();
        _ingredients.Add(animal);
        animal.OnMovedToFactory.AddListener(() => OnIngredientLeft(index));
        animal.OnRemovedFromFactory.AddListener(() => OnIngredientReturned(index));
        _spawned[index]++;
        newNod.MakeBusy(animal);
        return animal;
    }


    public Ingredient Spawn(Vector3 position)
    {
        var index = NewAnimalIndex();
        Ingredient animal = Instantiate(_set.GetAnimalTemplate(index), position, Quaternion.LookRotation(Vector3.back, Vector3.up));
        _ingredients.Add(animal);
        animal.OnMovedToFactory.AddListener(() => OnIngredientLeft(index));
        animal.OnRemovedFromFactory.AddListener(() => OnIngredientReturned(index));
        _spawned[index]++;
        return animal;
    }

    private void ChangeAnimalSet(int level, LevelType type)
    {
        _set = type.AnimalSet;
        _spawned = new int[_set.Size];
    }

    private int NewAnimalIndex()
    {
        var animalsCount = 0;
        foreach (var count in _spawned)
            animalsCount += count;
        var ratios = new float[_spawned.Length];
        for (var i=0; i< _set.Size; i++)
        {
            if (animalsCount - _spawned[i] > 0)
            {
                ratios[i] = (float)_spawned[i] / ((animalsCount - _spawned[i])/(_set.Size -1));
            }
        }
        var minRatioIndex = Random.Range(0, _set.Size);
        for (var i = 0; i < _set.Size; i++)
        {
            if (ratios[i] < _AnimalRatio && ratios[i] < ratios[minRatioIndex])
            {
                minRatioIndex = i;
            }
        }
        return minRatioIndex;
    }

    private void OnIngredientReturned(int index)
    {
        _spawned[index]++;
    }

    private void OnIngredientLeft(int index)
    {
        _spawned[index]--;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log(s);
    //    }
    //}

    //private void WriteString(string data)
    //{
    //    string path = "Assets/test.json";

    //    StreamWriter writer = new StreamWriter(path, false);
    //    writer.Write(data);
    //    writer.Close();
    //}

    //private string ReadString()
    //{
    //    string path = "Assets/test.json";

    //    StreamReader reader = new StreamReader(path);
    //    string result = reader.ReadToEnd();
    //    reader.Close();

    //    return result;
    //}
}
