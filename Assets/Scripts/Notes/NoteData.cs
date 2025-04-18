using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float timeToHit;
    public int lane;
    public int groupIndex;    // 0–3 instead of RGBA
}
