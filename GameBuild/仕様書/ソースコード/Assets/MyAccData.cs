using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

// MyAccのデータ置き場
#region Opinion Structures

[Serializable]
public class OpinionCounts
{
    public int yes;
    public int no;
    public int neutral;

    public int Total => yes + no + neutral;
}

[Serializable]
public class OpinionEntry
{
    public string tag;
    public OpinionCounts counts;
}

#endregion

// MyAcc のデータ本体

[Serializable]
public class MyAccData
{
    // ===== Basic =====
    public string id;     // 一意ID
    public string name;
    public List<string> tags;  
    public string info2; // 使ってない。後で何か足せる

    // ===== Opinion =====
    [NonSerialized]
    public Dictionary<string, OpinionCounts> opinionMap
        = new Dictionary<string, OpinionCounts>();

    public List<OpinionEntry> opinionEntries = new();

    // ===== Status =====
    public int influencePoint = 1;
    public float creditRatio = 50f;
    public int RiskLevel;
    public int lastPostTurn = -1;


    // ===== Constructor =====
    public MyAccData(string name, List<string> tags, string info2 = "")
    {
        id = Guid.NewGuid().ToString();
        this.name = name;
        this.tags = tags;
        this.info2 = info2;

    }

    // ===== Save / Load =====
    public void OnBeforeSave()
    {
        opinionEntries.Clear();
        foreach (var kv in opinionMap)
        {
            opinionEntries.Add(new OpinionEntry
            {
                tag = kv.Key,
                counts = kv.Value
            });
        }
    }

    // ===== ロード =====
    public void OnAfterLoad()
    {
        // 古いセーブ対応
        if (opinionEntries == null)
            opinionEntries = new List<OpinionEntry>();

        opinionMap = new Dictionary<string, OpinionCounts>();

        foreach (var e in opinionEntries)
        {
            if (e != null && e.counts != null && !string.IsNullOrEmpty(e.tag))
            {
                opinionMap[e.tag] = e.counts;
            }
        }
    }
    // ===== Utility =====
    public OpinionCounts GetOpinion(string tag)
    {
        if (!opinionMap.TryGetValue(tag, out var counts))
        {
            counts = new OpinionCounts();
            opinionMap[tag] = counts;
        }
        return counts;
    }
   
}
