using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        var saveManager = SaveManager.Instance;

        if (saveManager.pendingLoadSlot.HasValue)
        {
            int slot = saveManager.pendingLoadSlot.Value;
            saveManager.pendingLoadSlot = null;

            saveManager.Load(slot);
        }
    }
}