using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class LargeScaleManager : MonoBehaviour
{
    public static LargeScaleManager Instance { get; private set; }

    [SerializeField]
    private List<LargeScaleData> largeScales = new();

    public event Action<LargeScaleData> OnLargeScaleUpdated;

    public LargeScaleData GetByUnlock(UnlockTag tag)
    {
        return largeScales.FirstOrDefault(ls => ls.unlockTag == tag);
    }
   
    private bool IsUnlocked(LargeScaleData ls)
    {
        return TagManager.Instance != null &&
               TagManager.Instance.IsUnlocked(ls.unlockTag);
    }

    
    private void Start()
    {
        RebuildAll();
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

    private void OnEnable()
    {
        TrySubscribe();             
    }

    void TrySubscribe()
    {
        if (CountryVariableManager.Instance == null)
        {
            Invoke(nameof(TrySubscribe), 0.1f);
            return;
        }

        CountryVariableManager.Instance.OnVariableUpdated += OnVariableUpdated;
    }

    private void OnDisable()
    {
        if (CountryVariableManager.Instance != null)
            CountryVariableManager.Instance.OnVariableUpdated -= OnVariableUpdated;       
    }

    private void OnVariableUpdated(UnlockTagKey key)
    {
        foreach (var ls in largeScales)
        {
            if (!ls.IsUnlocked) continue;

            if (!ls.sources.Any(src =>
                 TagMapping.VariableToUnlock(src.sourceTag) == key.tag))
                continue;

            Recalculate(ls);
        }
    }

    private void RebuildAll()
    {
        foreach (var ls in largeScales)
        {
            if (!ls.IsUnlocked) continue;
            Recalculate(ls);
        }
    }

    private void Recalculate(LargeScaleData ls)
    {
        ls.Reset();

        float totalWeight = ls.sources.Sum(s => s.weight);

        foreach (var src in ls.sources)
        {
            Debug.Log($"[LargeScale] watching {src.sourceTag}");
            var unlock = TagMapping.VariableToUnlock(src.sourceTag);
            var variable = CountryVariableManager.Instance.GetByTag(unlock);
            if (variable == null) continue;
            
            if (!TagManager.Instance.IsUnlocked(unlock)) continue;

            float ratio = src.weight / totalWeight;

            int levelMultiplier = variable.debateLevel;

            ls.debatePoint += Mathf.RoundToInt(
                variable.debatePoint * ratio * levelMultiplier
            );

            ls.yes += Mathf.RoundToInt(variable.yes * ratio);
            ls.no += Mathf.RoundToInt(variable.no * ratio);
            ls.neutral += Mathf.RoundToInt(variable.neutral * ratio);


            Debug.Log($"[LargeScale] watching {src.sourceTag}");
        }

        // ãŒÀilŒûj
        
        ls.debatePoint = LargeScaleDebatePointRule.ClampDebatePoint(ls.debatePoint);

        UpdateDebateLevel(ls);

        OnLargeScaleUpdated?.Invoke(ls);
    }


    private void UpdateDebateLevel(LargeScaleData ls)
    {
        int level = 1;
        int required = 1000;
        int remaining = ls.debatePoint;

        while (remaining >= required)
        {
            remaining -= required;
            level++;
            required = Mathf.CeilToInt(required * 1.3f);
        }

        ls.debateLevel = level;
    }
}
