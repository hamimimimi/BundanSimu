using UnityEngine;
using TMPro;
using System.Linq;

public class CountryConstantView : MonoBehaviour
{
    [Header("Condition")]
    public UnlockTag revealByUnlock;

    [Header("Unlock Key (optional)")]
    [SerializeField] string revealKey;

    [Header("Type")]
    public ConstantViewType viewType;

    [Header("UI")]
    public TMP_Text titleText;
    public TMP_Text bodyText;

    CountryConstantData data;

    [SerializeField] GameObject contentRoot;

    [SerializeField] int partyIndex;

    [SerializeField] int foreignIndex;

    bool subscribed = false;


    void OnDisable()
    {
        if (subscribed && TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged -= Refresh;
            subscribed = false;
        }
    }

    void Awake()
    {
        if (contentRoot != null)
            contentRoot.SetActive(false);
    }

    void OnEnable()
    {
        TrySubscribe();

        Refresh();
    }

    void Update()
     {
      if (!subscribed)
         TrySubscribe();
     }

    void ClearBody()
    {
        bodyText.text = "";
    }

    void AddLine(string line)
    {
        bodyText.text += line + "\n";
    }

     void TrySubscribe()
      {
       if (!subscribed && TagManager.Instance != null)
        {
         TagManager.Instance.OnUnlockStateChanged += Refresh;
         subscribed = true;
         Debug.Log($"Subscribed: {viewType}");
        }
      }


    private void Start()
    {   
        TrySubscribe();
        data = CountryConstantManager.Instance.Data;
        Refresh();
    }

    public void Refresh()
    {
        Debug.Log($"Refresh called: {viewType}");
        Debug.Log($"CHECK: need {revealByUnlock}");

        if (TagManager.Instance == null) return;

        if (data == null)
        {
         data = CountryConstantManager.Instance?.Data;
         if (data == null)
          {
            Debug.LogWarning("Country data not ready");
            return;
          }
        }
        bool unlocked;

        if (string.IsNullOrEmpty(revealKey))
        {
            unlocked = TagManager.Instance.IsUnlocked(revealByUnlock);
        }
        else
        {
            unlocked = TagManager.Instance.IsUnlocked(
       new UnlockTagKey { tag = revealByUnlock, key = revealKey }
   );

        }


        if (contentRoot != null)
            contentRoot.SetActive(unlocked);

        if (!unlocked) return;

        if (data == null)
        {
            data = CountryConstantManager.Instance?.Data;

            if (data == null) 
                {
                    Debug.Log("Data not rady, showing empty view");
                    return;
                }
        }

        Draw();
    }

    void Draw()
    {
        if (data == null)
        {
            Debug.LogError("Country data missing");
        }

        ClearBody();

        switch (viewType)
        {
            case ConstantViewType.Population:
                titleText.text = "人口";
                bodyText.text =
                    $"{data.population:N0} 人\n" +
                    $"規模：{data.sizeCategory}";
                break;

            case ConstantViewType.PopulationPyramid:
                titleText.text = "人口構成";
                bodyText.text =
                    $"{data.pyramidType}\n" +
                    $"若年 {data.youngPercent}% / 成人 {data.adultPercent}% / 高齢 {data.oldPercent}%";
                break;

            case ConstantViewType.Government:
                titleText.text = "政体";
                bodyText.text = data.governmentType.ToString();
                break;

            case ConstantViewType.Ethnic:
                titleText.text = "民族構成";
                bodyText.text = BuildEthnicText();
                break;
            case ConstantViewType.PoliticalPartySummary:
                DrawPartySummary(data);
                break;

            case ConstantViewType.PoliticalPartyPolicies:
                DrawPartyPolicies(data);
                break;

            case ConstantViewType.LandSummary:
                DrawLandSummary(data);
                break;

            case ConstantViewType.ForeignCountryList:
                DrawForeignCountryList(data);
                break;

            case ConstantViewType.ForeignCountryDetail:
                DrawForeignCountryDetail(data);
                break;
        }
    }

    string BuildEthnicText()
    {
        var lines = "";
        foreach (var e in data.ethnicGroups)
            lines += $"{e.name}: {e.percentage}%\n";
        return lines;
    }

    void DrawPartySummary(CountryConstantData data)
    {
        foreach (var party in data.politicalParties)
        {
            var names = string.Join(", ", party.politicians.Select(p => p.name));
            AddLine($"{party.partyName} ({party.supporterCount})\nPoliticians: {names}");
        }
    }

    void DrawPartyPolicies(CountryConstantData data)
    {
        foreach (var party in data.politicalParties)
        {
            var names = string.Join(", ", party.politicians.Select(p => p.name));
            AddLine($"{party.partyName} ({party.supporterCount})");
            AddLine($"Politicians: {names}");
            AddLine(""); // 空行
        }
    }

    void DrawLandSummary(CountryConstantData data)
    {
        AddLine($"Total Land: {data.landAreas.Count}");

        var grouped = data.landAreas
            .GroupBy(a => a.type)
            .Select(g => $"{g.Key}: {g.Count()}");

        foreach (var line in grouped)
            AddLine(line);
    }

    void DrawForeignCountryList(CountryConstantData data)
    {
        Debug.Log(data);
Debug.Log(data.foreignRelations);
        foreach (var rel in data.foreignRelations)
        {
            AddLine(rel.countryName);
        }
    }

    void DrawForeignCountryDetail(CountryConstantData data)
    {
        var r = data.foreignRelations[foreignIndex];
      if (data.foreignRelations == null ||
        foreignIndex < 0 ||
        foreignIndex >= data.foreignRelations.Count)
       {
        AddLine("No data");
        return;
       }
        AddLine($"Country: {r.countryName}");
        AddLine($"Relation: {r.relationType}");
        AddLine($"Power: {r.powerBalance}");
        AddLine($"Territory Issue: {r.hasTerritorialIssue}");
        AddLine($"History Issue: {r.hasHistoricalIssue}");
        AddLine($"Export: {r.exportRatio}");
        AddLine($"Import: {r.importRatio}");
    }

}
