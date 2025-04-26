using UnityEngine;
using System.Collections.Generic;

public class GroupUIManager : MonoBehaviour
{
    public static GroupUIManager Instance { get; private set; }

    [Header("Prefab & Parent")]
    public GameObject groupDisplayPrefab;
    public Transform parentPanel;

    public Dictionary<int, SingleGroupUI> groupUIs = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitializeGroups(List<NoteGroup> groups)
    {
        foreach (var group in groups)
        {
            var go = Instantiate(groupDisplayPrefab, parentPanel);
            var ui = go.GetComponent<SingleGroupUI>();
            ui.InitializeGroup(group);
            ui.SetValue(group.groupValue);
            groupUIs[group.groupIndex] = ui;
        }
    }

    public void UpdateGroupValue(int groupIndex, float newValue)
    {
        if (groupUIs.TryGetValue(groupIndex, out var ui))
            ui.SetValue(newValue);
    }

    public void GrayOut(int groupIndex)
    {
        if (groupUIs.TryGetValue(groupIndex, out SingleGroupUI groupUI))
            groupUI.SetGrayedOut(true);
    }

    public void LockValue(int groupIndex)
    {
        if (groupUIs.TryGetValue(groupIndex, out SingleGroupUI groupUI))
            groupUI.SetLocked();
    }

    public bool CheckLocked(int groupIndex)
    {
        if (groupUIs.TryGetValue(groupIndex, out SingleGroupUI groupUI))
            return groupUI.locked;
        return false;
    }
}
