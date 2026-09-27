using UnityEngine;
using System.Collections.Generic;

public static class SaveMyAccSystem
{
    private const string SAVE_KEY = "MY_ACC_SAVE";

    // 現在保持しているデータ
    private static List<MyAccData> cachedList = new List<MyAccData>();

    // 追加して即保存（DecisionButtonController から呼ばれている）
    public static void AddAndSave(MyAccData data)
    {
        LoadAll();

        cachedList.Add(data);
        SaveAll();
        MyAccCounter.Instance?.Refresh();
    }

    // 全保存（UIController から呼ばれている）
    public static void SaveAll()
    {
        foreach (var acc in cachedList)
        {
            acc.OnBeforeSave();
        }

        SaveWrapper wrapper = new SaveWrapper
        {
            list = cachedList
        };

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public static void SaveAll(List<MyAccData> list)
    {
        // 既存の保存用内部リストに反映
        cachedList = list;
        SaveAll();   // 既存の引数なし SaveAll を呼ぶ
    }


    // 全ロード（UIController から呼ばれている）
    public static List<MyAccData> LoadAll()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            cachedList = new List<MyAccData>();
            return cachedList;
        }

        string json = PlayerPrefs.GetString(SAVE_KEY);
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);

        cachedList = wrapper?.list ?? new List<MyAccData>();

        foreach (var acc in cachedList)
        {
            acc.OnAfterLoad();
        }

        return cachedList;
    }
}
