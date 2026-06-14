using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Start()
    {
        // Move the panel to the middle of the screen
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void DisplayGameOverPanel(int level, int score, int highestLevel, int highScore)
    {
        levelText.text = $"You made it to level {level}!";
        scoreText.text = $"Your score was {score}!";
        highScoreText.text = $"HIGHSCORE: lvl {highestLevel}, {highScore}!";
    }
}