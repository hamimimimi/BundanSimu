using System;
using UnityEngine;
using System.Collections.Generic;

//　数値変化とレベル計算

[Serializable]
public class CountryVariable
{
    public UnlockTag tag;   // DebateEconomy / PoliticalParty / etc
    public string key;      // "" = Tagのみ / partyId など

    
    public string displayName;
    
    // 世論（合計100）
    public int yes;
    public int no;
    public int neutral;

    // 上限。
    public int cap;

    // 議論Point
    public int debatePoint;
    public int debateLevel;



    // 初期値
    public void InitializeOpinion()
    {
        yes = Mathf.RoundToInt(cap * 0.03f);
        no = Mathf.RoundToInt(cap * 0.03f);
        neutral = cap - yes - no;
    }

    public UnlockTagKey TagKey =>
       new UnlockTagKey { tag = tag, key = key };


    public bool IsMatch(UnlockTagKey k)
    {
        // 完全一致
        if (k.tag == tag && k.key == key) return true;

        // フォールバック：Tagのみ Variable
        if (string.IsNullOrEmpty(key) && k.tag == tag) return true;

        return false;
    }


    public Dictionary<string, int> ToPieData()
    {
        return new Dictionary<string, int>
        {
            { "Yes", yes },
            { "No", no },
            { "Neutral", neutral }
        };
    }

    public enum PostChoice { Yes, No, Neutral }

    // 初期設定
    public void Initialize()
    {
        debatePoint = 0;
        debateLevel = 1;

        InitializeOpinion();
    }

    //　DevatePoint計算
    public void UpdateDebatePoint(int add)
    {
        debatePoint += add;

    }


    //　レベル進捗
    public float GetDebateLevelProgress()
    {
        int required = 1000;
        for (int i = 1; i < debateLevel; i++)
            required = Mathf.CeilToInt(required * 1.3f);

        return Mathf.Clamp01((float)debatePoint / required);
    }


    public bool UpdateDebateLevel()
    {
        int oldLevel = debateLevel;

        int level = 1;
        int required = 1000;
        int remaining = debatePoint;

        while (remaining >= required)
        {
            remaining -= required;
            level++;
            required = Mathf.CeilToInt(required * 1.3f);
        }

        debateLevel = level;

        return oldLevel != debateLevel;
    }



    // 上限解放
    public void AddCap(int amount)
    {
        int ratio = Mathf.Min(debateLevel, 30);

        int addYes = Mathf.RoundToInt(amount * ratio / 100f);
        int addNo = Mathf.RoundToInt(amount * ratio / 100f);
        int addNeutral = amount - addYes - addNo;

        yes += addYes;
        no += addNo;
        neutral += addNeutral;

        cap += amount;
    }
}
