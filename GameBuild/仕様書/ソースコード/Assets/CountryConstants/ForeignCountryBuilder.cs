using UnityEngine;

public class ForeignCountryBuilder : MonoBehaviour
{
    public static void Build(CountryConstantData data)
    {
        foreach (var rel in data.foreignRelations)
        {
            CreateVariable(rel);
            CreateDefinitions(rel);
        }
    }

    static void CreateVariable(ForeignCountryRelationData rel)
    {
        var variable = new CountryVariable
        {
            tag = UnlockTag.ForeignCountry,
            key = rel.countryName + "_post",
            displayName = rel.countryName,

            yes = 33,
            no = 33,
            neutral = 34
        };

        CountryVariableManager.Instance.Register(variable);
    }

    static void CreateDefinitions(ForeignCountryRelationData rel)
    {
        CreateDef(rel.countryName, "_post");
        CreateDef(rel.countryName, "_view");
    }

    static void CreateDef(string name, string suffix)
    {
        var def = ScriptableObject.CreateInstance<UnlockDefinition>();

        def.unlockTag = UnlockTag.ForeignCountry;
        def.unlockKey = name + suffix;
        def.title = name + suffix;
        def.requiredPoint = 10;

        UnlockDefinitionManager.Instance.Register(def);
    } 
}
