using UnityEngine;
using UnityEngine.UI;

public class MakeAccController : MonoBehaviour
{

    [SerializeField] private Button tomakeaccButton;
    [SerializeField] private GameObject makeaccPanel;
    [SerializeField] private Button resumemyaccButton;

    void Start()
    {
        makeaccPanel.SetActive(false);

        tomakeaccButton.onClick.AddListener(ToMakeAcc);
        resumemyaccButton.onClick.AddListener(ResumeMyAcc);

    }

    private void ToMakeAcc()
    {
        makeaccPanel.SetActive(true);
    }

    private void ResumeMyAcc()
    {
        makeaccPanel.SetActive(false);
    }
   
   
    void Update()
    {
        
    }
}
