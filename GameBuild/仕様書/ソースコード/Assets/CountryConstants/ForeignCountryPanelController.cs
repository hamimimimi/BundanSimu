using UnityEngine;
using System.Collections.Generic;

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
    void Update()
    {
    if (!subscribed && TagManager.Instance != null)
    {
        TagManager.Instance.OnUnlockStateChanged += Refresh;
        subscribed = true;

        Debug.Log("Foreign panel subscribed");
        Refresh();
    }
}

    void OnEnable()
    {
        TrySubscribe();
        Refresh();
    }

    void OnDisable()
    {
        if (subscribed && TagManager.Instance != null)
        {
         TagManager.Instance.OnUnlockStateChanged -= Refresh;
            subscribed = false;
        }
           
    }
    void TrySubscribe()
    {
        if (!subscribed && TagManager.Instance != null)
        {
            TagManager.Instance.OnUnlockStateChanged += Refresh;
            subscribed = true;
        }
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
