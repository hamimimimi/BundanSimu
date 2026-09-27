using UnityEngine;
using System.Linq;

public class ElectionDistrictViewController : MonoBehaviour
{
    public Transform parent; // •\¦æ
    public ElectionDistrictView prefab;

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
        //  Šù‘¶‚ğ‘S•”íœ
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

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
    }
}
