using UnityEngine;
using TMPro;

public class ItemView : MonoBehaviour
{
    public UnlockTagKey itemKey;

    public TMP_Text nameText;
    public TMP_Text countText;

    void Start()
    {
        Refresh();
    }
void OnEnable()
{
    if (ItemInventoryManager.Instance != null)
        ItemInventoryManager.Instance.OnItemChanged += Refresh;

    
}

void OnDisable()
{
    if (ItemInventoryManager.Instance != null)
        ItemInventoryManager.Instance.OnItemChanged -= Refresh;
}
    void Refresh()
    {
        var def = UnlockDefinitionManager.Instance.Get(itemKey.tag, itemKey.key);

        if (def != null && nameText != null)
            nameText.text = def.title;

        int count = ItemInventoryManager.Instance.Get(itemKey);
      
        gameObject.SetActive(count > 0);

        if (count <= 0) return;
        if (countText != null)
            countText.text = $"x{count}";
    }
    public void OnClickUse()
    {
        ItemUseController.Instance.Open(itemKey);
    }
}
