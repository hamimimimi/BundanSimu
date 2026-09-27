using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyAccListUI : MonoBehaviour
{
    public GameObject MyAccitemPrefab;  // CharacterItem prefab
    public Transform Content;      // ListPanel ¨ Content

    public void RefreshList()
    {
        // ŒÃ‚¢ˆê——‚ğíœ
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        // V‚µ‚­ˆê——•\¦
        foreach (var c in MyAccManager.Instance.MyAccs)
        {
            GameObject obj = Instantiate(MyAccitemPrefab, Content);
            obj.GetComponentInChildren<TMP_Text>().text = $"{c.name}";
        }
    }
}
