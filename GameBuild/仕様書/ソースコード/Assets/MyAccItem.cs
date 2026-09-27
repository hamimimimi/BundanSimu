using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

//　MyAccの計算とロジック置き場

public class MyAccItem : MonoBehaviour
{
    [Header("Basic UI")]
    public TMP_Text textName;
    public TMP_Text tagsText;
    public Button btnDelete;

    [Header("Post UI")]
    public Button postButton;

    [Header("Charts")]
    public PieChartController pieChart;

    [Header("Rank UI")]
    public Transform rankRoot;
    public GameObject rankItemPrefab;

    [Header("Status UI")]
    public InfluencePointView influencePointView;
    public TMP_Text riskLevelText;
    [SerializeField] private TMP_Text creditRatioText;

    [Header("UI")]
    public TMP_Dropdown postTagDropdown;

    private MyAccData data;
    private System.Action<string> onDeleteCallback;

    public MyAccData Data => data;
  
// Setup を Action<string> を受け取る形にする
    public void Setup(MyAccData d, System.Action<string> onDelete)
    {
        data = d;
        onDeleteCallback = onDelete;

        if (textName != null)
            textName.text = data.name;

        if (tagsText != null && data.tags != null)
            tagsText.text = string.Join(" / ", data.tags);

        RefreshAll();
        RefreshPostButtonState();
        RefreshInfluencePoint();
        UpdateCreditRatioText();

    }

    private void Awake()
    {
        
       

        if (btnDelete != null)
        {
            // ボタンのリスナーは Setup で上書きするので、ここでは登録しない／安全のためクリア
            btnDelete.onClick.RemoveAllListeners();
            btnDelete.onClick.AddListener(() =>
            {
                if (onDeleteCallback != null && data != null) onDeleteCallback.Invoke(data.id);
            });
        }
    }

    private void OnEnable()
    {
        TurnManager.OnTurnChanged += OnTurnChangedSafe;

        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged += RefreshTagDropdown;
            RefreshTagDropdown(); 
        }
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= OnTurnChangedSafe;

