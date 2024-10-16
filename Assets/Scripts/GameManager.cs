using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveGameManager saveManager;
    [SerializeField] private UIManager uiManager;
    // Start is called before the first frame update
    private void Awake()
    {
        saveManager.Initialise(this);
        uiManager.Initialise(this);
        uiManager.ChangeHighscore(saveManager.CurrentHighScore,saveManager.CurrentHighScoreTime);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
