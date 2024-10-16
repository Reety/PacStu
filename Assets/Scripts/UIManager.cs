using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameManager currentGame;
    public TMP_Text highscore_score;
    public TMP_Text highscore_time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHighscore(int score, string time)
    {
        highscore_score.text = $"{score}";
        highscore_time.text = time;

    }

    public void Initialise(GameManager game)
    {
        currentGame = game;
    }
}
