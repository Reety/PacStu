using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BGM : MonoBehaviour
{
    public static BGM instance;
    
    [FormerlySerializedAs("bgmState")] public BGMState BGMState = BGMState.NormalBGM;
    private AudioSource bgmPlayer;
    //public AudioClip introMusic;
    public AudioClip NormalBGM;
    public AudioClip GhostScared;
    public AudioClip GhostDead;

    private bool updateBGM = true;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void Initialise()
    {
        bgmPlayer = GetComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.clip = NormalBGM;
        bgmPlayer.loop = true;
        //bgmPlayer.Play();
        //float introLength = introMusic.length + 0.5f;

        //StartCoroutine(StartBGM(introLength));

    }

    // Update is called once per frame
    void Update()
    {
        if (MainSceneManager.CurrentGameState == MainGameState.GameStarting) return;
        if (!updateBGM) return;
        
        bgmPlayer.clip = TouristController.Instance.BGMContext switch
        {
            BGMState.NormalBGM => NormalBGM,
            BGMState.GhostScared => GhostScared,
            BGMState.GhostDead => GhostDead,
            _ => bgmPlayer.clip
        };

        bgmPlayer.loop = true;
        if (!bgmPlayer.isPlaying) bgmPlayer.Play();
    }
    


}
