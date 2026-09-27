using UnityEngine;
using System.Collections.Generic;

public class UnlockUIRegistry : MonoBehaviour
{
    public Transform issueParent;
    public Transform politicianParent;
    public Transform partyParent;

    Dictionary<UnlockTag, Transform> map;

    void Awake()
    {
        map = new Dictionary<UnlockTag, Transform>()
        {
            { UnlockTag.Issue, issueParent },
            { UnlockTag.Politician, politicianParent },
            { UnlockTag.PoliticalParty, partyParent }
        };
    }

    public Transform GetParent(UnlockTag tag)
    {
        return map[tag];
    }
}