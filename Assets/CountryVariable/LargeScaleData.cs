using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LargeScaleSource
{
    public DebateTag sourceTag;
    public float weight;   // 1, 0.25, 3 など
}

[Serializable]
public class LargeScaleData
{
    public string displayName;
    public UnlockTag unlockTag;

    [Header("Sources")]
    public List<LargeScaleSource> sources = new();

    [Header("Values")]
    public int debatePoint;
    public int yes;
    public int no;
    public int neutral;

    public int debateLevel;

    public bool IsUnlocked =>
        TagManager.Instance != null &&
        TagManager.Instance.IsUnlocked(unlockTag);

    public void Reset()
    {
        debatePoint = 0;
        yes = 0;
        no = 0;
        neutral = 0;
        debateLevel = 1;
    }

    /// <summary>
    /// View 用：円グラフデータ
    /// </summary>
    public Dictionary<string, int> ToPieData()
    {
        return new Dictionary<string, int>
        {
            { "Yes", yes },
            { "No", no },
            { "Neutral", neutral }
        };
    }

}
