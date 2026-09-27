using UnityEngine;

public class ForeignCountryViewGenerator : MonoBehaviour
{
public Transform parent;
    public ForeignCountryRowView prefab;

    void Start()
    {
        TagManager.Instance.OnUnlockStateChanged += Refresh;
        Refresh();
    }

    void Refresh()
    {
        foreach (Transform c in parent)
            Destroy(c.gameObject);

        var unlocked = TagManager.Instance.GetUnlockedTagKeys();

        foreach (var key in unlocked)
        {
            if (key.tag != UnlockTag.ForeignCountry)
                continue;

            if (!key.key.EndsWith("_view"))
                continue;

            Create(key.key.Replace("_view", ""));
        }
    }

    void Create(string country)
    {
        var data = ForeignCountryManager.Instance.Get(country);
        if (data == null) return;

        var view = Instantiate(prefab, parent);
        view.Set(data);
    }
}
