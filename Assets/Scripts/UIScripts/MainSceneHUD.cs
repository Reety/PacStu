using System;
using System.Collections;
using System.Collections.Generic;
using CollisionScripts;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
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

    public Button QuitButton;
    // Start is called before the first frame update
    

    public void Initialize()
    {
        Score.text = $"{GameManager.CurrentScore}";
        Timer.text = $"{TimeSpan.Zero:hh\\:mm\\:ss}";
        GhostTimer.text = "0";
        GhostTimer.gameObject.SetActive(false);
        QuitButton.onClick.RemoveAllListeners();

        mainScene = new MainSceneManager(GameObject.FindGameObjectWithTag("Player").GetComponent<PacStudentController>(),GameObject.FindGameObjectWithTag("EnemyController").GetComponent<TouristController>());
        
        PelletCollision.OnCollision += UpdateScorePellet;
        CherryCollision.OnCollision += UpdateScoreCherry;
        TouristController.OnGhostScared += OnGhostsScared;
        TouristController.OnGhostRecovered += OnGhostsRecovered;
        PacStudentController.OnPacStuDeath += OnPacStuDeath;
    }

    private void OnPacStuDeath()
    {
        if (Lives.transform.childCount == 0) return;
        Lives.transform.GetChild(lives - 1)?.gameObject.SetActive(false);
        lives--;
    }

    void Awake()
    {
        
        
    }
    

    void Start()
    {
        Initialize();
        StartCoroutine(StartGameCountDown());
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

        while (lives != 0)
        {
            time += Time.deltaTime;
            Timer.text = $"{TimeSpan.FromSeconds(time):mm\\:ss\\:ff}";
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void UpdateScorePellet()
    {
        GameManager.CurrentScore += 10;
        Score.text = $"{GameManager.CurrentScore}";
    }
    

    private void UpdateScoreCherry()
    {
        GameManager.CurrentScore += 100;
        Score.text = $"{GameManager.CurrentScore}";
    }

    private void OnGhostsScared()
    {
        GhostTimer.gameObject.SetActive(true);
    }

    private void OnGhostsRecovered()
    {
        GhostTimer.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        PelletCollision.OnCollision -= UpdateScorePellet;
        CherryCollision.OnCollision -= UpdateScoreCherry;
        TouristController.OnGhostScared -= OnGhostsScared;
        TouristController.OnGhostRecovered -= OnGhostsRecovered;
        PacStudentController.OnPacStuDeath -= OnPacStuDeath;
    }
}
