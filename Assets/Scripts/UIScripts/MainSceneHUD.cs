using System;
using System.Collections;
using System.Collections.Generic;
using CollisionScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneHUD : MonoBehaviour
{
    public GameObject Lives;

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
        
        PelletCollision.OnCollision += UpdateScore;
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
        
    }

    private void UpdateScore()
    {
        GameManager.CurrentScore += 10;
        Score.text = $"{GameManager.CurrentScore}";
    }
}
