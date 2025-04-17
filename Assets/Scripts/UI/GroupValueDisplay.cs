using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GroupValueDisplay : MonoBehaviour
{
    public TextMeshProUGUI redValueText;
    public TextMeshProUGUI blueValueText;
    public TextMeshProUGUI greenValueText;
    public TextMeshProUGUI yellowValueText;

    private void Update()
    {
        redValueText.text = $"Red: {GroupManager.Instance.GetGroupValue(Color.red)}";
        blueValueText.text = $"Blue: {GroupManager.Instance.GetGroupValue(Color.blue)}";
        greenValueText.text = $"Green: {GroupManager.Instance.GetGroupValue(Color.green)}";
        yellowValueText.text = $"Yellow: {GroupManager.Instance.GetGroupValue(new Color(1,1,0,1))}";
    }
}
