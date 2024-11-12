using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveGameManager saveManager;
    [SerializeField] private UIManager uiManager;

    public static int CurrentScore = 0;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        saveManager.Initialise(this);
        uiManager.Initialise(this);
        uiManager.ChangeHighscore(saveManager.CurrentHighScore,saveManager.CurrentHighScoreTime);
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            CurrentScore = 0;
            uiManager.LoadMainGameUI();
        }
    }
}
