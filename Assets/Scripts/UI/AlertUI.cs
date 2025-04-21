using UnityEngine;
using TMPro;
using System.Collections;

public class AlertUI : MonoBehaviour
{
    public TextMeshProUGUI label;
    public CanvasGroup canvasGroup;
    private float fadeInTime = 0.1f;
    private float displayTime = 0.5f;
    private float fadeOutTime = 0.3f;

    private Coroutine currentRoutine;

    private void Start()
    {
        // Start hidden
        canvasGroup.alpha = 0f;
    }

    public void Show(string message, Color color)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        label.text = message;
        label.color = color;
        currentRoutine = StartCoroutine(PlayFeedback());
    }

    private IEnumerator PlayFeedback()
    {
        // fade in
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // hold
        yield return new WaitForSeconds(displayTime);

        // fade out
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
