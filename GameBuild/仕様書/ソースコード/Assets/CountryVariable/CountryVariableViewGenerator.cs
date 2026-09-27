using UnityEngine;
using System.Linq;

public class CountryVariableViewGenerator : MonoBehaviour
{
    public Transform parent;
    public CountryVariableView prefab;

    void Start()
    {
        TagManager.Instance.OnUnlockStateChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (TagManager.Instance != null)
            TagManager.Instance.OnUnlockStateChanged -= Refresh;
    }

    void Refresh()
    {
        // ‘Síœ
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        var unlocked = TagManager.Instance.GetUnlockedTagKeys();

        foreach (var key in unlocked)
        {
            // Post‚¾‚¯‘ÎÛ
            if (!key.key.EndsWith("_post")) continue;

            Create(key);
        }
    }

    void Create(UnlockTagKey key)
    {
        var view = Instantiate(prefab, parent);

       
        view.Initialize(key);
    }
}
