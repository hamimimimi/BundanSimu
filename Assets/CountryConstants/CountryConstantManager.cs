using UnityEngine;
using System;
using System.Linq;

public class CountryConstantManager : MonoBehaviour
{
    public static CountryConstantManager Instance { get; private set; }

    public CountryConstantData Data { get; private set; }

    public event Action OnDataReady;





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



        // Builder é¿çs
        var builders = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
         .OfType<IUnlockDefinitionBuilder>();

        foreach (var b in builders)
        {
            b.Build();
        }

        // Buttonê∂ê¨
        var generator = FindFirstObjectByType<UnlockButtonGenerator>();
        generator.Generate();

        Debug.Log("=== ALL INITIALIZED ===");
    }
             
    
    // åvéZé¿çs
    void Generate()
    {
        Data = CountryGenerator.Generate();
        Debug.Log("CountryConstant generated");

        OnDataReady?.Invoke();
    }
}
