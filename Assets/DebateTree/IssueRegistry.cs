using UnityEngine;
using System.Collections.Generic;

public class IssueRegistry : MonoBehaviour
{
    public static IssueRegistry Instance;

    public List<IssueDefinition> issues;

    Dictionary<string, IssueDefinition> map;

    void Awake()
    {
        Instance = this;

        map = new Dictionary<string, IssueDefinition>();

        foreach (var issue in issues)
        {
            map[issue.issueId] = issue;
        }
    }

    public IssueDefinition Get(string id)
    {
        return map[id];
    }
}