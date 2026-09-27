using TMPro;
using UnityEngine;

public class SaveSlotView : MonoBehaviour
{
    public int slot;
    public TMP_Text titleText;
    public TMP_Text infoText;


    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        var data = SaveManager.Instance.LoadMeta(slot);

        titleText.text = $"Slot {slot}";

        if (data == null)
        {
            infoText.text = "‹ó‚«";
        }
        else
        {
            infoText.text =
                $"Turn: {data.currentTurn}\n{data.saveTime}";
        }
    }
}
