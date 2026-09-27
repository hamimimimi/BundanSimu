using UnityEngine;

[CreateAssetMenu(menuName = "Country/IssueDefinition")]
public class IssueDefinition : ScriptableObject
{
    public string issueId;

    public string displayName;

    public IssueCategory category;
}

public enum IssueCategory
{
    Foreign,
    Economy,
    Society,
    Borderdispute,
    Histricaldispute

}
