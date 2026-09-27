using UnityEngine;

[CreateAssetMenu(menuName = "Unlock/UnlockDefinition")]
public class UnlockDefinition : ScriptableObject
{
    [Header("Unlock")]
    public UnlockTag unlockTag;
    public string unlockKey; // ‹ó‚È‚ç Tag-only

    [Header("UI")]
    public string title;
    public string description;

    [Header("Requirement")]
    public UnlockTag[] requiredTags = new UnlockTag[0];

    [Header("Cost")]
    public int requiredPoint; // 0‚È‚çƒRƒXƒg‚È‚µ

    [Header("Key Variants")]
    public string[] keyVariants;

}
