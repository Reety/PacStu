using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    StartScene = 0,
    MainScene = 1
}
public enum BGMState
{
    GhostScared,
    NormalBGM,
    GhostDead
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private UIManager uiManager;
    
    private MainSceneManager mainSceneManager;
   
    
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance == null) 
        {
            DontDestroyOnLoad(gameObject); 
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else if(Instance != this) 
        {
            Destroy(gameObject); 
        }
        
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case ((int)GameScene.StartScene) :
                uiManager = GameObject.FindGameObjectWithTag("MainMenuUI").GetComponent<UIManager>();
                uiManager.Initialise();
                break;
            case ((int)GameScene.MainScene) :
                //Debug.Log("loading main scene");
                mainSceneManager = MainSceneManager.MSManager;
                mainSceneManager.Initialise();
                break;
        }

        
    }

    private void OnDestroy()
    {
        //Debug.Log("Destroyed");
    }
}
