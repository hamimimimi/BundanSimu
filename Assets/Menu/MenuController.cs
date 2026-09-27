using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        GameReset.ResetGame();
        SceneManager.LoadScene("BundanSimu");
    }
    public void OnClickLoad()
    {
        SaveManager.Instance.Load(0);
        SceneManager.LoadScene("BundanSimu");
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Bundansimu");
    }

}