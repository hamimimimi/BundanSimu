using UnityEngine;

public class DistrictIssueDefinitionBuilder : MonoBehaviour, IUnlockDefinitionBuilder
{
  public void Build()
    {
        var data = CountryConstantManager.Instance.Data;

        if (data == null || data.districts == null)
        {
            Debug.LogError("districts ‚ª‚È‚¢");
            return;
        }

        foreach (var district in data.districts)
        {
            CreateLeaderPopularityIssue(district);
        }

        Debug.Log("District Issue ’è‹`ì¬Š®—¹");
    }

    void CreateLeaderPopularityIssue(ElectionDistrictData district)
    {
        if (district == null)
        {
            Debug.LogError("district null");
            return;
        }

        if (UnlockDefinitionManager.Instance == null)
        {
            Debug.LogError("UnlockDefinitionManager null");
            return;
        }

        string key = $"{district.districtId}_leader_popularity_post";

        var def = ScriptableObject.CreateInstance<UnlockDefinition>();

        // š Tag‚Í‹¤’Ê
        def.unlockTag = UnlockTag.Issue;

        // š District‚²‚Æ‚Ì¯•Ê‚ÍKey‚Å
        def.unlockKey = key;

        def.title = $"{district.districtId} Leader Popularity";

        def.requiredPoint = 0;
        def.requiredTags = new UnlockTag[0];

        UnlockDefinitionManager.Instance.Register(def);
       
        var variable = new CountryVariable
       {
        tag = UnlockTag.Issue,
        key = key,
        displayName = def.title,
        yes = 40,
            no = 30,
            neutral = 30,

            debatePoint = 0,
            debateLevel = 1
        };

        CountryVariableManager.Instance.Register(variable);
    }  
}
