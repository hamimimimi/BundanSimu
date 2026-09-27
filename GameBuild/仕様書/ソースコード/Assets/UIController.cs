using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public Transform contentParent;   // ScrollView -> Viewport -> Content をセット
    public GameObject itemPrefab;     // Project 側の prefab をセット
    public ScrollRect scrollRect;     // ScrollRect をセット（トップに戻すため）
    


    public void RefreshListUI()
    {
        Debug.Log("RefreshListUI START. contentParent=" + (contentParent ? contentParent.name : "NULL"));

        if (contentParent == null || itemPrefab == null)
        {
            Debug.LogError("UIController: contentParent or itemPrefab is null!");
            return;
        }

        // 既存の子を全部消す（Runtime）
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }

        // 読み込み
        List<MyAccData> list = SaveMyAccSystem.LoadAll();
        Debug.Log("Loaded list count: " + (list != null ? list.Count.ToString() : "null"));

        if (list == null || list.Count == 0)
        {
           

            // レイアウト更新してスクロールトップ
            Canvas.ForceUpdateCanvases();
            if (scrollRect != null) scrollRect.verticalNormalizedPosition = 1f;
            return;
        }

        foreach (var data in list)
        {
            // Instantiate(prefab, parent, false) を使うとローカル transform が正しくなる
            GameObject go = Instantiate(itemPrefab, contentParent, false);

            // 強制リセット（保険）
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localRotation = Quaternion.identity;
                rt.localScale = Vector3.one;
                rt.anchorMin = new Vector2(0f, 1f);
                rt.anchorMax = new Vector2(1f, 1f);
                rt.pivot = new Vector2(0.5f, 1f);
                rt.anchoredPosition = Vector2.zero;
            }

            // Setup（MyAccItemが存在すれば）
            var item = go.GetComponent<MyAccItem>();
            if (item != null)
            {
                item.Setup(data, id => OnDeleteRequested(id));
            }
            else
            {
                Debug.LogWarning("Instantiated prefab has no MyAccItem component: " + go.name);
            }
        }

        // レイアウト再構築（確実に反映させる）
        Canvas.ForceUpdateCanvases();
        // 即時レイアウト再構築（Content の RectTransform）
        var contentRect = contentParent as RectTransform;
        if (contentRect != null)
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        // スクロールを上に
        if (scrollRect != null) scrollRect.verticalNormalizedPosition = 1f;

        Debug.Log("RefreshListUI END");

       

    }

    


    public void OnDeleteRequested(string id)
    {
        var list = SaveMyAccSystem.LoadAll();
        list.RemoveAll(x => x.id == id);
        SaveMyAccSystem.SaveAll(list);
        RefreshListUI();
    }
}