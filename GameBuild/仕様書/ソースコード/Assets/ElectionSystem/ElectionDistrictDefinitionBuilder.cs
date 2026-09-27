using UnityEngine;

public class ElectionDistrictDefinitionBuilder : MonoBehaviour, IUnlockDefinitionBuilder
{
    [SerializeField] int baseCost = 10;


    public void Build()
    {
       
        var data = CountryConstantManager.Instance.Data;

        if (data == null || data.districts == null)
        {
            Debug.LogError("districts Ç™ë∂ç›ÇµÇ»Ç¢");
            return;
        }

        foreach (var districts in data.districts)
        {
        
            // ï\é¶óp
            Create(districts);
            
        }
        Debug.Log("íËã`í«â¡ÇµÇΩ");
    }

    void Create(ElectionDistrictData district)
    {
        if (district == null)
        {
            Debug.LogError("districty is null!");
        }

        if (UnlockDefinitionManager.Instance == null)
        {
            Debug.LogError("UnlockDefinitionManager is null!");
            return;
        }

        var def = ScriptableObject.CreateInstance<UnlockDefinition>();

        def.unlockTag = UnlockTag.District;
        def.unlockKey = district.districtId;           //  string ID
        def.title = district.districtId;
        def.requiredPoint = baseCost;
        def.requiredTags = new UnlockTag[0];


        UnlockDefinitionManager.Instance.Register(def);
    }
}
