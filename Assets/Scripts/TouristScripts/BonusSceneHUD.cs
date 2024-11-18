using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusSceneHUD : MonoBehaviour
{
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TMP_Text Score;

    [SerializeField] private TMP_Text Timer;
    
    [SerializeField] private TMP_Text FinalScore;
    [SerializeField] private TMP_Text FinalTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = Level2Manager.L2Manager.CurrentScore.ToString();
    }

    public void Initialise()
    {
        Timer.enabled = false;
        Score.enabled = false;
        gameOver.SetActive(false);
        instructions.SetActive(true);
        
        
    }

    public void StartBonusRound()
    {
        Level2Manager.L2Manager.StartGame();
        Timer.enabled = true;
        Score.enabled = true;
        instructions.SetActive(false);
        StartCoroutine(StartGameTimer());
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene((int)GameScene.StartScene);
    }

    public void OnGameOver()
    {
        gameOver.SetActive(true);
        FinalScore.text = Score.text;
        FinalTime.text = Timer.text;
    }
    private IEnumerator StartGameTimer()
    {
        float time = 0;

        while (Level2Manager.L2Manager.CurrentGameState == MainGameState.GamePlaying)
        {
            time += Time.deltaTime;
            Level2Manager.L2Manager.CurrentTime = TimeSpan.FromSeconds(time);
            Timer.text = $"{Level2Manager.L2Manager.CurrentTime:mm\\:ss\\:ff}";
            yield return new WaitForSeconds(0.01f);
        }
    }
}
