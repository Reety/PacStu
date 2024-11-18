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

    public void Initialise()
    {
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        highscore_score.text = SaveGameManager.CurrentHighScore.ToString();
        highscore_time.text = $@"{SaveGameManager.CurrentHighScoreTime:mm\:ss\:ff}";
        
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    

    public void OnClick(int scene)
    {
        if (changingScene) return;
        changingScene = true;
        
        levelButtons.ButtonAnimOver += LoadGameScene;
        StartCoroutine(levelButtons.ButtonAnimate(scene));

    }
    
    

    private void LoadGameScene(int sceneIndex)
    {
        levelButtons.ButtonAnimOver -= LoadGameScene;
        SceneManager.LoadScene(sceneIndex);
        changingScene = false;
    }
    

}
