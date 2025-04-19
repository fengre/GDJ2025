using UnityEngine;
using TMPro;
using System.Collections;

public class FeedbackUI : MonoBehaviour
{
    public TextMeshProUGUI label;
    public CanvasGroup canvasGroup;
    public float fadeInTime = 0.1f;
    public float displayTime = 0.5f;
    public float fadeOutTime = 0.3f;

    private Coroutine currentRoutine;

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
        Destroy(gameObject);
    }
}
