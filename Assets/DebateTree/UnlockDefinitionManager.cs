using System.Collections.Generic;
using UnityEngine;
using System;

public class UnlockDefinitionManager : MonoBehaviour
{
    public static UnlockDefinitionManager Instance { get; private set; }

    List<UnlockDefinition> definitions = new();


    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadAllDefinitions();
    }
    //@‰Šú¶¬
    public void LoadAllDefinitions()
    {
        var loaded = Resources.LoadAll<UnlockDefinition>("UnlockDefinitions");

        definitions.Clear();
        definitions.AddRange(loaded);

        Debug.Log("Loaded UnlockDefinitions: " + definitions.Count);

        foreach (var d in definitions)
        {
            Debug.Log($"Definition Loaded : {d.unlockTag} / {d.unlockKey}");
        }
    }

    public void Register(UnlockDefinition def)
    {
        definitions.Add(def);
    }

    public IReadOnlyList<UnlockDefinition> GetAll()
    {
        return definitions;
    }

    // ’P‘ÌDefinitionæ“¾

    public UnlockDefinition Get(UnlockTag tag, string key)
    {
      foreach (var d in definitions)
      {
        if (d.unlockTag == tag && d.unlockKey == key)
            return d;
      }
      return null;
     }

    // ƒQ[ƒ€’†¶¬
    public UnlockDefinition CreateDynamicDefinition(
    UnlockTag tag,
    string key,
    string title,
    string description)
{
    var def = ScriptableObject.CreateInstance<UnlockDefinition>();

    def.unlockTag = tag;
    def.unlockKey = key;
    def.title = title;
    def.description = description;

    def.requiredPoint = 0;
    def.requiredTags = new UnlockTag[0];
    def.keyVariants = null;

    Register(def);

    return def;
}

public bool Exists(UnlockTag tag, string key)
{
    foreach (var d in definitions)
    {
        if (d.unlockTag == tag && d.unlockKey == key)
            return true;
    }
    return false;
}
}
