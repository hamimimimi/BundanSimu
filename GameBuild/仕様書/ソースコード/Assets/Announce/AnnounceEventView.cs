using UnityEngine;
using TMPro;

public class AnnounceEventView : MonoBehaviour
{
    public TMP_Text contentText;
    public TMP_Text turnText;

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
