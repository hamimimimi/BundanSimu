using UnityEngine;

// Load時のUI更新

public class UIRefresher : MonoBehaviour
{
    void OnEnable()
    {
        SaveManager.Instance.OnAfterLoad += RefreshAll;
    }

    void OnDisable()
    {
        SaveManager.Instance.OnAfterLoad -= RefreshAll;
    }

    void RefreshAll()
    {
        foreach (var v in FindObjectsByType<CountryVariableView>(FindObjectsSortMode.None))
            v.RefreshAll();

        foreach (var v in FindObjectsByType<GovernmentStabilityView>(FindObjectsSortMode.None))
              v.Refresh();



        // 他も必要なら追加
    }
}
