using UnityEngine;
using TMPro;

public class ClearSceneView : MonoBehaviour
{
    public TMP_Text resultText;

    void Start()
    {
        var type = GameResultManager.Instance.victoryType;

        resultText.text = GetMessage(type);
    }

    string GetMessage(VictoryType type)
    {
        switch (type)
        {
            case VictoryType.Instability1000:
                return "政権不安定度が限界に達し、政権を揺るがしました";

            default:
                return "勝利";
        }
    }
}