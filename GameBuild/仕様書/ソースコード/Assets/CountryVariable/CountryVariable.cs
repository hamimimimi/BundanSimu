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

    // 議論
    public int debatePoint;
    public int debateLevel;
    public int maxDebatePoint;

    [Header("Debate Cap")]
    public DebateCapSource capSource;
    public float capRatio = 1f;


    // 初期値
    public void EnsureInitialized()
    {}

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

    // 投稿反映（加算量は外から渡す）
    public void ApplyPostAdvanced(
        PostChoice choice,
        int mainAdd,
        int subAdd,
        int neutralAdd,
        int maxDebatePoint)
    {
    
        switch (choice)
        {
            case PostChoice.Yes:
                yes += mainAdd;
                no += subAdd;
                neutral += neutralAdd;
                break;

            case PostChoice.No:
                no += mainAdd;
                yes += subAdd;
                neutral += neutralAdd;
                break;

            case PostChoice.Neutral:
                neutral += mainAdd;
                break;
        }
        int add = mainAdd + subAdd + neutralAdd;
        UpdateDebatePoint(add);

        UpdateDebateLevel();
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

   

    //　DevatePoint計算
    private void UpdateDebatePoint(int add)
    {
        debatePoint += add;
        debatePoint = DebatePointRule.ClampDebatePoint(this);
    }


    //　レベル進捗
    public float GetDebateLevelProgress()
    {
        int required = 1000;
        for (int i = 1; i < debateLevel; i++)
            required = Mathf.CeilToInt(required * 1.3f);

        return Mathf.Clamp01((float)debatePoint / required);
    }

    public void UpdateDebateLevel()
    {
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
    }

}
