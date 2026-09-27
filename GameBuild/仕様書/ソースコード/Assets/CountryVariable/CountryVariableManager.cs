using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

// データ管理、Optionを保持、Currentを切り替えるだけ　UIを触らない。
[DefaultExecutionOrder(-100)]
public class CountryVariableManager : MonoBehaviour
{
    public static CountryVariableManager Instance { get; private set; }

    private Dictionary<UnlockTagKey, CountryVariable> variablesByKey = new();
    private Dictionary<UnlockTag, CountryVariable> variablesByTag = new();

    // UI が参照する Current
    private CountryVariable current;
    public CountryVariable Current => current;

    public event Action<DebateTag> OnDebateVariableUpdated;

    public event Action<UnlockTagKey> OnVariableUpdated;




    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void Register(CountryVariable variable)
    {
        var key = variable.TagKey;

        variablesByKey[key] = variable;

        if (string.IsNullOrEmpty(variable.key))
            variablesByTag[variable.tag] = variable;
    }
  


    public CountryVariable GetByKey(UnlockTagKey key)
    {
        // ① 完全一致（Tag+Key）
        if (variablesByKey.TryGetValue(key, out var v))
            return v;

        // ② フォールバック（Tagのみ）
        if (variablesByTag.TryGetValue(key.tag, out v))
            return v;

        return null;
    }

    
    


    /// <summary>
    /// すべての Variable を取得（拡張用）
    /// </summary>
    public IReadOnlyCollection<CountryVariable> GetAll()
    {
       return variablesByTag.Values;
    }

   //以下の扱いは保留最終的に消すかも（２０２６年３月４日１７時）

    public void ApplyPost(UnlockTagKey tagKey, CountryVariable.PostChoice choice, int mainAdd,
    int subAdd,
    int neutralAdd)
    {
        var variable = GetByKey(tagKey);
        if (variable == null)
            return;

        int max =
        CountryConstantManager.Instance
            .GetCapValue(variable.capSource);

        variable.ApplyPostAdvanced(choice, mainAdd, subAdd, neutralAdd, max);

        
        variable.UpdateDebateLevel();

        OnVariableUpdated?.Invoke(tagKey);

        // Debate系だけ旧イベントも通知
        if (tagKey.key == "")
        {
            OnDebateVariableUpdated?.Invoke((DebateTag)tagKey.tag);
        }
    }
    public CountryVariable GetByTag(UnlockTag tag)
    {
        return GetByKey(new UnlockTagKey { tag = tag, key = "" });
    }

}