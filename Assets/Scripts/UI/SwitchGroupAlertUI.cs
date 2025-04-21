using UnityEngine;

public class SwitchGroupAlertUI : AlertUI
{
    public static SwitchGroupAlertUI Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
}
