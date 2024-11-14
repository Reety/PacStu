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
public class MainSceneManager
{
    public static MainSceneManager MSManager;
    public static MainGameState currentState;
    [SerializeField] private PacStudentController mcController;

    [SerializeField] private TouristController touristController;

    private BGM bgmController;

    private MainSceneHUD mainSceneHUD;

    private CherryController cherryController;

    private Collider2D pelCollider;

    private Tilemap pelMap;
    
    public static int CurrentScore = 0;

    public static TimeSpan CurrentTime = TimeSpan.Zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public MainSceneManager(PacStudentController mc, TouristController tc, MainSceneHUD hud)
    {
        MSManager = this;
        currentState = MainGameState.GameStarting;
        mainSceneHUD = hud;
        mcController = mc;
        touristController = tc;
        bgmController = BGM.instance;
        mcController.enabled = false;
        touristController.enabled = false;
        bgmController.enabled = false;

        cherryController = GameObject.FindGameObjectWithTag("CherryController").GetComponent<CherryController>();
        pelMap = GameObject.FindGameObjectWithTag("Interactables").GetComponent<Tilemap>();
        pelCollider = GameObject.FindGameObjectWithTag("Interactables").GetComponent<Collider2D>();

        cherryController.enabled = false;
        pelCollider.enabled = false;
    }

    public void StartGame()
    {
        mcController.enabled = true;
        touristController.enabled = true;
        bgmController.enabled = true;
        mcController.Initialise();
        touristController.Initialise();
        bgmController.Initialise();

        cherryController.enabled = true;
        pelCollider.enabled = true;
        currentState = MainGameState.GamePlaying;
    }

    public void GameOver()
    {
        mainSceneHUD.GameOver();
        currentState = MainGameState.GameOver;
        Debug.Log(CurrentTime.TotalSeconds);
        Debug.Log(CurrentScore);
        SaveGameManager.UpdateHighScore(CurrentScore,CurrentTime);
        // Update is called once per frame
    }
}
