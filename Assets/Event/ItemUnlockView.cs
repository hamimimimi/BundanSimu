using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemUnlockView : MonoBehaviour
{
    public UnlockTagKey unlockKey;
    public UnlockTagKey itemKey;

    public GameObject root;

    [Header("UI")]
    public TMP_Text costText;
    public TMP_Text countText;

    [Header("ê›íË")]
    public int cost = 100;

    bool isInitialized = false;

    public TMP_Text nameText;

    void OnEnable()
    {
        if (!isInitialized)
        {
            StartCoroutine(Initialize());
        }
    }

    IEnumerator Initialize()
    {

    yield return new WaitUntil(() =>
        TagManager.Instance != null &&
        ItemInventoryManager.Instance != null &&
        MoneyManager.Instance != null
    );

    TagManager.Instance.OnUnlockStateChanged += Refresh;
    ItemInventoryManager.Instance.OnItemChanged += Refresh;
    MoneyManager.Instance.OnMoneyChanged += OnMoneyChanged;

    isInitialized = true;

    Refresh();

    }

    void OnDisable()
    {  
        if (TagManager.Instance != null)
        TagManager.Instance.OnUnlockStateChanged -= Refresh;
     
        if (ItemInventoryManager.Instance != null)
        ItemInventoryManager.Instance.OnItemChanged -= Refresh;

        if (MoneyManager.Instance != null)
        MoneyManager.Instance.OnMoneyChanged -= OnMoneyChanged;
        isInitialized= false;
    }

    void Refresh()
    {  
        
        
        if (TagManager.Instance == null) return;
        
        bool unlocked = TagManager.Instance.IsUnlocked(unlockKey);
       if (root != null)
        root.SetActive(unlocked);
        
        if (!unlocked) return;

        var def = UnlockDefinitionManager.Instance.Get(itemKey.tag, itemKey.key);
        if (def == null)
        {
          Debug.LogError($"Definition NOT FOUND: {itemKey.tag} / {itemKey.key}");
        }
else
{
    Debug.Log($"Definition FOUND: {def.title}");
}
        if (def != null && nameText != null)
        {
           nameText.text = def.title;
        }
        // èäéùêî
        int count = ItemInventoryManager.Instance.Get(itemKey);
        if (countText != null)
            countText.text = $"èäéùêî: {count}";

        // ÉRÉXÉg
        if (costText != null)
            costText.text = $"Cost: {cost}";
    }
    void OnMoneyChanged(int money)
    {
     
    }
   
}