using UnityEngine;
using System.Collections;

public class ConstantsPanelController : MonoBehaviour
{
    [SerializeField] RectTransform panel;
    [SerializeField] float slideDuration = 0.25f;
    [SerializeField] float hiddenX = 600f;
    [SerializeField] float shownX = 0f;

    bool isShown = false;
    Coroutine slideRoutine;

    void Awake()
    {
        float width = panel.rect.width;
        hiddenX = width;
        shownX = 0f;

        panel.anchoredPosition =
            new Vector2(hiddenX, panel.anchoredPosition.y);

        isShown = false;
    }

    public void Toggle()
    {
        isShown = !isShown;

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(Slide(isShown ? shownX : hiddenX));
    }

    IEnumerator Slide(float targetX)
    {
        float startX = panel.anchoredPosition.x;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            float x = Mathf.Lerp(startX, targetX, t);
            panel.anchoredPosition = new Vector2(x, panel.anchoredPosition.y);
            yield return null;
        }

        panel.anchoredPosition = new Vector2(targetX, panel.anchoredPosition.y);
    }

    public void Show()
    {
        if (isShown) return;
        isShown = true;
        StartSlide(shownX);
    }

    public void Hide()
    {
        if (!isShown) return;
        isShown = false;
        StartSlide(hiddenX);
    }

    void StartSlide(float targetX)
    {
        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(Slide(targetX));
    }


}
