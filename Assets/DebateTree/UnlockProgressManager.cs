using System.Collections.Generic;
using UnityEngine;

public class UnlockProgressManager : MonoBehaviour
{
    public static UnlockProgressManager Instance;

    private Dictionary<UnlockTagKey, float> progress = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public float GetProgress(UnlockTagKey key)
    {
        progress.TryGetValue(key, out float value);
        return value;
    }

    public void AddProgress(UnlockTagKey key, float amount)
    {
        if (!progress.ContainsKey(key))
            progress[key] = 0f;

        progress[key] += amount;
    }

    public bool IsComplete(UnlockTagKey key, float required)
    {
        return GetProgress(key) >= required;
    }


public List<UnlockProgressSaveData> GetSaveData()
{
    var list = new List<UnlockProgressSaveData>();

    foreach (var kv in progress)
    {
        list.Add(new UnlockProgressSaveData
        {
            tag = kv.Key.tag,
            key = kv.Key.key,
            progress = kv.Value
        });
    }

    return list;
}

public void Load(List<UnlockProgressSaveData> data)
{
    progress.Clear();

    if (data == null) return;

    foreach (var d in data)
    {
        var key = new UnlockTagKey
        {
            tag = d.tag,
            key = d.key
        };

        progress[key] = d.progress;
    }
}
}
