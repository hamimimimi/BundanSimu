using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// 関係国用パネル表示切替。

public class ForeignCountryPanelController : MonoBehaviour
{
    [SerializeField] UnlockTag revealByUnlock;

    [Header("Optional: specific key")]
    [SerializeField] string revealKey;

    [SerializeField] GameObject contentRoot;

    bool subscribed;

    void Awake()
    {

        if (contentRoot != null)
        contentRoot.SetActive(false);
    }


    void OnEnable()
    {
        StartCoroutine(WaitForTagManager());
    }

    void OnDisable()
    {
        if (subscribed && TagManager.Instance != null)
        {
         TagManager.Instance.OnUnlockStateChanged -= Refresh;
            subscribed = false;
        }
           
    }

    IEnumerator WaitForTagManager()
    {
        while (TagManager.Instance == null)
            yield return null;

        if (!subscribed)
        {
            TagManager.Instance.OnUnlockStateChanged += Refresh;
            subscribed = true;
        }

        Refresh();
    }
    

    void Refresh()
    {
        if (TagManager.Instance == null) return;

        bool unlocked;

          if (string.IsNullOrEmpty(revealKey))
        {
            unlocked = TagManager.Instance.IsUnlocked(revealByUnlock);
        }
        else
        {
            unlocked = TagManager.Instance.IsUnlocked(
                new UnlockTagKey { tag = revealByUnlock, key = revealKey }
            );
        }

        if (contentRoot != null)  
        contentRoot.SetActive(unlocked);
    }
}
