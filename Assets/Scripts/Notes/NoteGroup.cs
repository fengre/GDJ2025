using UnityEngine;

[System.Serializable]
public class NoteGroup
{
    public int groupIndex;
    public string groupName;
    public Color groupColor;
    public float groupDecayRate; // How much this group's value drops per second
    public float groupValue = 50f;

    public AudioSource audioSource;

    public float redZoneTimer = 0f;
    public float maxRedZoneTime = 5f; // Customize as needed
    public bool isShutDown = false;
    public float earliestNoteTime;
    public bool hasStarted = false;

    public void Adjust(float delta)
    {
        groupValue = Mathf.Clamp(groupValue + delta, 0f, 100f);
    }
}
