using UnityEngine;

// ConstantPanel（右パネル）開閉ボタン

public class ConstantToggleButton : MonoBehaviour
{
    [SerializeField] ConstantsPanelController panel;

    public void OnClick()
    {
        panel.Toggle();
    }
}
