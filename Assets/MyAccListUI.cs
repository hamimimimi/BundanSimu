using UnityEngine;
using UnityEngine.UI;
using TMPro;

// MyAccのリストを更新する。

public class MyAccListUI : MonoBehaviour
{
    public GameObject MyAccitemPrefab;  
    public Transform Content;     

    public void RefreshList()
    {
        // 古い一覧を削除
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        // 新しく一覧表示
        foreach (var c in MyAccManager.Instance.MyAccs)
        {
            GameObject obj = Instantiate(MyAccitemPrefab, Content);
            obj.GetComponentInChildren<TMP_Text>().text = $"{c.name}";
        }
    }
}
