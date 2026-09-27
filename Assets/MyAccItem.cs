using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

//　MyAccの計算とロジック置き場。（注）余裕があるときに表示・各種計算・UI表示・セーブの役割を分離する。

public class MyAccItem : MonoBehaviour
{
    [Header("Basic UI")]
    public TMP_Text textName;
    public Button btnDelete;// 切除ボタン。仕様は検討中。

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

    [Header("Level UI")]
    [SerializeField] private TMP_Text influenceLevelText;
    [SerializeField] private TMP_Text creditLevelText;
    [SerializeField] private TMP_Text riskLevelTextUI;
    [SerializeField] private TMP_Text informationLevelText;
    [SerializeField] private TMP_Text moneyLevelText;


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


        RefreshAll();
        RefreshPostButtonState();
        RefreshInfluencePoint();
        UpdateCreditRatioText();

    }

    private void Awake()
    {      
        if (btnDelete != null)
        {            
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
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= OnTurnChangedSafe;
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
        SaveManager.Instance.Save(0);
    }

    // CreditRatio（信用率）の計算。0～100。詳しくは仕様書へ 
    void UpdateCreditRatio(string opinionkey, CountryVariable.PostChoice choice)
    {
        const float LOW_CREDIT_BORDER = 25f;
　　　　const float HIGH_CREDIT_BORDER = 75f;

　　　　const float LOW_CREDIT_BONUS = 0.3f;
　　　　const float NORMAL_CREDIT_BONUS = 1.0f;
　　　　const float HIGH_CREDIT_BONUS = 1.5f;

　　　　const float BASE_CREDIT_CHANGE = 2f;

        var c = data.GetOpinion(opinionkey);       
        if (c.Total == 0) return;

        float x = (c.yes - c.no) / (float)c.Total;

        // 信用率補正計算。
        float band =
            data.creditRatio < LOW_CREDIT_BORDER ? LOW_CREDIT_BONUS:
            data.creditRatio < HIGH_CREDIT_BORDER ? NORMAL_CREDIT_BONUS : HIGH_CREDIT_BONUS;

        float t = BASE_CREDIT_CHANGE;
         
        float delta = choice switch
        {
            CountryVariable.PostChoice.Yes => t * (1f - x),
            CountryVariable.PostChoice.No => -t * (1f + x),
            _ => -t * Mathf.Abs(x)  // 絶対値
        };

        // レベル補正
        float bonus = data.GetStatusLevel(StatusTagType.CreditRatio) * 10;

        data.creditRatio = Mathf.Clamp(data.creditRatio + delta * band + bonus, 0f, 100f);// ０～100に収める。
    }

    // Risk（危険度）の計算 少数意見により上昇。詳しくは仕様書へ
    void UpdateRiskLevel(CountryVariable variable, CountryVariable.PostChoice choice)
    {
        bool minority =
            (choice == CountryVariable.PostChoice.Yes && variable.yes < variable.no) ||
            (choice == CountryVariable.PostChoice.No && variable.no < variable.yes);

        if (minority) data.RiskLevel++;
        else if (choice == CountryVariable.PostChoice.Neutral) data.RiskLevel--;

        // レベル補正
        int bonus = data.GetStatusLevel(StatusTagType.RiskLevel) * -5;

        data.RiskLevel = Mathf.Clamp(data.RiskLevel + bonus, 0, 100);
    }

    //  　各種UIを更新 
    void RefreshAll()
    {
        influencePointView.Refresh(data.influencePoint);
        riskLevelText.text = $"Risk: {data.RiskLevel}";
        creditRatioText.text = $"Credit: {Mathf.RoundToInt(data.creditRatio)}";

        RefreshLevelTexts();

        pieChart.DrawPie(ToFlatCounts());
        UpdateRankList();
    }

    // 投稿ボタンの切り替え。
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
    
    // 過去の投稿リストを更新。
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

    // 影響力を更新。
    void RefreshInfluencePoint()
    {
        if (influencePointView == null || data == null)
            return;

        influencePointView.Refresh(data.influencePoint);
    }
    // 信用率テキスト。
    void UpdateCreditRatioText()
    {
        if (creditRatioText == null || data == null)
            return;

        creditRatioText.text = $"Credit: {Mathf.RoundToInt(data.creditRatio)}";
    }
    // 危険度テキスト
    void RefreshRiskLevel()
    {
        if (riskLevelText == null || data == null)
            return;

        riskLevelText.text = $"Risk: {data.RiskLevel}";
    }
    // 投稿時に更新。
    public void OnPosted()
    {
        RefreshInfluencePoint();
        UpdateCreditRatioText();
        RefreshRiskLevel();
    }

    //  Influence 
    void ApplyInfluenceGain()
    {
        int delta = CalculateInfluenceDelta(data.influencePoint);
        
        // レベル補正
        int bonus = data.GetStatusLevel(StatusTagType.InfluencePoint) * 100;

        data.influencePoint = Mathf.Max(1, data.influencePoint + delta + bonus);

        // レベルアップ確認
        LevelUpManager.Instance.CheckLevelUp(this);
    }

    // 影響力計算。細かい数値については要調整。
    int CalculateInfluenceDelta(int currentValue)
    {
        float r = Random.value;

        if (r < 0.01f)          // GreatSuccess 1%
            return Random.Range(800, 1001);

        if (r < 0.79f)          // Success 78%
            return Random.Range(100, 301);

        if (r < 0.99f)          // Failure 20%
            return Random.Range(-50, 51);

        // GreatFailure 1%

        StatusTagType randomType =
        data.GetRandomStatusType();
        // デメリット付与
        data.AddStatusLevel(randomType, -1);

        return -500;
    }

    // レベルアップ更新
    public void RefreshLevelTexts()
    {
        if (data == null)
            return;

        influenceLevelText.text =
            data.GetStatusLevel(StatusTagType.InfluencePoint).ToString();

        creditLevelText.text =
            data.GetStatusLevel(StatusTagType.CreditRatio).ToString();

        riskLevelTextUI.text =
            data.GetStatusLevel(StatusTagType.RiskLevel).ToString();

        informationLevelText.text =
            data.GetStatusLevel(StatusTagType.InformationPoint).ToString();

        moneyLevelText.text =
            data.GetStatusLevel(StatusTagType.Money).ToString();
    }



}
