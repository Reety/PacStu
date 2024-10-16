using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    private GameManager currentGame;

    private const string highScore = "high score";
    
    public int CurrentHighScore => !PlayerPrefs.HasKey(highScore) ? 0 : PlayerPrefs.GetInt(highScore);
    public string CurrentHighScoreTime => !PlayerPrefs.HasKey(highScore) ? "00:00:00" : PlayerPrefs.GetString(highScore);

    
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
        PlayerPrefs.SetInt(highScore,0);
        PlayerPrefs.SetString(highScore,"00:00:00");
        PlayerPrefs.Save();

    }
    
    
}
