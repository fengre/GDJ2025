using UnityEngine;
using TMPro;

public class ScoreTextUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private ScoreManager scoreManager;

    private void LateUpdate()
    {
        scoreText.text = $"Score: {scoreManager.GetTotalScore()}";
    }

}

