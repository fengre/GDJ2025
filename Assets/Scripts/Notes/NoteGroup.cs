using UnityEngine;

[System.Serializable]
public class NoteGroup
{
    public Color color;
    public float value = 50f;
    public float decayRate = 1f; // How much this group's value drops per second

    public void Adjust(float delta)
    {
        value = Mathf.Clamp(value + delta, 0f, 100f);
    }
}
