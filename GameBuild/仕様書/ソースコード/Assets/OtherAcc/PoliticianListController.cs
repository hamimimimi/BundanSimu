using UnityEngine;

public class PoliticianListController : MonoBehaviour
{
    public static PoliticianListController Instance;

    public Transform parent;
    public PoliticianListItem prefab;

    void Awake()
    {
        Instance = this;
    }

    public void Add(PoliticianData data)
    {
        var item = Instantiate(prefab, parent);
        item.Setup(data);
    }
}