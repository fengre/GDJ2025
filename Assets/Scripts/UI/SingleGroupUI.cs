using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleGroupUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI groupNameText;
    public RectTransform barBackground;  // container for zones & tick
    public RectTransform tick;
    public CanvasGroup groupCanvasGroup;

    [Header("Zones (0–100)")]
    [Tooltip("Define each colored zone by its min/max value (0–100) and color.")]
    public List<Zone> zones = new List<Zone>()
    {
        new Zone{ min=0,   max=25,  color=Color.red    },
        new Zone{ min=25,  max=45,  color=Color.yellow },
        new Zone{ min=45,  max=55,  color=Color.green  },
        new Zone{ min=55,  max=75,  color=Color.yellow },
        new Zone{ min=75,  max=100, color=Color.red    },
    };

    [Header("Lerp Settings")]
    public float lerpSpeed = 5f;

    [Serializable]
    public struct Zone
    {
        [Range(0,100)] public float min, max;
        public Color color;
    }

    float barWidth;
    float currentNorm, targetNorm;

    void Start()
    {
        // cache bar width
        barWidth = barBackground.rect.width;

        // create the colored zones
        foreach (var z in zones)
        {
            var go = new GameObject($"Zone_{z.min}_{z.max}", typeof(Image));
            go.transform.SetParent(barBackground, false);

            var rt = go.GetComponent<RectTransform>();
            // anchorMin/Max are normalized 0–1 across the width
            rt.anchorMin = new Vector2(z.min / 100f, 0f);
            rt.anchorMax = new Vector2(z.max / 100f, 1f);
            // zero out offsets so it stretches exactly
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            var img = go.GetComponent<Image>();
            img.color = z.color;
            // ensure these are behind the tick
            go.transform.SetSiblingIndex(0);
        }

        // initialize positions
        currentNorm = targetNorm = 0f;
    }

    void Update()
    {
        // smoothly move toward the new target
        currentNorm = Mathf.Lerp(currentNorm, targetNorm, Time.deltaTime * lerpSpeed);

        // reposition the tick
        Vector2 pos = tick.anchoredPosition;
        pos.x = currentNorm * barWidth;
        tick.anchoredPosition = pos;
    }

    public void SetGroupText(NoteGroup group)
    {
        groupNameText.text = (group.groupIndex + 1) + " | " + group.groupName;
        groupNameText.color = group.groupColor;
    }

    public void SetValue(float value)
    {
        targetNorm = Mathf.Clamp01(value / 100f);
    }

    public void SetGrayedOut(bool isGrayedOut)
    {
        groupCanvasGroup.alpha = isGrayedOut ? 0.5f : 1f;
        groupCanvasGroup.interactable = !isGrayedOut;
        groupCanvasGroup.blocksRaycasts = !isGrayedOut;
    }

}
