using UnityEngine;

public static class ElectionVariableGenerator
{
    public static void CreateCandidateVariable(
        string districtId,
        string candidateName)
    {
        string baseKey = $"election_{districtId}_{candidateName}";
        string postKey = baseKey + "_post";

        var variable = new CountryVariable
        {
            tag = UnlockTag.Election,
            key = postKey,
            displayName = candidateName,

            yes = 0,
            no = 0,
            neutral = 100,

            debatePoint = 0,
            debateLevel = 1
        };

        CountryVariableManager.Instance.Register(variable);

        // UnlockDefinition
        var def = ScriptableObject.CreateInstance<UnlockDefinition>();

        def.unlockTag = UnlockTag.Election;
        def.unlockKey = baseKey;   // post–³‚µ
        def.title = candidateName;
        def.requiredPoint = 10;
        def.requiredTags = new UnlockTag[0];

        UnlockDefinitionManager.Instance.Register(def);
    }
}