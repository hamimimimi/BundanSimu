using System.Collections.Generic;
using UnityEngine;

public class MyAccManager : MonoBehaviour
{

    public static MyAccManager Instance;

    public List<MyAccData> MyAccs = new List<MyAccData>();

    public int Count => MyAccs.Count;
    void Awake()
    {
        if (Instance == null) Instance = this;
        Load();
    }

    public void AddMyAcc(MyAccData data)
    {
        MyAccs.Add(data);
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(new Wrapper { list = MyAccs });
        PlayerPrefs.SetString("MyAccList", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        string json = PlayerPrefs.GetString("MyAccList", "");
        if (!string.IsNullOrEmpty(json))
        {
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            MyAccs = wrapper.list;
        }
    }

    [System.Serializable]
    class Wrapper
    {
        public List<MyAccData> list;
    }
}
