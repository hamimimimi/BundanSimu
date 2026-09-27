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

        CountryVariableFactory.Create(
    UnlockTag.Issue,
    postKey,
    $"Territorial Issue: {country}");

        // UnlockDefinition
        var def = ScriptableObject.CreateInstance<UnlockDefinition>();

    def.unlockTag = UnlockTag.Issue;
    def.unlockKey = postKey;          // © post–³‚µ
    def.title = $"Territorial Issue: {country}";
    def.requiredPoint = 10;
    def.requiredTags = new UnlockTag[0];

    UnlockDefinitionManager.Instance.Register(def);

    Debug.Log("Territorial Issue created: " + baseKey);
    }  
}
