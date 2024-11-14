using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMState
{
    GhostScared,
    NormalBGM,
    GhostDying
}
public class GameManager : MonoBehaviour
{
    [SerializeField] public SaveGameManager saveManager;
    [SerializeField] public UIManager uiManager;
    
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        SceneManager.sceneLoaded += OnSceneLoaded;
        saveManager.Initialise(this);
        uiManager.Initialise(this);
        
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            uiManager.LoadMainGameUI();
        } 
        
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
}
