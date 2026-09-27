using UnityEngine;
using TMPro;

public class InfoPointView : MonoBehaviour
{
    [SerializeField] TMP_Text infoPointText;

    void Update()
    {
        var mgr = PlayerInformationPointManager.Instance;
        if (mgr == null) return;

        infoPointText.text = $"èÓïÒPt / Turn : {mgr.PointPerTurn:F1}";
    }
}
