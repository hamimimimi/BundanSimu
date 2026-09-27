using UnityEngine;
using System.Collections.Generic;

public class ElectionViewController : MonoBehaviour
{
    public static ElectionViewController Instance;

    public ElectionView prefab;
    public Transform parent;

     Dictionary<string, ElectionView> views = new();

    void Awake()
    {
        Instance = this;
    }

    public void Create(ElectionDistrictData district)
    {    
        if (views.ContainsKey(district.districtId))
        return;

        var view = Instantiate(prefab, parent);
        view.Setup(district);
        views[district.districtId] = view;
        district.view = view;
    }

    public void Refresh(ElectionDistrictData district)
    {
        if (!views.ContainsKey(district.districtId)) return;
        var view = views[district.districtId];
        view.Setup(district);

        district.view = view;
    }
}