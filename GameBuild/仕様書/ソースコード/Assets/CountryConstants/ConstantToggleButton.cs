using UnityEngine;


public class ConstantToggleButton : MonoBehaviour
{
    [SerializeField] ConstantsPanelController panel;

    public void OnClick()
    {
        panel.Toggle();
    }
}
