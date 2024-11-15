using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MainGameState
{
    GameStarting,
    GamePlaying,
    GameOver
}
public class MainSceneManager : MonoBehaviour
{
    
    public static MainSceneManager MSManager;
    
    public static MainGameState CurrentGameState;
    
    [SerializeField] private PacStudentController mcController;
    [SerializeField] private TouristController touristController;
    [SerializeField] BGM bgmController;
    [SerializeField] MainSceneHUD mainSceneHUD;
    [SerializeField] CherryController cherryController;

    private Collider2D mcCollider => mcController.GetComponent<Collider2D>();
    
    public static int CurrentScore = 0;

    public static TimeSpan CurrentTime = TimeSpan.Zero;
    // Start is called before the first frame update
    void Awake()
    {
        MSManager = this;
    }

    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        CurrentGameState = MainGameState.GameStarting;
        
        mcController.enabled = false;
        mcCollider.enabled = false;
        touristController.enabled = false;
        bgmController.enabled = false;
        cherryController.enabled = false;
        
        mainSceneHUD.Initialize();

    }

    public void StartGame()
    {
        mcController.enabled = true;
        mcCollider.enabled = true;
        touristController.enabled = true;
        bgmController.enabled = true;
        
        mcController.Initialise();
        touristController.Initialise();
        bgmController.Initialise();
        cherryController.enabled = true;

        CurrentGameState = MainGameState.GamePlaying;
    }

    public void GameOver()
    {
        CurrentGameState = MainGameState.GameOver;
        cherryController.enabled = false;
        SaveGameManager.UpdateHighScore(CurrentScore,CurrentTime);
        mainSceneHUD.GameOver();
    }
}
