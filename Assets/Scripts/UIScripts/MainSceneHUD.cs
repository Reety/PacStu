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
    public GameObject Lives;
    private int lives = 3;

    public TMP_Text Score;

    public TMP_Text Timer;

    public TMP_Text GhostTimer;

    public Button QuitButton;
    // Start is called before the first frame update
    

    public void Initialize()
    {
        Score.text = $"{GameManager.CurrentScore}";
        Timer.text = $"{TimeSpan.Zero:hh\\:mm\\:ss}";
        GhostTimer.text = "0";
        GhostTimer.gameObject.SetActive(false);
        QuitButton.onClick.RemoveAllListeners();
        
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
        Initialize();
    }
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GhostTimer.gameObject.activeSelf)
        {
            GhostTimer.text = TouristController.GhostCounter.ToString();
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
