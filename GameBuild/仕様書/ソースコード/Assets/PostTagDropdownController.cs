using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class PostTagDropdownController : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    private List<UnlockTagKey> currentKeys = new();

    private void Start()
    {
        
        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged += Refresh;
        }
     Refresh();

    }


    private void OnDestroy()
    {
        if (TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged -= Refresh;
        }
    }

    public void Refresh()
    {
        dropdown.ClearOptions();

        currentKeys = TagManager.Instance
           .GetUnlockedTagKeys()
           .Where(k => !string.IsNullOrEmpty(k.key) && k.key.EndsWith("_post"))
           .ToList();

        Debug.Log("=== POST KEYS ===");
        

        var options = currentKeys
            .Select(k => k.ToString())
            .ToList();

        options.Insert(0, "None");

        dropdown.AddOptions(options);


        dropdown.value = 0;
    }
}
