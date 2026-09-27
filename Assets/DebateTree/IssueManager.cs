using UnityEngine;
using System.Collections.Generic;

public class IssueDefinition1 : MonoBehaviour
{
  public static IssueDefinition1 Instance;

    Dictionary<string, bool> unlockedIssues = new();

    void Awake()
    {
        Instance = this;
    }

    public void Unlock(string issue)
    {
        unlockedIssues[issue] = true;
    }

    public bool IsUnlocked(string issue)
    {
        return unlockedIssues.ContainsKey(issue);
    }
}
