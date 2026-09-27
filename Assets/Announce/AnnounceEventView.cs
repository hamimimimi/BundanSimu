using UnityEngine;
using TMPro;

// AnnouncePrefab本体。5ターンで消滅する。

public class AnnounceEventView : MonoBehaviour
{
    public TMP_Text contentText;   // 内容
    public TMP_Text turnText;      // 発生したターン

    int remainingTurns = 5;

    public void OnTurnPassed()
    {
        remainingTurns--;
    Debug.Log("event remaining " + remainingTurns);
        if (remainingTurns <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(string message, int turn)
    {
        contentText.text = message;
        turnText.text = $"Turn {turn}";
    }


}
