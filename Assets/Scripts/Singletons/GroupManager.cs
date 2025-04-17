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

    // The target band. In this example, the goal is to keep the value between 45 and 55.
    public Vector2 targetBand = new Vector2(45f, 55f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // If no groups are assigned in the Inspector, create default groups.
            if (groups.Count == 0)
            {
                groups.Add(new NoteGroup() { color = Color.red,    value = 50 });
                groups.Add(new NoteGroup() { color = Color.green,  value = 50 });
                groups.Add(new NoteGroup() { color = Color.blue,   value = 50 });
                groups.Add(new NoteGroup() { color = new Color(1f, 1f, 0f, 1f), value = 50 });
            }
        }
        else
        {
            Destroy(gameObject);
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
    public void ChangeGroupValue(Color targetColor, float delta)
    {
        NoteGroup group = groups.Find(g => ColorsEqual(g.color, targetColor));
        if (group != null)
        {
            group.Adjust(delta);
            Debug.Log("Group " + targetColor.ToString() + " new value: " + group.value);
        }
        else
        {
            Debug.LogWarning("No group found for color: " + targetColor);
        }
    }

    /// <summary>
    /// Optionally, check if the group’s value is within the desired band.
    /// </summary>
    public bool IsGroupWithinBand(Color targetColor)
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
