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

    // 互換用（既存コードが残っている間だけ）後で消す
    public bool IsComplete(UnlockTag tag, string key, float required)
    {
        return IsComplete(new UnlockTagKey { tag = tag, key = key }, required);
    }
}
