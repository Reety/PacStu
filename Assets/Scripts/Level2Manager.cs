using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager L2Manager;

    [SerializeField] public L2PacStuController mcController;
    [SerializeField] public Level2TouristController touristController;
    [SerializeField] public BonusSceneHUD hudController;
    [SerializeField] public L2BGMController BGMController;
    public Transform PlayableCharacter => mcController.transform;
    private Collider2D mcCollider => mcController.GetComponent<Collider2D>();
    public MainGameState CurrentGameState { get; set; }



    public int CurrentScore = 0;

    public TimeSpan CurrentTime = TimeSpan.Zero;
    // Start is called before the first frame update
    void Awake()
    {
        L2Manager = this;

    }

    private void Start()
    {

    }

    public void Initialise()
    {
        CurrentGameState = MainGameState.GameStarting;
        CurrentScore = 0;
        CurrentTime = TimeSpan.Zero;
        
        hudController.Initialise();
        touristController.Initialise();
        mcController.Initialise();
        BGMController.Initialise();
        mcCollider.enabled = false;
        
    }

    public void StartGame()
    {
        mcCollider.enabled = true;
        Level2TouristController.OnGhostKilled += GainPoint;
        CurrentGameState = MainGameState.GamePlaying;
    }

    public void GameOver()
    {
        CurrentGameState = MainGameState.GameOver;
        StartCoroutine(EndingGame());

    }

    IEnumerator EndingGame()
    {
        hudController.OnGameOver();
        yield return new WaitForSeconds(3.0f);
        hudController.QuitGame();
    }
    public void GainPoint()
    {
        int point = touristController.CurrentState == TouristState.TouristScared ? 30 : 10;
        if (touristController.BGMContext == BGMState.GhostHypnotised) point += 10;
        
        CurrentScore += point;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Level2TouristController.OnGhostKilled -= GainPoint;

    }
}
