using UnityEngine;
using UnityEngine.UI;

public class MakeMyAccControlller : MonoBehaviour
{
    [SerializeField] private Button tomakemyaccButton;
    [SerializeField] private GameObject makemyaccPanel;
    [SerializeField] private Button resumemyaccButton;

    void Start()
    {
        makemyaccPanel.SetActive(false);

        tomakemyaccButton.onClick.AddListener(ToMyAcc);
        resumemyaccButton.onClick.AddListener(Resume);

    }

    private void ToMyAcc()
    {
        makemyaccPanel.SetActive(true);
    }

    private void Resume()
    {
        makemyaccPanel.SetActive(false);
    }

    void Update()
    {

    }
}

