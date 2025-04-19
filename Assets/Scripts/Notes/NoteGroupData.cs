using UnityEngine;

[System.Serializable]
public class NoteGroupData
{
    public int groupIndex;
    public string groupName;
    public Color groupColor;

    public NoteGroupData(int index, string name, float r, float g, float b, float a)
    {
        this.groupIndex = index;
        this.groupName = name;
        this.groupColor = new Color(r, g, b, a);
    }
}

