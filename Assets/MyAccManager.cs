using System.Collections.Generic;
using UnityEngine;

// MyAcc管理用。

public class MyAccManager : MonoBehaviour
{

    public static MyAccManager Instance;

    public List<MyAccData> MyAccs = new List<MyAccData>();

    public int Count => MyAccs.Count;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddMyAcc(MyAccData data)
    {
        MyAccs.Add(data);
    }

    public List<MyAccData> GetSaveData()
    {
    foreach (var acc in MyAccs)
    {
        acc.OnBeforeSave();
    }

    return new List<MyAccData>(MyAccs);
    }

    public void Load(List<MyAccData> data)
    {
        Debug.Log("MyAcc LOAD: " + data.Count);
    MyAccs = new List<MyAccData>(data);

    // ロード後処理
    foreach (var acc in MyAccs)
    {
        acc.OnAfterLoad();
    }
    }
}
