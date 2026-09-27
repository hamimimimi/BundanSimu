using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoadSlotButton : MonoBehaviour
{
    public int slot;

    public void OnClick()
    {
        Debug.Log($"Load Slot {slot}");

        SaveManager.Instance.pendingLoadSlot = slot;

        // ÉQÅ[ÉÄñ{ï“Ç÷
        SceneManager.LoadScene("BundanSimu");
    }
}