using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TMP_Text text;
    public string displayPrefix = "Score:";

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = displayPrefix + " 0";
    }

    public void DisplayScore(int score)
    {
        text.text = displayPrefix + " " + score;
    }
}
