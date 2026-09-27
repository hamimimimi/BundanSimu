using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockButton : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text conditionText;
    public Button button;

    [Header("Unlock")]
    public UnlockTagKey unlockKey;
   

    [Header("Requirement")]
    public UnlockTagKey[] requiredUnlockedKeys;

    [Header("Cost")]
    public int requiredPoint;

    public bool isSelected { get; private set; }

    public void SetSelected(bool value)
    {
        isSelected = value;
        Refresh();
    }

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }


    private void OnEnable()
    {
        if (TagManager.Instance != null)
            TagManager.Instance.OnUnlockStateChanged += Refresh;

        Refresh(); 
    }

    private void OnDisable()
    {
        if (TagManager.Instance != null)
            TagManager.Instance.OnUnlockStateChanged -= Refresh;
    }

    bool IsUnlocked()
    {
        return TagManager.Instance.IsUnlocked(unlockKey);         
    }

    bool RequirementsMet()
    {
        foreach (var key in requiredUnlockedKeys)
        {
            if (!TagManager.Instance.IsUnlocked(key))
                return false;
        }
        return true;
    }

    bool IsReadyToUnlock()
    {
        return UnlockProgressManager.Instance
            .IsComplete(unlockKey, requiredPoint);
    }   

    public void Refresh()
    {
        if (TagManager.Instance == null) return;

        if (IsUnlocked())
        {
            conditionText.text = "解放済み";
            button.interactable = false;
            return;
        }

        if (!RequirementsMet())
        {
            conditionText.text = "前提未達成";
            button.interactable = false;
            return;
        }

        if (IsReadyToUnlock())
        {
            conditionText.text = "解放可能";
            button.interactable = true;
            return;
        }

        // 未解放・未達成
        conditionText.text = isSelected
            ? "選択中（調査中）"
            : "未選択";

        button.interactable = true;
    }

    public void OnClick()
    {
        if (IsUnlocked())
            return;

        if (IsReadyToUnlock())
        {
            // 解放
            TagManager.Instance.Unlock(unlockKey.tag, unlockKey.key);
            UnlockSelectionManager.Instance.Clear();
            Refresh();
            return;
        }
        UnlockSelectionManager.Instance.Select(this);
    }

}
