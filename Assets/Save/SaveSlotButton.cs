using UnityEngine;

public class SaveSlotButton : MonoBehaviour
{
    public int slotIndex;

    public void OnClick()
    {
        var mode = SaveLoadPanelController.Instance.CurrentMode;

        if (mode == SaveLoadPanelController.Mode.Save)
        {
            SaveManager.Instance.Save(slotIndex);
        }
        else
        {
            SaveManager.Instance.Load(slotIndex);
        }

        SaveLoadPanelController.Instance.RefreshSlots();
    }
}
