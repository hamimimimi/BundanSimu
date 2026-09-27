using UnityEngine;

public class IssueGenerator : MonoBehaviour
{
 public static void Generate(CountryConstantData data)
    {
        foreach (var rel in data.foreignRelations)
        {
            // 30%‚Å—Ì“y–â‘è”­¶
            if (Random.value < 0.3f)
            {
                CreateTerritorialIssue(rel.countryName);
            }
        }
    }

    static void CreateTerritorialIssue(string country)
    {
        string baseKey = $"territorial_{country}";
        string postKey = baseKey + "_post";

        var variable = new CountryVariable
        {
            tag = UnlockTag.Issue,
            key = postKey,
            displayName = $"Territorial Issue: {country}",

            yes = 40,
            no = 30,
            neutral = 30,

            debatePoint = 0,
            debateLevel = 1
        };

        CountryVariableManager.Instance.Register(variable);

        // UnlockDefinition
        var def = ScriptableObject.CreateInstance<UnlockDefinition>();

    def.unlockTag = UnlockTag.Issue;
    def.unlockKey = baseKey;          // © post–³‚µ
    def.title = $"Territorial Issue: {country}";
    def.requiredPoint = 10;
    def.requiredTags = new UnlockTag[0];

    UnlockDefinitionManager.Instance.Register(def);

    Debug.Log("Territorial Issue created: " + baseKey);
    }  
}
