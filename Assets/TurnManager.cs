using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections;


// Turnの進行時の処理等。


public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [SerializeField]TMP_Text turnCountText;

    public int CurrentTurn = 1;

    private const string TURN_SAVE_KEY = "CURRENT_TURN";

    public static System.Action OnTurnChanged;
    
    public void SetTurn(int turn) 
    {
        CurrentTurn = turn;

    SaveTurn();        
    UpdateTurnUI();       
    }
    
    [SerializeField] Button turnEndButton;
    bool isProcessing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadTurn();
        UpdateTurnUI();
    }

    public void EndTurn()
    {Debug.Log("EndTurn START");
        if (isProcessing) return;

        StartCoroutine(EndTurnRoutine());
Debug.Log("EndTurn END");
    }

    IEnumerator EndTurnRoutine()
    { 
       isProcessing = true;

        if (turnEndButton != null) 
       turnEndButton.interactable = false;

        yield return null;

        // ターン進行
        CurrentTurn++;

        SimulationManager.Instance.StepAll();
     
        SaveTurn();
     
        UpdateTurnUI();
        
        OnTurnChanged?.Invoke();
       
        

        // 情報Point獲得
        PlayerInformationPointManager.Instance.GainTurnPoint();

        float gained =
            Mathf.FloorToInt(
                PlayerInformationPointManager.Instance.PointPerTurn
            );

        // 選択中 UnlockButton に進捗付与
        var key = UnlockSelectionManager.Instance.CurrentKey;

        if (key.HasValue)
        {
            UnlockProgressManager.Instance.AddProgress(
                key.Value,
                PlayerInformationPointManager.Instance.PointPerTurn
            );

            UnlockSelectionManager.Instance.CurrentButton.Refresh();
        }
        if (turnEndButton != null)
        turnEndButton.interactable = true;

        isProcessing = false;
    }

    void UpdateTurnUI()
    {
        if (turnCountText != null)
        {
            turnCountText.text = $"Turn {CurrentTurn}";
        }
    }

     void SaveTurn()
    {
        PlayerPrefs.SetInt(TURN_SAVE_KEY, CurrentTurn);
        PlayerPrefs.Save();
    }

     void LoadTurn()
    {      
     CurrentTurn = 1;
    }
}
