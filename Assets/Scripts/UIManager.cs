using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIScripts;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Button = UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager currentGame;
    public TMP_Text highscore_score;
    public TMP_Text highscore_time;
    



    [SerializeField] private LevelButton levelButtons;
    
    private bool changingScene = false;
    private MainSceneHUD hudController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHighScore();
    }

    public void Initialise(GameManager game)
    {
        currentGame = game;
        UpdateHighScore();
        
    }

    private void UpdateHighScore()
    {
        highscore_score.text = SaveGameManager.CurrentHighScore.ToString();
        highscore_time.text = $@"{SaveGameManager.CurrentHighScoreTime:mm\:ss\:ff}";
        
    }
    

    public void OnClick(string scene)
    {
        if (changingScene) return;
        changingScene = true;
        
        levelButtons.ButtonAnimOver += LoadGameScene;
        StartCoroutine(levelButtons.ButtonAnimate(scene));

    }

    public void OnQuitButton()
    {
        if (changingScene) return;
        changingScene = true;
        
        currentGame.LoadScene("StartScene");
    }
    

    private void LoadGameScene(string sceneName)
    {
        levelButtons.ButtonAnimOver -= LoadGameScene;
        currentGame.LoadScene(sceneName);
        changingScene = false;
    }

    public void LoadMainGameUI()
    {
        hudController = GameObject.FindGameObjectWithTag("HUD").GetComponent<MainSceneHUD>();
        hudController.Initialize();
        hudController.QuitButton.onClick.AddListener(OnQuitButton);
    }

}
