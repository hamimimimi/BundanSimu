using UnityEngine;
using TMPro;

public class TooltipController : MonoBehaviour
{
    public static TooltipController Instance;

    [SerializeField] private TMP_Text tooltipText;
    [SerializeField] private RectTransform rectTransform;

    private Canvas canvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        canvas = GetComponentInParent<Canvas>();
        Hide();
    }

    void Update()
    {
        FollowMouse();
    }

    public void Show(string message)
    {
        tooltipText.text = message;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void FollowMouse()
    {
        if (!gameObject.activeSelf) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera,
            out pos
        );

        rectTransform.localPosition = pos + new Vector2(20, -20);
    }
}