        if (TagManager.Instance != null)
            TagManager.Instance.OnUnlockStateChanged -= RefreshTagDropdown;
    }

    private void OnTurnChangedSafe()
    {
        RefreshPostButtonState();
    }


    // PostButtonController から呼ばれる
    public void ApplyPost(UnlockTagKey tagKey, CountryVariable.PostChoice choice)
    {
        

        if (data.lastPostTurn == TurnManager.Instance.CurrentTurn)
            return;

        string opinionKey = tagKey.ToString();

        var c = data.GetOpinion(opinionKey);

        
        switch (choice)
        {
            case CountryVariable.PostChoice.Yes: c.yes++; break;
            case CountryVariable.PostChoice.No: c.no++; break;
            case CountryVariable.PostChoice.Neutral: c.neutral++; break;
        }

        
        ApplyInfluenceGain();

        
        CountryVariable variable = null;
        if (tagKey.tag == UnlockTag.DebateEconomy ||
            tagKey.tag == UnlockTag.DebateMilitary ||
            tagKey.tag == UnlockTag.DebateEnvironment)
        {
            variable = CountryVariableManager.Instance.GetByTag(tagKey.tag);
        }

        if (variable != null)
        {
            UpdateRiskLevel(variable, choice);
        }

        UpdateCreditRatio(opinionKey, choice);

        // UI
        RefreshAll();

        data.lastPostTurn = TurnManager.Instance.CurrentTurn;
        SaveMyAccSystem.SaveAll();
    }

    // ===== CreditRatio =====
    void UpdateCreditRatio(string opinionkey, CountryVariable.PostChoice choice)
    {
        var c = data.GetOpinion(opinionkey);       
        if (c.Total == 0) return;

        float x = (c.yes - c.no) / (float)c.Total;

        float band =
            data.creditRatio < 25f ? 0.3f :
            data.creditRatio < 75f ? 1.0f : 1.5f;

        float t = 2.0f;
        float delta = choice switch
        {
            CountryVariable.PostChoice.Yes => t * (1f - x),
            CountryVariable.PostChoice.No => -t * (1f + x),
            _ => -t * Mathf.Abs(x)
        };

        data.creditRatio = Mathf.Clamp(data.creditRatio + delta * band, 0f, 100f);
    }

    // ===== Risk =====
    void UpdateRiskLevel(CountryVariable variable, CountryVariable.PostChoice choice)
    {
        bool minority =
            (choice == CountryVariable.PostChoice.Yes && variable.yes < variable.no) ||
            (choice == CountryVariable.PostChoice.No && variable.no < variable.yes);

        if (minority) data.RiskLevel++;
        else if (choice == CountryVariable.PostChoice.Neutral) data.RiskLevel--;

        data.RiskLevel = Mathf.Clamp(data.RiskLevel, 0, 100);
    }

    // ===== UI =====
    void RefreshAll()
    {
        influencePointView.Refresh(data.influencePoint);
        riskLevelText.text = $"Risk: {data.RiskLevel}";
        creditRatioText.text = $"Credit: {Mathf.RoundToInt(data.creditRatio)}";

        pieChart.DrawPie(ToFlatCounts());
        UpdateRankList();
    }

    void RefreshPostButtonState()
    {
        if (postButton == null)
            return;

        if (data == null)
            return;

        if (TurnManager.Instance == null)
            return;
        postButton.interactable =
            data.lastPostTurn < TurnManager.Instance.CurrentTurn;
    }

    Dictionary<string, int> ToFlatCounts()
    {
        var dict = new Dictionary<string, int>();

        foreach (var kv in data.opinionMap)
        {
            dict[$"{kv.Key}-Yes"] = kv.Value.yes;
            dict[$"{kv.Key}-No"] = kv.Value.no;
            dict[$"{kv.Key}-Neutral"] = kv.Value.neutral;
        }
        return dict;
    }

    void UpdateRankList()
    {
        foreach (Transform child in rankRoot)
            Destroy(child.gameObject);

        var flat = ToFlatCounts();
        int total = data.opinionMap.Values
    .Sum(c => c.yes + c.no + c.neutral);
        if (total == 0) return;

        int index = 1;
        foreach (var kv in flat.OrderByDescending(k => k.Value))
        {
            float percent = kv.Value / (float)total * 100f;
            var item = Instantiate(rankItemPrefab, rankRoot);

            item.transform.Find("IndexText").GetComponent<TMP_Text>().text = index.ToString();
            item.transform.Find("NameText").GetComponent<TMP_Text>().text = kv.Key;
            item.transform.Find("PercentText").GetComponent<TMP_Text>().text =
                $"{Mathf.RoundToInt(percent)}%";

            item.transform.Find("ColorImage").GetComponent<Image>().color =
                pieChart.GetColorForTag(kv.Key);

            index++;
        }
    }

    void RefreshInfluencePoint()
    {
        if (influencePointView == null || data == null)
            return;

        influencePointView.Refresh(data.influencePoint);
    }
    void UpdateCreditRatioText()
    {
        if (creditRatioText == null || data == null)
            return;

        creditRatioText.text = $"Credit: {Mathf.RoundToInt(data.creditRatio)}";
    }
    void RefreshRiskLevel()
    {
        if (riskLevelText == null || data == null)
            return;

        riskLevelText.text = $"Risk: {data.RiskLevel}";
    }

    public void OnPosted()
    {
        RefreshInfluencePoint();
        UpdateCreditRatioText();
        RefreshRiskLevel();
    }

    // ===== Influence =====
    void ApplyInfluenceGain()
    {
        int delta = CalculateInfluenceDelta(data.influencePoint);
        data.influencePoint = Mathf.Max(1, data.influencePoint + delta);
    }

    int CalculateInfluenceDelta(int currentValue)
    {
        float r = Random.value;

        if (r < 0.01f)          // GreatSuccess 1%
            return Random.Range(1000, 2001);

        if (r < 0.79f)          // Success 78%
            return Random.Range(100, 301);

        if (r < 0.99f)          // Failure 20%
            return Random.Range(-50, 51);

        // GreatFailure 1%
        return -500;
    }

    void RefreshTagDropdown()
    {
        if (postTagDropdown == null) return;

        postTagDropdown.ClearOptions();

        var options = TagManager.Instance
            .GetUnlockedTagKeys()
            .Select(tk => tk.ToString())
            .ToList();

        postTagDropdown.AddOptions(options);
    }

}
