using UnityEngine;

[System.Serializable]
public class NoteGroupData
{
    public int groupIndex;
    public string groupName;
    public Color groupColor;
    public float groupDecayRate;
    public AudioSource audioSource;

    public NoteGroupData(int index, string name, Color color, float decayRate)
    {
        this.groupIndex = index;
        this.groupName = name;
        this.groupColor = color;
        this.groupDecayRate = decayRate;
    }
}

