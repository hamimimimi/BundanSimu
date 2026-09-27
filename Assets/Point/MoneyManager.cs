using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] int initialMoney = 1000;

    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;

    void Awake()
    {
        Instance = this;
        Money = initialMoney;
    }

    public void Add(int value)
    {
        Money += value;

        if (Money < 0)
            Money = 0;

        OnMoneyChanged?.Invoke(Money);
    }

    public bool Spend(int value)
    {
        if (!CanConsume(value))
            return false;

        Money -= value;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }

    public void Set(int value)
    {
        Money = value;
        OnMoneyChanged?.Invoke(Money);
    }
public bool CanConsume(int value)
{
    return Money >= value;
}
}
