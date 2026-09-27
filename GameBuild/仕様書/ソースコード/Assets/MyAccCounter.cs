using UnityEngine;
using System.Collections.Generic;

public class MyAccCounter : MonoBehaviour
{
   
    public static MyAccCounter Instance;

    private List<MyAccData> cachedList;

    void Start()
    {
        cachedList = SaveMyAccSystem.LoadAll();
    }

    public int Count => cachedList != null ? cachedList.Count : 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Refresh()
    {
        cachedList = SaveMyAccSystem.LoadAll();
    }

}
