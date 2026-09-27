using System;
using UnityEngine;

public class GovernmentStabilityManager : MonoBehaviour
{
    public static GovernmentStabilityManager Instance;
 
    [SerializeField] private int initialValue = 0;// 政権不安定度
    public int Instability { get; private set; }

    public Action<int> OnInstabilityChanged;

    void Awake()
    {
        Instance = this;
        Instability = initialValue;
    }

    public void AddInstability(int value)
    {
        Instability += value;
        Debug.Log($"政権不安定度 +{value} → {Instability}");

        OnInstabilityChanged?.Invoke(Instability);

        CheckVictory();
    }

    bool isGameEnded = false;
    
    void CheckVictory()
     {
        if (isGameEnded) return;
      
        if (Instability >= 1000)
       {
            isGameEnded = true;
        GameResultManager.Instance.Win(VictoryType.Instability1000);
       }
     }

    public void SetInstability(int value)
    {
        Instability = value;
        OnInstabilityChanged?.Invoke(Instability);
    }
}