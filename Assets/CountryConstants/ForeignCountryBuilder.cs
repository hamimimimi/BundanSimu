using UnityEngine;


// 関係国生成。

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


    // Definition生成。2種類情報表示用のView、Variable用のPost
    static void CreateVariable(ForeignCountryRelationData rel)
    {
        CountryVariableFactory.Create(
    UnlockTag.ForeignCountry,
    rel.countryName + "_post",
    rel.countryName);
    }

    static void CreateDefinitions(ForeignCountryRelationData rel)
    {
        CreateDef(rel.countryName, "_post");
        CreateDef(rel.countryName, "_view");
    }

    // UnlockButton2種類生成。
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
