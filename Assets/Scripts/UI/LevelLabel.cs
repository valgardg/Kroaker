using UnityEngine;
using TMPro;

public class LevelLabel : MonoBehaviour
{
    private TMP_Text text;
    public string displayPrefix = "Level:";
    
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
