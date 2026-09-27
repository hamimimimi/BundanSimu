using UnityEngine;

public class DebateTreePanelController : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false); // Å‰‚Í”ñ•\¦
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
