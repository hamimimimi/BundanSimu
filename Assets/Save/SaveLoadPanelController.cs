using UnityEngine;
using TMPro;

public class SaveLoadPanelController : MonoBehaviour
{
    public static SaveLoadPanelController Instance;

    public GameObject panel;
    public GameObject slotPanel;
    [SerializeField] TMP_Text modeText;

    public enum Mode
    {
        Save,
        Load
    }

    public Mode CurrentMode { get; private set; }

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void OpenSave()
    {
        CurrentMode = Mode.Save;
        modeText.text = "セーブ";

        panel.SetActive(true);
        slotPanel.SetActive(true);
        
        RefreshSlots();
    }

    public void OpenLoad()
    {
        CurrentMode = Mode.Load;
        
        modeText.text = "ロード";
        panel.SetActive(true);
        slotPanel.SetActive(true);
        
        RefreshSlots();
    }

    public void Close()
    {
        panel.SetActive(false);
        
        if (slotPanel != null)
        slotPanel.SetActive(false);
    }

public void RefreshSlots()
{
    foreach (var v in FindObjectsByType<SaveSlotView>(FindObjectsSortMode.None))
        v.Refresh();
}
}
