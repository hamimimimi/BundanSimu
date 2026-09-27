using UnityEngine;
using UnityEngine.UI;

public class AnnounceEventViewController : MonoBehaviour
{
    public static AnnounceEventViewController Instance;

    [SerializeField] ScrollRect scrollRect;
    public Transform content;
    public AnnounceEventView prefab;

    void Awake()
    {
        Instance = this;
    }

     private void OnEnable()
     {
    
      TurnManager.OnTurnChanged += OnTurnChanged;
     }

     private void OnDisable()
      {
       TurnManager.OnTurnChanged -= OnTurnChanged;
      }

      void OnTurnChanged()
      {
   
       foreach (var v in content.GetComponentsInChildren<AnnounceEventView>(true))
       {
         v.OnTurnPassed();
       }
      }

    public void AddEvent(string message)
    {
        var view = Instantiate(prefab, content);

        view.Setup(
            message,
            TurnManager.Instance.CurrentTurn
        );
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;

    }


}
