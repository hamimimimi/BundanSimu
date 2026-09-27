using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemInventoryManager : MonoBehaviour
{
    public static ItemInventoryManager Instance;

    Dictionary<UnlockTagKey, int> items = new();
   
    public event System.Action OnItemChanged; 
    
    public List<ItemSaveData> GetAll() 
    {
          var list = new List<ItemSaveData>();

        foreach (var kv in items)
        {
            list.Add(new ItemSaveData
            {
                tag = kv.Key.tag.ToString(),
                key = kv.Key.key,
                amount = kv.Value
            });
        }

        return list;
    }

    public void Load(List<ItemSaveData> data)
    {
            items.Clear();

        foreach (var save in data)
        {
            var key = new UnlockTagKey
            {
                tag = (UnlockTag)Enum.Parse(typeof(UnlockTag), save.tag),
                key = save.key
            };

            items[key] = save.amount;
        }
    }
    
    void Awake()
    {
        Instance = this;
    }

    public void Add(UnlockTagKey key, int amount)
    {
        if (!items.ContainsKey(key))
            items[key] = 0;

        items[key] += amount;
        OnItemChanged?.Invoke();
    }

    public int Get(UnlockTagKey key)
    {
        return items.TryGetValue(key, out var v) ? v : 0;
    }


}