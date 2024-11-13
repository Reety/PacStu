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
    public AudioClip GhostDead;

    private bool updateBGM = true;
    
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
        if (!updateBGM) return;
        
        bgmPlayer.clip = TouristController.instance.CurrentState switch
        {
            TouristState.TouristNormal when bgmPlayer.clip != NormalBGM => NormalBGM,
            TouristState.TouristScared when bgmPlayer.clip != GhostScared => GhostScared,
            _ => bgmPlayer.clip
        };

        bgmPlayer.loop = true;
        if (!bgmPlayer.isPlaying) bgmPlayer.Play();
    }

    public void PlayGhostDead()
    {
        bgmPlayer.clip = GhostDead;
        updateBGM = false;
        bgmPlayer.loop = false;
        StartCoroutine(StartGhostDead());
    }

    private IEnumerator StartGhostDead()
    {
        bgmPlayer.Play();
        yield return new WaitForSeconds(GhostDead.length);
        bgmPlayer.Stop();
        updateBGM = true;
    }
    


}
