using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LargeScaleView : MonoBehaviour
{
    [SerializeField]
    private UnlockTag targetUnlock;

    [Header("UI Root")]
    [SerializeField]
    private GameObject contentRoot;

    [Header("Pie")]
    [SerializeField]
    private PieChartController pieChart;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text valueText;

    private LargeScaleData data;

    private bool subscribed = false;

    void Awake()
    {
        if (contentRoot != null)
            contentRoot.SetActive(false);
    }

    void OnEnable()
    {
        TrySubscribe();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    void TrySubscribe()
    {
        if (subscribed)
            return;

        if (TagManager.Instance == null || LargeScaleManager.Instance == null)
        {
            // èâä˙âªë“Çø
            return;
        }

        TagManager.Instance.OnUnlockStateChanged += RefreshVisibility;
        LargeScaleManager.Instance.OnLargeScaleUpdated += OnLargeScaleUpdated;

        subscribed = true;

        // Åö çwì«Ç≈Ç´ÇΩèuä‘Ç…àÍìxÇæÇØï]âø
        RefreshVisibility();
    }

    void Unsubscribe()
    {
        if (!subscribed)
            return;

        if (TagManager.Instance != null)
            TagManager.Instance.OnUnlockStateChanged -= RefreshVisibility;

        if (LargeScaleManager.Instance != null)
            LargeScaleManager.Instance.OnLargeScaleUpdated -= OnLargeScaleUpdated;

        subscribed = false;
    }

    void Update()
    {
        // Åö TagManager / LargeScaleManager Ç™å„ê∂ê¨ÇÃèÍçáÇ…îıÇ¶ÇÈ
        if (!subscribed)
            TrySubscribe();
    }

    void RefreshVisibility()
    {
        
        if (TagManager.Instance == null)
            return;

        bool unlocked = TagManager.Instance.IsUnlocked(targetUnlock);

        

        if (contentRoot != null)
            contentRoot.SetActive(unlocked);

        if (!unlocked)
            return;
        
        if (data == null)        
            data = LargeScaleManager.Instance.GetByUnlock(targetUnlock);
  
        RefreshAll();
    }

    void OnLargeScaleUpdated(LargeScaleData updated)
    {
        if (updated != data)
            return;

        RefreshAll();
    }

    void RefreshAll()
    {
        if (data == null)
            return;

        if (titleText != null)
            titleText.text = data.displayName;

        if (valueText != null)
            valueText.text =
                $"DP: {data.debatePoint}\n" +
                $"Lv: {data.debateLevel}";

        if (pieChart != null)
        {
            pieChart.DrawPie(data.ToPieData());
        }
    }
}
