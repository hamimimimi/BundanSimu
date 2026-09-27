using UnityEngine;

public class PlayerPointManager : MonoBehaviour
{
    public static PlayerPointManager Instance { get; private set; }

    [SerializeField]
    int currentPoint = 0;

    // UnlockButton が参照する用
    public int CurrentPoint => currentPoint;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 今は常に足りていることにする（仮実装）
    public bool HasEnough(int cost)
    {
        return currentPoint >= cost;
    }

    // UnlockButton から呼ばれる
    public bool Consume(int cost)
    {
        if (currentPoint < cost)
            return false;

        currentPoint -= cost;
        return true;
    }

    // TurnEnd などから呼ぶ予定
    public void AddPoint(int value)
    {
        currentPoint += value;
    }
}
