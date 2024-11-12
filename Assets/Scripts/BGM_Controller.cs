using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BGM : MonoBehaviour
{
    public static BGM instance;
    
    public BGMState bgmState = BGMState.NormalBGM;
    private AudioSource bgmPlayer;
    //public AudioClip introMusic;
    public AudioClip NormalBGM;
    public AudioClip GhostScared;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        bgmPlayer = GetComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.clip = NormalBGM;
        bgmPlayer.loop = true;
        bgmPlayer.Play();
        //float introLength = introMusic.length + 0.5f;

        //StartCoroutine(StartBGM(introLength));

    }

    // Update is called once per frame
    void Update()
    {
        bgmPlayer.clip = bgmState switch
        {
            BGMState.NormalBGM when bgmPlayer.clip != NormalBGM => NormalBGM,
            BGMState.GhostScared when bgmPlayer.clip != GhostScared => GhostScared,
            _ => bgmPlayer.clip
        };
        
        if (!bgmPlayer.isPlaying) bgmPlayer.Play();
    }
    
    
}
