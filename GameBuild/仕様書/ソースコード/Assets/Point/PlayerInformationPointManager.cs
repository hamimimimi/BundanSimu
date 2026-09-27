using UnityEngine;

public class PlayerInformationPointManager : MonoBehaviour
{
    public static PlayerInformationPointManager Instance;

    [SerializeField] float currentPoint;
    public float CurrentPoint => currentPoint;

    [Header("Per Turn")]
    [SerializeField] float basePointPerTurn = 1f;
    [SerializeField] float pointPerMyAcc = 0.1f;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public float PointPerTurn
    {
        get
        {
            int myAccCount =
                MyAccCounter.Instance != null
                    ? MyAccCounter.Instance.Count
                    : 0;

            return basePointPerTurn + myAccCount * pointPerMyAcc;
        }
    }

    public void GainTurnPoint()
    {
        currentPoint += PointPerTurn;
    }

    public bool CanConsume(float amount)
    {
        return currentPoint >= amount;
    }

    public void Consume(float amount)
    {
        currentPoint -= amount;
    }

}
