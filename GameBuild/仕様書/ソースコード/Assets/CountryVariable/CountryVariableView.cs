using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ï`âÊÇ∑ÇÈÇæÇØÅ@éÛÇØéÊÇ¡ÇΩVariableÇï€éùÅ@RefreshAllÇÕçƒï`âÊÇæÇØ

public class CountryVariableView : MonoBehaviour
{
    [Header("Identity")]
    public UnlockTagKey debatetag;
    public TMP_Text titleText;

    [Header("Condition")]
    public UnlockTagKey revealByUnlock; // ï\é¶èåè

    [Header("Root")]
    public GameObject contentRoot;

    [Header("Pie")]
    public PieChartController pieChart;

    [Header("Debate UI")]
    public TMP_Text debatePointText;
    public TMP_Text debateLevelText;
    public LevelRingView levelRing;

    private CountryVariable variable;

    private void Awake()
    {
        gameObject.SetActive(true);

        if (contentRoot != null)
            contentRoot.SetActive(false);
    }


    

    public void Initialize(UnlockTagKey key)
    {
        debatetag = key;

        revealByUnlock = key;

        Debug.Log($"Init View: {debatetag}");

        variable = CountryVariableManager.Instance.GetByKey(debatetag);

        if (variable == null)
        {
            Debug.LogError($"Variable NOT FOUND: {debatetag}");
        }
        else
        {
            Debug.Log($"Variable FOUND: {variable.displayName}");
        }

        if (variable != null)
            variable.EnsureInitialized();

        TrySubscribe();
        RefreshVisibility();
    }

    public void OnEnable()
    {
        TrySubscribe();
        RefreshVisibility();
    }

    private void OnDisable()
    {
        if (TagManager.Instance != null)
            TagManager.Instance.OnUnlockStateChanged -= RefreshVisibility;

        if (CountryVariableManager.Instance != null)
            CountryVariableManager.Instance.OnVariableUpdated -= OnVariableUpdated;
    }

    void TrySubscribe()
    {
        if (TagManager.Instance == null)
        {
            Debug.Log($"[View {debatetag}] TagManager not ready yet");
            return;
        }

        TagManager.Instance.OnUnlockStateChanged -= RefreshVisibility;
        TagManager.Instance.OnUnlockStateChanged += RefreshVisibility;

        if (CountryVariableManager.Instance != null)
        {
            CountryVariableManager.Instance.OnVariableUpdated -= OnVariableUpdated;
            CountryVariableManager.Instance.OnVariableUpdated += OnVariableUpdated;
        }
    }
    void RefreshVisibility()
    {
       
        if (TagManager.Instance == null)
            return;

        bool unlocked = TagManager.Instance.IsUnlocked(revealByUnlock);



        Debug.Log($"{debatetag} unlocked={unlocked}");

        contentRoot.SetActive(unlocked);


        if (unlocked)
            RefreshAll();
    }

    public void RefreshAll()
    {
        if (variable == null) return;

        if (titleText != null)
            titleText.text = variable.displayName;

        if (pieChart != null)
            pieChart.DrawPie(variable.ToPieData());

        if (debatePointText != null)
            debatePointText.text = $"ãcò_P: {variable.debatePoint}";

        if (debateLevelText != null)
            debateLevelText.text = $"Lv {variable.debateLevel}";

        if (levelRing != null)
            levelRing.SetProgress(variable.GetDebateLevelProgress());
    }


    void OnVariableUpdated(UnlockTagKey key)
    {
        if (!key.Equals(debatetag)) return;
        RefreshAll();
    }
}
  