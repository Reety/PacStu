using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class L2BGMController : MonoBehaviour
{
    private AudioSource bgmPlayer;

    public static L2BGMController Instance;
    [SerializeField] private AudioClip normalBGM;
    [SerializeField] private AudioClip pacstuSlowBGM;
    [FormerlySerializedAs("studentScaredBGM")] [SerializeField] private AudioClip ghostScaredBGM;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (Level2Manager.L2Manager.CurrentGameState != MainGameState.GamePlaying) return;
        
        PlayBGM();
    }

    public void Initialise()
    {
        bgmPlayer = GetComponent<AudioSource>();
    }

    public void PlayBGM()
    {
        AudioClip clipToPlay = (Level2TouristController.Instance.BGMContext) switch
        {
            BGMState.NormalBGM => normalBGM,
            BGMState.GhostHypnotised => pacstuSlowBGM,
            BGMState.GhostScared => ghostScaredBGM,
            _ => bgmPlayer.clip
        };
        
        bgmPlayer.clip = clipToPlay;
        bgmPlayer.loop = true;
        if (!bgmPlayer.isPlaying) bgmPlayer.Play();
    }



}
