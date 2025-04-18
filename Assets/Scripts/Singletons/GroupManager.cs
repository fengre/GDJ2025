using UnityEngine;
using System.Collections.Generic;

public class GroupManager : MonoBehaviour
{
    public static GroupManager Instance;
    
    // List of groups; you can pre-populate these via the Inspector or create them in code.
    public List<NoteGroup> groups = new List<NoteGroup>();

    // How much to change a group on a hit or miss.
    public float perfectHitIncrease = 5f;
    public float greatHitIncrease   = 3f;
    public float goodHitIncrease    = 2f;
    public float missDecrease       = 5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeGroups(List<NoteGroupData> groupData)
    {
        groups.Clear();
        foreach (var data in groupData)
        {
            NoteGroup group = new NoteGroup();
            group.name = data.name;
            group.color = data.color;
            group.value = 50f;
            group.decayRate = 1f; // default, or read from data if you include it
            groups.Add(group);
        }
    }


    private void Update()
    {
        foreach (var group in groups)
        {
            float decay = group.decayRate * Time.deltaTime;
            group.Adjust(-decay);
        }
    }


    /// <summary>
    /// Adjusts the group value for the group with the specified color.
    /// </summary>
    public void ChangeGroupValue(int groupIndex, float delta)
    {
        groups[groupIndex].Adjust(delta);
    }


    /// <summary>
    /// Optionally, check if the group’s value is within the desired band.
    /// </summary>
    public bool IsGroupWithinBand(Color targetColor, Vector2 targetBand)
    {
        NoteGroup group = groups.Find(g => ColorsEqual(g.color, targetColor));
        if (group != null)
        {
            return group.value >= targetBand.x && group.value <= targetBand.y;
        }
        return false;
    }

    /// <summary>
    /// Compares two Colors using a tolerance.
    /// </summary>
    private bool ColorsEqual(Color c1, Color c2)
    {
        return Mathf.Approximately(c1.r, c2.r) &&
               Mathf.Approximately(c1.g, c2.g) &&
               Mathf.Approximately(c1.b, c2.b) &&
               Mathf.Approximately(c1.a, c2.a);
    }

    public float GetGroupValue(Color color)
    {
        NoteGroup group = groups.Find(g => g.color == color);
        return group != null ? group.value : 0f;
    }
}
