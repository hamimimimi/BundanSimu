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


    public event Action<UnlockTagKey> OnVariableUpdated;

    public event Action<CountryVariable> OnDebateLevelUp;

    public List<VariableSaveData> GetAllSaveData() 
    {
        var list = new List<VariableSaveData>();

    foreach (var v in variablesByKey.Values)
    {
        list.Add(new VariableSaveData
        { 
            tag = v.tag.ToString(),
            key = v.key,
            debatePoint = v.debatePoint,
            debateLevel = v.debateLevel
        });
    }

    return list;
    }

    public void Load(List<VariableSaveData> data) 
    { 
    foreach (var save in data) 
    {
        var key = new UnlockTagKey
        {
            tag = (UnlockTag)Enum.Parse(typeof(UnlockTag), save.tag),
            key = save.key
        };

        var variable = GetByKey(key);

        if (variable == null)
            continue; 

        variable.debatePoint = save.debatePoint;
        variable.debateLevel = save.debateLevel;

        OnVariableUpdated?.Invoke(key);
    }
    }
    


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

    // 外から変更を呼ぶ用
    public void NotifyUpdated(CountryVariable variable)
    {
        OnVariableUpdated?.Invoke(variable.TagKey);
    }

    public void NotifyDebateLevelUp(
    CountryVariable variable)
    {
        OnDebateLevelUp?.Invoke(variable);
    }


    /// <summary>
    /// すべての Variable を取得（拡張用）
    /// </summary>
    public IReadOnlyCollection<CountryVariable> GetAll()
    {
       return variablesByTag.Values;
    }


    public CountryVariable GetByTag(UnlockTag tag)
    {
        return GetByKey(new UnlockTagKey { tag = tag, key = "" });
    }

}