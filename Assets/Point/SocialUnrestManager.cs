using UnityEngine;
using System.Collections.Generic;

public class SocialUnrestManager : MonoBehaviour
{
    private HashSet<UnlockTagKey> triggered = new HashSet<UnlockTagKey>();

    void Start()
    {
        CountryVariableManager.Instance.OnVariableUpdated += OnVariableUpdated;
    }

    void OnVariableUpdated(UnlockTagKey key)
    {
        var variable = CountryVariableManager.Instance.GetByKey(key);

        if (variable == null) return;

        // Lv3以上で未発生なら社会不安
        if (variable.debateLevel >= 3 && !triggered.Contains(key))
        {
            triggered.Add(key);
            TriggerSocialUnrest(variable);
        }
    }

    void TriggerSocialUnrest(CountryVariable variable)
    {
        Debug.Log($"社会不安発生: {variable.displayName}");

        // 既存 AnnounceView 使用
        AnnounceEventViewController.Instance.AddEvent(
            $"社会不安が発生: {variable.displayName}"
        );

        // デモ生成
        DemoManager.Instance.ScheduleDemo(variable);
    }
}