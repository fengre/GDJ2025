using System.Collections;
using UnityEngine;

public class JudgmentLineGlow : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    public Color glowColor = Color.gray;
    public float flashDuration = 0.2f;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void TriggerGlow()
    {
        rend.material.color = glowColor;
        // StopAllCoroutines();
        // StartCoroutine(GlowCoroutine());
    }

    public void CancelGlow()
    {
        rend.material.color = originalColor;
    }

    private IEnumerator GlowCoroutine()
    {
        rend.material.color = glowColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }
}
