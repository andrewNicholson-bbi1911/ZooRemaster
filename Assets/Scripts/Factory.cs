using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using RSG;

public class Factory : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private float _movePerAnimal = 0.1f;
    [SerializeField] private ComboText _comboText;
    [SerializeField] private Image _comboImage;
    [SerializeField] private List<RecipeSO> _recipes;
    [SerializeField] private FactoryPoint _factoryPoint; 

    [SerializeField] private ParticleSystem _confetti;

    [SerializeField] private List<IngredientSO> _producedProducts = new List<IngredientSO>();


    protected List<Ingredient> _ingredientsContained = new List<Ingredient>();

    public List<IngredientSO> _usingIngredients = new List<IngredientSO>();
    private IPromiseTimer _promiseTimer = new PromiseTimer();
    private ComboContainer _comboContainer;
    private List<IngredientSO> _producingProducts;

    public Vector3 DoorPosition => _door.transform.position;
    public ComboText ComboText => _comboText;
    public bool HasIngredients => _ingredientsContained.Count > 0;
    //public int IngredientlID => _ingredients.Count > 0 ? _ingredients.Peek().ID : -100;
    public List<IngredientSO> ProducingProducts => _producingProducts;
    public List<Ingredient> IngredientsContained => _ingredientsContained;
    public List<IngredientSO> UsingIngredients => _usingIngredients;

    public event UnityAction<Factory> GotIngredient; // replacing class
    public event UnityAction<List<Ingredient>> ReleasedIngredient; // check replacing logic
    public event UnityAction<Factory> Interacted;  // check replacing logic
    public event UnityAction<InGameEvent> OnProcessed;


    // Does these events need for Ingredeint logic?
    protected void DoOnVeryNiceMove() => VeryNiceMove?.Invoke();
    protected void DoOnNiceMove() => NiceMove?.Invoke();
    protected void DoInterracted(Factory factory) => Interacted?.Invoke(factory);
    protected void DoOnProcessed(InGameEvent inGameEvent) => OnProcessed?.Invoke(inGameEvent);

    public event UnityAction NiceMove;
    public event UnityAction VeryNiceMove;
    public event UnityAction BadMove;

    public void OpenDoor() => _door.Open();

    public void PlayConfetti() => _confetti.Play();

    public void CloseDoor() => _door.Close();

    
    private void Start()
    {
        _producingProducts = new List<IngredientSO>();
        _usingIngredients = new List<IngredientSO>();

        foreach(var recepe in _recipes)
        {
            _producingProducts.Add(recepe.FinalProduct);
            _usingIngredients.AddRange(recepe.Ingredients);
        }
    }
    

    private void OnEnable()
    {
        NiceMove += OnNiceMove;
        VeryNiceMove += OnNiceMove;
        BadMove += OnBadMove;

        _factoryPoint.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        NiceMove -= OnNiceMove;
        VeryNiceMove -= OnNiceMove;
        BadMove -= OnBadMove;

        _factoryPoint.gameObject.SetActive(true);
    }

    private void OnNiceMove()
    {
        _comboContainer.AddStreak(transform.position);
    }

    private void OnBadMove()
    {
        _comboContainer.ResetStreak(transform.position);
    }

    public void Init(ComboContainer container)
    {
        _comboContainer = container;
    }

    public virtual bool TryProcessProduct(RecipeSO recipe)
    {
        if (_recipes.Contains(recipe))
        {
            var needIngredients = new List<IngredientSO>();
            var usedIngredients = new List<Ingredient>();
            needIngredients.AddRange(recipe.Ingredients);

            foreach(var curIngredient in _ingredientsContained)
            {
                var usingIngredient = needIngredients.Find(x => x.Name == curIngredient.Name);
                if (usingIngredient!=null)
                {
                    needIngredients.Remove(usingIngredient);
                    usedIngredients.Add(curIngredient);
                }
            }

            if (needIngredients.Count <= 0) // ????? ?? ???????????? ??? ??????????? ???????????
            {
                UseIngredinents(usedIngredients);
                ProduceProduct(recipe.FinalProduct);
                OnProcessed?.Invoke(new InGameEvent(InGameEvenType.Produce, recipe.FinalProduct.Name, 1));
                return true;
            }
            else  // ?????-?? ?????????? ?? ?????
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        /*
        List<Ingredient> preparedIngradients = _ingredientsContained.ToList();

        foreach (var necessaryIngradient in recipe.Ingredients)
        {
            foreach (var ingradient in preparedIngradients)
            {
                if (necessaryIngradient == ingradient)
                {
                    preparedIngradients.Remove(ingradient);
                }
            }
        }

        if (preparedIngradients.Count == 0)
        {
            UseIngredinents(preparedIngradients);
            ProduceProduct(recipe.FinalProduct);

            return true;
        }

        return false;
        */
    }

    public virtual bool TryTakeGroup(List<Node> nodes)
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

        bool inOtherFactory = false;
        Factory[] factories = FindObjectsOfType<Factory>();


        foreach (var factory in factories)
            if (factory != this  && factory._usingIngredients.FindAll(x => x.Name == newIngredients[0].Name).Count > 0);
                inOtherFactory = true;

        bool canUse = _usingIngredients.FindAll(x => x.Name == newIngredients[0].Name).Count > 0;

        //bool sameAnimals = _ingredients.Count == 0 || newIngredients[0].ID == _ingredients[_ingredients.Count - 1].ID;
        StartCoroutine(AddAnimalsLoop(newIngredients, canUse && inOtherFactory == false));

        return canUse;
    }


    protected virtual IEnumerator AddAnimalsLoop(List<Ingredient> newIngredients, bool sameAnimals)
    {
        MoveAnimals(newIngredients.Count * _movePerAnimal);
        float maxDelta = _movePerAnimal * (newIngredients.Count - 1);
        int i = 0;
        foreach (var ingredient in newIngredients)
        {
            _ingredientsContained.Add(ingredient);
            int sideDelta = _ingredientsContained.Count % 2 == 0 ? 1 : -1;
            Vector3 position = transform.position - transform.forward * (2 + maxDelta - i * _movePerAnimal) + transform.right * sideDelta;
            ingredient.MoveToAviary(this, 0.5f, position);
            _promiseTimer.WaitFor(0.3f).Then(() =>
            {
                UpdateCounter(GetSamengredientsInRowCount(), ingredient.CountColor, sameAnimals);
            });
            i++;

            yield return new WaitForSeconds(0.1f);
        }

        _promiseTimer.WaitFor(0.4f).Then(() =>
        {
            _door.Close();
            ReactOnNewIngredients(newIngredients);
        });
    }

    protected void MoveAnimals(float delta)
    {
        foreach (var item in _ingredientsContained)
        {
            _promiseTimer.WaitFor(0.2f).Then(() =>
            {
                item.Go(item.transform.position + transform.forward * -delta, 0.2f);
            });
        }
    }

    private void UpdateCounter(int value, Color color, bool sameAnimals)
    {
        _comboImage.color = color;
        _comboText.QuickReset();
        _comboText.Increase(value);
        if (sameAnimals)
            GotIngredient?.Invoke(this);
    }

    protected virtual void ReactOnNewIngredients(List<Ingredient> newAnimals)
    {
        
        int newAnimalsID = newAnimals[0].ID;
        if (newAnimals.Count != _usingIngredients.Count)
        {
            //if (_ingredientsContained.Where(item => item.ID == newAnimalsID).ToArray().Length == _ingredientsContained.Count)
            if (_usingIngredients.FindAll(x => x.Name == newAnimals[0].Name).Count > 0)
            {
                string animation = newAnimals.Count > 3 ? "spin" : "bounce";
                animation = "bounce";
                foreach (Ingredient animal in _ingredientsContained)
                    animal.PlayAnimation(animation);
                if (newAnimals.Count > 4)
                    VeryNiceMove?.Invoke();
                else if (newAnimals.Count >= 1)
                    NiceMove?.Invoke();
            }
            else
            {
                foreach (Ingredient animal in newAnimals)
                    animal.PlayAnimation("fear");

                int count = GetSamengredientsInRowCount();
                BadMove?.Invoke();
                ReleaseIngredients(count);
            }
        }
        else
        {
            Factory[] factories = FindObjectsOfType<Factory>();
            bool canGet = true;
            foreach (var factory in factories)
                if (factory != this && factory.UsingIngredients.FindAll(x => x.Name == newAnimals[0].Name).Count > 0)
                    canGet = false;

            if (canGet)
            {
                if (newAnimals.Count > 4)
                    VeryNiceMove?.Invoke();
                else if (newAnimals.Count >= 1)
                    NiceMove?.Invoke();
            }
            else
            {
                foreach (Ingredient animal in _ingredientsContained)
                    animal.PlayAnimation("fear");
                Interacted?.Invoke(this);
                BadMove?.Invoke();
                int count = GetSamengredientsInRowCount();
                ReleaseIngredients(count);
            }
        }
        Interacted?.Invoke(this);
        
    }

    private int GetSamengredientsInRowCount()
    {
        if (_ingredientsContained.Count == 0)
            return 0;

        int count = 0;
        int prevID = _ingredientsContained[_ingredientsContained.Count-1].ID;
        foreach (var item in _ingredientsContained)
        {
            if (item.ID == prevID)
                count++;
        }

        return count;
    }

    private void ReleaseIngredients(int count)
    {
        List<Ingredient> animals = new List<Ingredient>();
        for (int i = 0; i < count; i++)
        {
            Ingredient animal = _ingredientsContained[_ingredientsContained.Count - 1];
            _ingredientsContained.Remove(animal);
            animals.Add(animal);
        }

        MoveAnimals(-count * _movePerAnimal);

        OpenDoor();
        _promiseTimer.WaitFor(1f).Then(() =>
        {
            CloseDoor();
        });

        ReleasedIngredient?.Invoke(animals);
        UpdateCounter(GetSamengredientsInRowCount(), _ingredientsContained[_ingredientsContained.Count - 1].CountColor, false);
    }

    private void Update()
    {
        _promiseTimer.Update(Time.deltaTime);
    }


    protected void UseIngredinents(List<Ingredient> ingredients)
    {
        foreach(var ingredient in ingredients)
        {
            _ingredientsContained.Remove(ingredient);
        }

        int lastIndex = ingredients.Count - 1;
        for(int i = lastIndex; i >= 0; i--)
        {
            Destroy(ingredients[i].gameObject);
        }
    }

    private void ProduceProduct(IngredientSO product)
    {
        _producedProducts.Add(product);
    }
}
