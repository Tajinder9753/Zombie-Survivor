using TMPro;
using UnityEngine;

public class Score_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private Enemy_Manager enemyManager;
    [SerializeField] private int difficultyIncreaseInterval = 10;
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

        //every 10 kills increase the difficulty slightly through changing something 
        if (numKills % difficultyIncreaseInterval == 0)
        {
            enemyManager.IncreaseDifficulty();
        }
    }
}
