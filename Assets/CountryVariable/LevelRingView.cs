using UnityEngine;
using UnityEngine.UI;

public class LevelRingView : MonoBehaviour
{
    public Image ringImage;

    // 現在レベル内の進捗（0〜1）
    public void SetProgress(float normalized)
    {
        ringImage.fillAmount = Mathf.Clamp01(normalized);
    }
}
