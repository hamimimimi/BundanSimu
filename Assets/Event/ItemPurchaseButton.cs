using UnityEngine;

public class ItemPurchaseButton : MonoBehaviour
{
    public UnlockTagKey itemKey;

    public void OnClick()
    {
        int cost = 100;

        if (!MoneyManager.Instance.CanConsume(cost))
        {
            Debug.Log("‚¨‹à‚ª‘«‚è‚È‚¢");
            return;
        }
        MoneyManager.Instance.Spend(cost);
        ItemInventoryManager.Instance.Add(itemKey, 1);
       
    }
}