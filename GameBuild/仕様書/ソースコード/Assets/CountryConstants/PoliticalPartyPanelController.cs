using UnityEngine;
using System.Linq;

public class PoliticalPartyPanelController : MonoBehaviour
{
    public Transform parent; // •\¦æ
    public PoliticalPartyView prefab;

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
            // view‚¾‚¯‘ÎÛ
            if (key.tag != UnlockTag.PoliticalParty) continue;
            if (!key.key.EndsWith("_view")) continue;

            CreateIfNeeded(key);
        }
    }

    void CreateIfNeeded(UnlockTagKey key)
    {
        

        string baseId = key.key.Replace("_view", "");

        var data = CountryConstantManager.Instance.Data;

        var party = data.politicalParties
            .FirstOrDefault(p => p.partyId == baseId);

        if (party == null) return;

        var view = Instantiate(prefab, parent);
        view.Setup(party);
    }
}
