using UnityEngine;

public class IssueUnlockListener : MonoBehaviour
{
    void OnEnable()
    {
        Invoke(nameof(Register), 0f);
    }

     void Register()
    {
        if(TagManager.Instance != null) 
        TagManager.Instance.OnUnlockStateChanged += OnUnlock;
    }

    void OnDisable()
    {
        TagManager.Instance.OnUnlockStateChanged -= OnUnlock;
    }

    void OnUnlock()
    {
        var tm = TagManager.Instance;
        var unlocked = tm.GetUnlockedTagKeys();

        foreach (var key in unlocked)
        {
            if (key.tag != UnlockTag.Issue) continue;

            if (key.key.EndsWith("_post") || key.key.EndsWith("_view"))
            continue;

                string postKey = key.key + "_post";

               var postTagKey = new UnlockTagKey
           {
                  tag = UnlockTag.Issue,
                  key = postKey
           };

                if (!tm.IsUnlocked(postTagKey))
           {              
                    tm.Unlock(UnlockTag.Issue, postKey);

                    Debug.Log("Post unlocked: " + postKey);
            }           
        }
    }
}
