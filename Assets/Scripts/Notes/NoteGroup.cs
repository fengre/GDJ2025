using UnityEngine;

[System.Serializable]
public class NoteGroup
{
    public int groupIndex;
    public string groupName;
    public Color groupColor;
    public float groupValue = 50f;
    public float decayRate = 1f; // How much this group's value drops per second

    public void Adjust(float delta)
    {
        groupValue = Mathf.Clamp(groupValue + delta, 0f, 100f);
    }
}
