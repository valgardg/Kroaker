using UnityEngine;
using TMPro;

public class LevelLabel : MonoBehaviour
{
    private TMP_Text text;
    public string displayPrefix = "Level:";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = displayPrefix + " 1";
    }

    public void DisplayLevel(int level)
    {
        text.text = displayPrefix + " " + level;
    }
}
