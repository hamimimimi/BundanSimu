using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ElectionDistrictViewController : MonoBehaviour
{   
    public static ElectionDistrictViewController Instance;
    
    public Transform parent; // ï\é¶êÊ
    public ElectionDistrictView prefab;

    Dictionary<string, ElectionDistrictView> views = new();
   
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        TagManager.Instance.OnUnlockStateChanged += Refresh;

        Refresh();
    }

    void OnDisable()
    {
        TagManager.Instance.OnUnlockStateChanged -= Refresh;
    }

    void Refresh()
    {
        //  ä˘ë∂ÇëSïîçÌèú
        foreach (Transform child in parent)
        
            Destroy(child.gameObject);
        
        views.Clear();

        var unlocked = TagManager.Instance.GetUnlockedTagKeys();

        foreach (var key in unlocked)
        {
           
            if (key.tag != UnlockTag.District) continue;
           
            CreateIfNeeded(key);
        }
    }

    void CreateIfNeeded(UnlockTagKey key)
    {     
        var data = CountryConstantManager.Instance.Data;

        var district = data.districts
            .FirstOrDefault(d => d.districtId == key.key);

        if (district == null) return;

        var view = Instantiate(prefab, parent);

        view.Setup(district);
        views[district.districtId] = view;
    } 
    

    public void RefreshDistrict(ElectionDistrictData district)
    {
        if (!views.ContainsKey(district.districtId)) return;

        views[district.districtId].Setup(district);
    }
}
