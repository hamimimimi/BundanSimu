using TMPro;
using UnityEngine;
using System.Collections;

public class GovernmentStabilityView : MonoBehaviour
{
    public TMP_Text text;

    IEnumerator Start()
    {
        // Manager生成待ち
        yield return new WaitUntil(() => GovernmentStabilityManager.Instance != null);

        GovernmentStabilityManager.Instance.OnInstabilityChanged += Refresh;

        Refresh(GovernmentStabilityManager.Instance.Instability);
    }

    void Refresh(int value)
    {
        text.text = $"政権不安定度 : {value}";
    }

    // UIRefresher用
        public void Refresh()
    {
        if (GovernmentStabilityManager.Instance == null) return;

        int value = GovernmentStabilityManager.Instance.Instability;
        text.text = $"政権不安定度 : {value}";
    }


    void OnDestroy()
    {
        if (GovernmentStabilityManager.Instance != null)
            GovernmentStabilityManager.Instance.OnInstabilityChanged -= Refresh;
    }
}