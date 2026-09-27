using UnityEngine;
using System;
using System.Linq;

public class CountryConstantManager : MonoBehaviour
{
    public static CountryConstantManager Instance { get; private set; }

    public CountryConstantData Data { get; private set; }

    public event Action OnDataReady;

    public int GetCapValue(DebateCapSource source)
    {
        return source switch
        {
            DebateCapSource.Population => (int)Data.population,
            DebateCapSource.YoungPopulation => (int)Data.youngPopulation,
            
            _ => (int)Data.population
        };
    }


    public int GetLargeScaleMaxDebatePoint()
    {
        if (Data == null)
            return int.MaxValue;

        return (int)Data.population;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
       Generate();

        // DefinitionManager èâä˙âª
        var defManager = FindObjectOfType<UnlockDefinitionManager>();

        // Builder é¿çs
        var builders = FindObjectsOfType<MonoBehaviour>()
         .OfType<IUnlockDefinitionBuilder>();

        foreach (var b in builders)
        {
            b.Build();
        }

        // Buttonê∂ê¨
        var generator = FindObjectOfType<UnlockButtonGenerator>();
        generator.Generate();

        Debug.Log("=== ALL INITIALIZED ===");
    }
             
    

    void Generate()
    {
        Data = CountryGenerator.Generate();
        Debug.Log("CountryConstant generated");

        OnDataReady?.Invoke();
    }
}
