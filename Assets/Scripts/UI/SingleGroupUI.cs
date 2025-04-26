using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleGroupUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI groupNameText;
    public Image groupIcon;
    public Image groupBackground;  // background color of the group
    public Color grayBackgroundColor;
    public RectTransform barBackground;  // container for zones & tick
    public RectTransform tick;
    public CanvasGroup groupCanvasGroup;
    public Button button;
    private int groupIndex;
    public bool locked = false;

    [Header("Zones (0–100)")]
    [Tooltip("Define each colored zone by its min/max value (0–100) and color.")]
    public List<Zone> zones = new List<Zone>();

    [Header("Lerp Settings")]
    public float lerpSpeed = 5f;

    [Serializable]
    public struct Zone
    {
        [Range(0,100)] public float min, max;
        public Color color;
        public Color grayColor;
    }

    float barWidth;
    float currentNorm, targetNorm;
    private List<GameObject> zoneObjects = new List<GameObject>();

    void Awake()
    {
        button.onClick.AddListener(OnGroupClick);
    }

    void Start()
    {
        Color outerGray = new Color32(0x10, 0x10, 0x10, 255);
        Color middleGray = new Color32(0x60, 0x60, 0x60, 255);
        Color centerGray = new Color32(0xA0, 0xA0, 0xA0, 255);

        Color red = new Color32(0xD8, 0x4A, 0x43, 255);
        Color yellow = new Color32(0xE6, 0xAD, 0x34, 255);
        Color green = new Color32(0x89, 0xBF, 0x4F, 255);

        zones = new List<Zone>()
        {
            new Zone{ min=0,   max=ScoreManager.Instance.okayMin,  color=red, grayColor=outerGray },
            new Zone{ min=ScoreManager.Instance.okayMin,  max=ScoreManager.Instance.idealMin,  color=yellow, grayColor=middleGray },
            new Zone{ min=ScoreManager.Instance.idealMin,  max=ScoreManager.Instance.idealMax,  color=green, grayColor=centerGray },
            new Zone{ min=ScoreManager.Instance.idealMax,  max=ScoreManager.Instance.okayMax,  color=yellow, grayColor=middleGray }, 
            new Zone{ min=ScoreManager.Instance.okayMax,  max=100, color=red, grayColor=outerGray },
        };

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
            zoneObjects.Add(go);
        }

        // initialize positions
        currentNorm = targetNorm = 0f;
    }

    private void OnGroupClick()
    {
        // Only switch if the group isn't shut down
        NoteManager.Instance.SwitchGroup(groupIndex);
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

    public void InitializeGroup(NoteGroup group)
    {
        groupIndex = group.groupIndex;
        groupNameText.text = group.groupName.ToUpper();
        Color bgColor = group.groupColor;
        bgColor.a = 0.1f;
        groupBackground.color = bgColor;

        // Assuming you have a list of sprites for the icons, indexed 1-5
        Sprite[] groupIcons = Resources.LoadAll<Sprite>("GroupIcons"); // Ensure these are in a "Resources/GroupIcons" folder

        if (group.groupIndex >= 0 && group.groupIndex < groupIcons.Length)
        {
            groupIcon.sprite = groupIcons[group.groupIndex];
        }
        else
        {
            Debug.LogWarning($"Invalid groupIndex {group.groupIndex}. Must be between 0 and 4.");
        }
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
        groupBackground.color = grayBackgroundColor;

        for (int i = 0; i < zones.Count; i++)
        {
            var z = zones[i];
            var go = zoneObjects[i];
            var img = go.GetComponent<Image>();
            img.color = z.grayColor;
            if (i == 2) {
                groupNameText.color = z.grayColor;
                groupIcon.color = z.grayColor;
            }
        }

    }

    public void Select()
    {
        Color bgColor = groupBackground.color;
        bgColor.a = 1f;
        groupBackground.color = bgColor;
    }

    public void Deselect()
    {
        Color bgColor = groupBackground.color;
        bgColor.a = 0.1f;
        groupBackground.color = bgColor;
    }

    public void SetLocked()
    {
        if (locked) return;
        locked = true;
        groupCanvasGroup.interactable = false;
        groupCanvasGroup.blocksRaycasts = false;
        groupBackground.color = grayBackgroundColor;
    }

}
