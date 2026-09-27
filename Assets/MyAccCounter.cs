using UnityEngine;
using System.Collections.Generic;

// MyAcc‚Ì”‚ğŒvZB


public class MyAccCounter : MonoBehaviour
{
   
    public static MyAccCounter Instance;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public int Count
    {
        get
        {
            if (MyAccManager.Instance == null)
                return 0;

            return MyAccManager.Instance.MyAccs.Count;
        }
    }

}
