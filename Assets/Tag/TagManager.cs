using UnityEngine;
using System;
using System.Collections.Generic;

public class TagManager : MonoBehaviour
{
    public static TagManager Instance { get; private set; }
  

    // Tag + Key ‰ğ•úi­“}EŠÖŒW‘‚È‚ÇjTag ’P‘Ì‰ğ•ú‚ÍKey‚ğhh
    private Dictionary<UnlockTag, HashSet<string>> unlockedKeys = new();

    public event Action OnUnlockStateChanged;


    private void Awake()
    {
       
        if (Instance != null)
        {
           
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Unlock(UnlockTag tag, string key = "")
    {
        Debug.Log($"UNLOCK: {tag} / {key}");

        if(!unlockedKeys.TryGetValue(tag, out var set))
        {
            set = new HashSet<string>();
            unlockedKeys[tag] = set;
        }
        if (set.Add(key))
            OnUnlockStateChanged?.Invoke();

    }
   
    public bool IsUnlocked(UnlockTag tag)
      => IsUnlocked(new UnlockTagKey { tag = tag, key = "" });

    public bool IsUnlocked(UnlockTagKey key)
    {
        // ‡@ Š®‘Sˆê’v
        if (unlockedKeys.TryGetValue(key.tag, out var set)
            && set.Contains(key.key))
            return true;

        // ‡A Tag‚Ì‚İ‰ğ•ú‚³‚ê‚Ä‚¢‚ê‚Î true
        if (key.key != ""
            && unlockedKeys.TryGetValue(key.tag, out set)
            && set.Contains(""))
            return true;

        return false;
    }

    public List<UnlockTagKey> GetUnlockedTagKeys()
    {
        var list = new List<UnlockTagKey>();

        foreach (var pair in unlockedKeys)
        {
            foreach (var key in pair.Value)
            {
                list.Add(new UnlockTagKey
                {
                    tag = pair.Key,
                    key = key
                });
            }
        }
        return list;
    }
    
    public List<UnlockTagSaveData> GetSaveData()
{
    var list = new List<UnlockTagSaveData>();

    foreach (var pair in unlockedKeys)
    {
        list.Add(new UnlockTagSaveData
        {
            tag = pair.Key,
            keys = new List<string>(pair.Value)
        });
    }

    return list;
}
public void Load(List<UnlockTagSaveData> data)
{
    unlockedKeys.Clear();

    if (data == null) return;

    foreach (var d in data)
    {
        unlockedKeys[d.tag] = new HashSet<string>(d.keys);
    }

    OnUnlockStateChanged?.Invoke();
}

}
