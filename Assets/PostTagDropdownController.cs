using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class PostTagDropdownController : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    private List<UnlockTagKey> currentKeys = new();

    public UnlockTagKey GetSelectedKey()
    {
        int index = dropdown.value;

        if (index <= 0 || index >= currentKeys.Count + 1)
            return default;

        return currentKeys[index - 1];
    }
    private void OnEnable()
    {
        Refresh();
        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged += Refresh;
        }

    }


    private void OnDisable()
    {
        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged -= Refresh;
        }
    }

    private bool refreshing;

    public void Refresh()
    {   Debug.Log("PostTagDropdown Refresh called");
        if (refreshing) return;
        refreshing = true;

        dropdown.ClearOptions();

        currentKeys = TagManager.Instance
           .GetUnlockedTagKeys()
           .Where(k => !string.IsNullOrEmpty(k.key) && k.key.EndsWith("_post"))
           .ToList();        

        var options = currentKeys
            .Select(k => k.ToString())
            .ToList();

        options.Insert(0, "None");
Debug.Log("Add Post Only: " + string.Join(",", currentKeys));
        dropdown.AddOptions(options);
        Debug.Log("Dropdown count after add: " + dropdown.options.Count);
        dropdown.value = 0;

        refreshing = false;
    }
}
