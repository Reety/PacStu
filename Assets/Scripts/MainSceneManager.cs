using System;
using System.Collections;
using System.Collections.Generic;
using LevelScripts;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    [SerializeField] public PacStudentController mcController;
    [FormerlySerializedAs("touristController")] [SerializeField] public GhostController ghostController;
    [SerializeField] BGM bgmController;
    [SerializeField] MainSceneHUD mainSceneHUD;
    [SerializeField] CherryController cherryController;

    public Transform PlayableCharacter => mcController.transform;

    private Collider2D mcCollider => mcController.GetComponent<Collider2D>();
    
    public static int CurrentScore = 0;

    public static TimeSpan CurrentTime = TimeSpan.Zero;
    // Start is called before the first frame update
    void Awake()
    {
        MSManager = this;
    }

    private void Update()
    {

    }

    private void Start()
    {
        //Initialise();
    }

    public void Initialise()
    {
        CurrentGameState = MainGameState.GameStarting;
        CurrentScore = 0;
        CurrentTime = TimeSpan.Zero;
        mcCollider.enabled = false;
        cherryController.enabled = false;
        
        ghostController.Initialise();
        mcController.Initialise();
        bgmController.Initialise();
        
        mainSceneHUD.Initialize();

    }

    public void StartGame()
    {
        mcCollider.enabled = true;
        bgmController.enabled = true;
        
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
