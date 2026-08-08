using TMPro;
using UnityEngine;

public class Score_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killCountText;
    private int score;
    private int numKills = 0;

    private void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        killCountText.text = "Kill Count: " + numKills;
    }

    public void AddScore(int amount)
    {
        score += amount;
        numKills++;
        UpdateScoreText();
    }
}
