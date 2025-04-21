using UnityEngine;

[System.Serializable]
public class SongData
{
    public string songTitle;
    public string bpm;
    public int difficulty;
    public string groupCSVName; // used to load Resources/GroupData_XXX.csv
}