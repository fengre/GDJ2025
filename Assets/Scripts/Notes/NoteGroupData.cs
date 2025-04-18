using UnityEngine;

[System.Serializable]
public class NoteGroupData
{
    public int groupIndex;
    public string name;
    public Color color;

    public NoteGroupData(int index, string name, float r, float g, float b, float a)
    {
        this.groupIndex = index;
        this.name = name;
        this.color = new Color(r, g, b, a);
    }
}

