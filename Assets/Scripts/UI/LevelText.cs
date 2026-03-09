using UnityEngine;
using TMPro;
using System.Collections;

public class LevelText : MonoBehaviour
{
    public float displayDuration = 2.0f;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text scoreText;

    public string levelDisplayPrefix = "Level";
    public string scoreDisplayPrefix = "+";

    void Awake()
    {
        levelText.text = "";
        scoreText.text = "";
    }

    public void DisplayLevel(int level, int levelScore = 0)
    {
        StartCoroutine(DisplayLevelCoroutine(level, levelScore));
    }

    IEnumerator DisplayLevelCoroutine(int level, int levelScore = 0)
    {
        levelText.text = levelDisplayPrefix + " " + "" + level;
        // if level brings a score, display it with the level
        if(levelScore > 0) { scoreText.text = scoreDisplayPrefix + levelScore; }
        yield return new WaitForSeconds(displayDuration);
        levelText.text = "";
        scoreText.text = "";
    }
}
