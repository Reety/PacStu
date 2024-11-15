using System;
using System.Collections;
using System.Collections.Generic;
using CollisionScripts;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneHUD : MonoBehaviour
{
    private MainSceneManager mainScene;
    public GameObject Lives;
    private int lives = 3;

    public TMP_Text Score;

    public TMP_Text Timer;

    public TMP_Text GhostTimer;
    public TMP_Text GameCountDown;
    public TMP_Text GameOverText;
    
    // Start is called before the first frame update
    

    public void Initialize()
    {
        mainScene = MainSceneManager.MSManager;
        
        Score.text = $"{MainSceneManager.CurrentScore}";
        Timer.text = $"{TimeSpan.Zero:hh\\:mm\\:ss}";
        
        GhostTimer.text = "0";
        GhostTimer.gameObject.SetActive(false);
        
        PelletCollision.OnCollision += UpdateScorePellet;
        CherryCollision.OnCollision += UpdateScoreCherry;
        TouristController.OnGhostScared += OnGhostsScared;
        TouristController.OnGhostRecovered += OnGhostsRecovered;
        PacStudentController.OnPacStuDeath += OnPacStuDeath;
        
        StartCoroutine(StartGameCountDown());
    }

    private void OnPacStuDeath()
    {
        
        if (lives == 0) return;
        lives--;
        Lives.transform.GetChild(lives)?.gameObject.SetActive(false);
        if (lives == 0)
        {
            MainSceneManager.MSManager.GameOver();
        }
        
    }

    void Awake()
    {
        
        
    }
    

    void Start()
    {

        //Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (GhostTimer.gameObject.activeSelf)
        {
            GhostTimer.text = TouristController.GhostCounter.ToString();
        }
    }

    private IEnumerator StartGameCountDown()
    {
        int count = 3;
        Dictionary<int, Color> colorNo = new Dictionary<int, Color>()
        {
            [3] = Color.red,
            [2] = Color.yellow,
            [1] = Color.cyan,
            [0] = Color.green
        };

        while (count >= 0)
        {
            if (count == 0)
            {
                GameCountDown.text = "GO";
                GameCountDown.color = colorNo[count];
            }
            else
            {
                GameCountDown.text = count.ToString();
                GameCountDown.color = colorNo[count];
            }
            
            yield return new WaitForSeconds(1.0f);
            count--;
        }

        GameCountDown.enabled = false;
        mainScene.StartGame();
        StartCoroutine(StartGameTimer());
    }

    private IEnumerator StartGameTimer()
    {
        float time = 0;

        while (MainSceneManager.CurrentGameState == MainGameState.GamePlaying)
        {
            time += Time.deltaTime;
            MainSceneManager.CurrentTime = TimeSpan.FromSeconds(time);
            Timer.text = $"{MainSceneManager.CurrentTime:mm\\:ss\\:ff}";
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void UpdateScorePellet()
    {
        MainSceneManager.CurrentScore += 10;
        Score.text = $"{MainSceneManager.CurrentScore}";
    }
    

    private void UpdateScoreCherry()
    {
        MainSceneManager.CurrentScore += 100;
        Score.text = $"{MainSceneManager.CurrentScore}";
    }

    private void OnGhostsScared()
    {
        GhostTimer.gameObject.SetActive(true);
    }

    private void OnGhostsRecovered()
    {
        GhostTimer.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Instantiate(GameOverText.gameObject, transform);
        Invoke(nameof(OnQuitButton),3);
    }

    public void OnQuitButton()
    {
        SceneManager.LoadScene((int)GameScene.StartScene);
    }

    void OnDestroy()
    {
        PelletCollision.OnCollision -= UpdateScorePellet;
        CherryCollision.OnCollision -= UpdateScoreCherry;
        TouristController.OnGhostScared -= OnGhostsScared;
        TouristController.OnGhostRecovered -= OnGhostsRecovered;
        PacStudentController.OnPacStuDeath -= OnPacStuDeath;
        //print("destoyed mainsceheHUD");
    }
}
