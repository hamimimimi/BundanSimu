using UnityEngine;

public class MenuLoadPanelController : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public void Start()
    {
        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
