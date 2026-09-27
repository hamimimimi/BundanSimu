using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IssueUnlockDefinitionBuilder : MonoBehaviour, IUnlockDefinitionBuilder
{
    public IssueRegistry registry;
    public void Build()
    {
        var manager = UnlockDefinitionManager.Instance;

        string[] issues =
        {
            "territorial_issue",
            "historical_issue"
        };

        foreach (var issue in registry.issues)
        {
            var def = ScriptableObject.CreateInstance<UnlockDefinition>();

            def.unlockTag = UnlockTag.Issue;
            def.unlockKey = issue.issueId;

            def.title = issue.displayName;

            manager.Register(def);
        }
    }
}
