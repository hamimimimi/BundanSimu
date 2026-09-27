using UnityEngine;

public class IssueDefinitionBuilder : MonoBehaviour, IUnlockDefinitionBuilder
{
    public void Build()
    {
        var manager = FindObjectOfType<IssueDefinitionManager>();

        manager.Add(new IssueDefinition
        {
            issueId = "territorial_issue",
            displayName = "Territorial Issue"
        });

        manager.Add(new IssueDefinition
        {
            issueId = "historical_issue",
            displayName = "Historical Issue"
        });

        Debug.Log("Issue definitions built");
    }
}