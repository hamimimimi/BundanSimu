using UnityEngine;
using TMPro;
using System.IO;

public class MenuSaveSlotView : MonoBehaviour
{
    public int slot;

    public TMP_Text infoText;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        string path = Application.persistentDataPath + $"/save_{slot}.json";

        if (!File.Exists(path))
        {
            infoText.text = "Empty";
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        infoText.text = $"Turn {data.currentTurn}";
    }
}
