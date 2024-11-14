using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    private GameManager currentGame;

    private const string highScore = "high score";
    private const string highScoreTime = "high score time";
    
    public static int CurrentHighScore => !PlayerPrefs.HasKey(highScore) ? 0 : PlayerPrefs.GetInt(highScore);
    public static TimeSpan CurrentHighScoreTime => !PlayerPrefs.HasKey(highScoreTime) ? TimeSpan.Zero : TimeSpan.FromSeconds(PlayerPrefs.GetFloat(highScoreTime));

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void Initialise(GameManager game)
    {
        currentGame = game;
        if (!PlayerPrefs.HasKey(highScore))
        {
            PlayerPrefs.SetInt(highScore,0);
            PlayerPrefs.SetFloat(highScoreTime,0);
        }

        //PlayerPrefs.Save();

    }

    public static void UpdateHighScore(int newScore, TimeSpan newTime)
    {
        if (newScore < CurrentHighScore) return;

        if (newScore == CurrentHighScore && newTime > CurrentHighScoreTime) return;
        
        PlayerPrefs.SetInt(highScore,newScore);

        float newTimef = (float)newTime.TotalSeconds;
        PlayerPrefs.SetFloat(highScoreTime,newTimef);

        PlayerPrefs.Save();

    }
    
    
}
