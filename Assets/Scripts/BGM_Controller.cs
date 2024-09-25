using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource _bgmPlayer;
    
    public AudioClip introMusic;
    public AudioClip bgm;
    // Start is called before the first frame update
    void Start()
    {
        _bgmPlayer = GetComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.clip = introMusic;
        _bgmPlayer.Play();
        float introLength = introMusic.length + 0.5f;
        
        Invoke(nameof(StartBGM),introLength);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartBGM()
    {
        _bgmPlayer.clip = bgm;
        _bgmPlayer.loop = true;
        _bgmPlayer.Play();
    }
    
    
}
