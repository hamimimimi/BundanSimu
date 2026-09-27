using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultManager : MonoBehaviour
{
    public static GameResultManager Instance;

    public VictoryType victoryType;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Win(VictoryType type)
    {
        victoryType = type;

        SceneManager.LoadScene("ClearScene");
    }
}