using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    public float transitionSpeed = 10f;

    [Header("Optional Overrides")]
    public AudioClip customClickClip;
    public AudioClip customHoverClip;

    private RectTransform rect;
    private bool isHovered = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 target = isHovered ? hoverScale : normalScale;
        rect.localScale = Vector3.Lerp(rect.localScale, target, Time.unscaledDeltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play custom hover or fallback
        AudioClip clip = (customHoverClip != null && customHoverClip.length > 0f)
            ? customHoverClip
            : UISoundPlayer.Instance.hoverSound;
        UISoundPlayer.Instance.Play(clip);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play custom click or fallback
        AudioClip clip = (customClickClip != null && customClickClip.length > 0f)
            ? customClickClip
            : UISoundPlayer.Instance.clickSound;
        UISoundPlayer.Instance.Play(clip);
    }
}
