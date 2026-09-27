using UnityEngine;

public class UnlockSelectionManager : MonoBehaviour
{
    public static UnlockSelectionManager Instance;
    // UI的に選択されているボタン
    public UnlockButton CurrentButton { get; private set; }

    // ロジック的に選択されている UnlockKey
    public UnlockTagKey? CurrentKey { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Select(UnlockButton button)
    {
        if (CurrentButton == button) return;

        if (CurrentButton != null)
            CurrentButton.SetSelected(false);

        CurrentButton = button;
        CurrentKey = button.unlockKey;
        CurrentButton.SetSelected(true);
    }

    public void Clear()
    {
        if (CurrentButton != null)
            CurrentButton.SetSelected(false);

        CurrentButton = null;
        CurrentKey = null;
    }
    public bool HasSelection()
    {
        return CurrentKey.HasValue;
    }
}
