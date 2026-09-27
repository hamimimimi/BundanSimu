using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZoomController : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private RectTransform zoomRoot;

    [Header("Zoom Settings")]
    [SerializeField]
    private float minZoom = 0.5f;

    [SerializeField]
    private float maxZoom = 2.5f;

    [SerializeField]
    private float zoomStep = 0.1f;

    [SerializeField]
    private float currentZoom = 1f;
    private bool dragging;

    void Start()
    {
        ApplyZoom();
    }

    public void ZoomIn()
    {
        currentZoom = Mathf.Min(currentZoom + zoomStep, maxZoom);
        ApplyZoom();
    }

    public void ZoomOut()
    {
        currentZoom = Mathf.Max(currentZoom - zoomStep, minZoom);
        ApplyZoom();
    }

    public void SetZoom(float value)
    {
        currentZoom = Mathf.Clamp(value, minZoom, maxZoom);
        ApplyZoom();
    }

    void ApplyZoom()
    {
        zoomRoot.pivot = new Vector2(0.5f, 0.5f);
        zoomRoot.localScale = Vector3.one * currentZoom;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    // マウスドラッグで移動
    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging) return;
        zoomRoot.anchoredPosition += eventData.delta;
    }

   
}
