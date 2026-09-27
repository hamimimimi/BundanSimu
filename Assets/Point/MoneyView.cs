using TMPro;
using UnityEngine;
using System.Collections;

public class MoneyView : MonoBehaviour
{
    public TMP_Text text;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => MoneyManager.Instance != null);

        MoneyManager.Instance.OnMoneyChanged += Refresh;
        Refresh(MoneyManager.Instance.Money);
    }

    void Refresh(int value)
    {
        text.text = $"Ž‘‹à : {value}";
    }

    void OnDestroy()
    {
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged -= Refresh;
    }

public void Refresh()
{
    text.text = $"Ž‘‹à: {MoneyManager.Instance.Money}";
}
}