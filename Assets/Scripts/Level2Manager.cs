using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager L2Manager;

    [SerializeField] public L2PacStuController mcController;
    [SerializeField] public Level2TouristController touristController;
    public Transform PlayableCharacter => mcController.transform;
    private Collider2D mcCollider => mcController.GetComponent<Collider2D>();
    public MainGameState CurrentGameState { get; set; }



    public static int CurrentScore = 0;

    public static TimeSpan CurrentTime = TimeSpan.Zero;
    // Start is called before the first frame update
    void Awake()
    {
        L2Manager = this;


    }

    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        CurrentGameState = MainGameState.GameStarting;
        CurrentScore = 0;
        CurrentTime = TimeSpan.Zero;
        mcCollider.enabled = true;

        touristController.Initialise();
        mcController.Initialise();
        CurrentGameState = MainGameState.GamePlaying;

    }

    public void GameOver()
    {
        CurrentGameState = MainGameState.GameOver;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
