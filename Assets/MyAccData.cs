using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

// MyAcc関係ののデータ置き場


// 各Tagに対する賛成反対中立の絶対量。
[Serializable]
public class OpinionCounts
{
    public int yes;
    public int no;
    public int neutral;

    public int Total => yes + no + neutral;
}

// Tagと賛成反対中立。
[Serializable]
public class OpinionEntry
{
    public string tag;
    public OpinionCounts counts;
}

// Tagの名前
public enum StatusTagType
{
    InfluencePoint,
    CreditRatio,
    RiskLevel,
    InformationPoint,
    Money

}


// MyAccのレベルアップ
[Serializable]
public class StatusTagLevel
{
    public StatusTagType type;
    public int level;

    public StatusTagLevel(StatusTagType type, int level = 0)
    {
        this.type = type;
        this.level = level;
    }

}


// MyAcc のデータ本体

[Serializable]
public class MyAccData
{
    //  Basic 
    public string id;     // 一意ID
    public string name;
 
 
    public List<StatusTagLevel> statusLevels = new();  // レベルアップ用

    public int nextLevelUpInfluence = 1000;  // 影響力によるレベルアップ計算用

    public string info2; // 使ってない。後で何か足せる

    //  Opinion 
    [NonSerialized]
    public Dictionary<string, OpinionCounts> opinionMap
        = new Dictionary<string, OpinionCounts>();

    public List<OpinionEntry> opinionEntries = new();

    //  Status 
    public int influencePoint = 1;
    public float creditRatio = 50f;
    public int RiskLevel;
    public int lastPostTurn = -1;


    //  Constructor 
    public MyAccData(
        string name,
        string info2 = "")
    {
        id = Guid.NewGuid().ToString();
        this.name = name;
        this.info2 = info2;

        statusLevels = new List<StatusTagLevel>()
    {
        new StatusTagLevel(StatusTagType.InfluencePoint),
        new StatusTagLevel(StatusTagType.CreditRatio),
        new StatusTagLevel(StatusTagType.RiskLevel),
        new StatusTagLevel(StatusTagType.InformationPoint),
        new StatusTagLevel(StatusTagType.Money)
    };
    }

    //  Save / Load Listに変換
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

    //  ロード Dictionaryに変換
    public void OnAfterLoad()
    {
        // 古いセーブ対応
        if (opinionEntries == null)
            opinionEntries = new List<OpinionEntry>();
       
        if (statusLevels == null)
           statusLevels = new List<StatusTagLevel>();
       
        opinionMap = new Dictionary<string, OpinionCounts>();

        foreach (var e in opinionEntries)
        {
            if (e != null && e.counts != null && !string.IsNullOrEmpty(e.tag))
            {
                opinionMap[e.tag] = e.counts;
            }
        }
    }
    //  opinionmapがあればそのまま、無ければ作る。 
    public OpinionCounts GetOpinion(string tag)
    {
        if (!opinionMap.TryGetValue(tag, out var counts))
        {
            counts = new OpinionCounts();
            opinionMap[tag] = counts;
        }
        return counts;
    }
   
public int GetStatusLevel(StatusTagType type)
{
    StatusTagLevel status = statusLevels.Find(x => x.type == type);

    return status != null ? status.level : 0;
}


    // レベル増減用関数。
    public void AddStatusLevel(StatusTagType type, int amount)
    {
        StatusTagLevel status = statusLevels.Find(x => x.type == type);

        if (status == null)
        {
            status = new StatusTagLevel(type);
            statusLevels.Add(status);
        }

        status.level += amount;

        // 上限下限
        if (type == StatusTagType.Money)
        {
            status.level = Mathf.Clamp(status.level, 0, 6);
        }
        else
        {
            status.level = Mathf.Clamp(status.level, -3, 3);
        }
    }

    public StatusTagType GetRandomStatusType()
    {
        return (StatusTagType)
            UnityEngine.Random.Range(
                0,
                System.Enum.GetValues(typeof(StatusTagType)).Length);
    }


}
