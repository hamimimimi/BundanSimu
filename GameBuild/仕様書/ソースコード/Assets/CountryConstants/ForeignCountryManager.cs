using UnityEngine;
using System.Collections.Generic;

public class ForeignCountryManager : MonoBehaviour
{
public static ForeignCountryManager Instance { get; private set; }

    private Dictionary<string, ForeignCountryRelationData> dataByKey
        = new();

    void Awake()
    {
        Instance = this;
    }

    public void Register(ForeignCountryRelationData d)
    {
        dataByKey[d.countryName] = d;
    }

    public ForeignCountryRelationData Get(string key)
    {
        dataByKey.TryGetValue(key, out var d);
        return d;
    }

    public IEnumerable<ForeignCountryRelationData> GetAll()
        => dataByKey.Values;
}
