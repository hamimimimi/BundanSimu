using UnityEngine;
using UnityEngine.UI;

public class MyAccController : MonoBehaviour
{
    [SerializeField] private Button tomyaccButton;
    [SerializeField] private GameObject myaccPanel;
    [SerializeField] private Button resumeButton;

    [SerializeField] private UIController uiController;

    void Start()
    {
        myaccPanel.SetActive(false);

        tomyaccButton.onClick.AddListener(ToMyAcc);
        resumeButton.onClick.AddListener(Resume);

    }

    private void ToMyAcc()
    {
        myaccPanel.SetActive(true);
        uiController?.RefreshListUI();
    }

    private void Resume()
    {
        myaccPanel.SetActive(false);
    }
 
}
