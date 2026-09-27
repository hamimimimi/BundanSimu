using TMPro;
using UnityEngine;

// ‰e‹¿—Í•\¦—pB

public class InfluencePointView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text influenceText;

    public void Refresh(int influencePoint)
    {
        influenceText.text = influencePoint.ToString();
    }
}
