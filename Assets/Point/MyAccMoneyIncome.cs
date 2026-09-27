using UnityEngine;


// MyAcc‚É‚æ‚é‚¨‹à‚Ì‘‰Á—p

public class MyAccMoneyIncome : MonoBehaviour
{
    private void OnEnable()
    {
        TurnManager.OnTurnChanged += GainMoney;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChanged -= GainMoney;
    }

    void GainMoney()
    {
        if (MyAccManager.Instance == null)
            return;

        int totalIncome = 0;

        foreach (var acc in MyAccManager.Instance.MyAccs)
        {
            int level = acc.GetStatusLevel(StatusTagType.Money);

            if (level <= 0)
                continue;

            totalIncome += (acc.influencePoint / 100) * level;
        }

        if (totalIncome > 0)
        {
            MoneyManager.Instance.Add(totalIncome);

            Debug.Log($"MyAccû“ü +{totalIncome}");
        }
    }
}
