using UnityEngine;

public class ElectionViewController : MonoBehaviour
{
    public static ElectionViewController Instance;

    public ElectionView prefab;
    public Transform parent;

    void Awake()
    {
        Instance = this;
    }

    public void Create(ElectionDistrictData district)
    {
        var view = Instantiate(prefab, parent);
        view.Setup(district);
    }
}