using UnityEngine;
using UnityEngine.SceneManagement;

// ƒQ[ƒ€ƒNƒŠƒAŠÖŒW

public class ClearSceneController : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}