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

    public void OnClick(string scene)
    {
        if (changingScene) return;
        changingScene = true;
        
        levelButtons.ButtonAnimOver += LoadScene;
        StartCoroutine(levelButtons.ButtonAnimate(scene));

    }

    public void OnQuitButton()
    {
        if (changingScene) return;
        changingScene = true;
        
        currentGame.LoadScene("StartScene");
    }
    

    private void LoadScene(string sceneName)
    {
        levelButtons.ButtonAnimOver -= LoadScene;
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
