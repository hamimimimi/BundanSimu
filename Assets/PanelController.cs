using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// パネル切り替え用。List更新などのイベントはonOpenにアタッチ


public class PanelControlller : MonoBehaviour
{
    [SerializeField] private Button toPanelButton;
    [SerializeField] private GameObject Panel;
    [SerializeField] private Button resumeButton;

    [SerializeField] private UnityEvent onOpen;

    void Start()
    {
        Panel.SetActive(false);

        toPanelButton.onClick.AddListener(ToPanel);
        resumeButton.onClick.AddListener(Resume);

    }

    private void ToPanel()
    {
        Panel.SetActive(true);
        onOpen?.Invoke();
    }

    private void Resume()
    {
        Panel.SetActive(false);
    }


}

