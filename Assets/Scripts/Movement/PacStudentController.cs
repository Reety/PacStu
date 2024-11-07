using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LevelScripts;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private LevelMap levelmap;
    
    private Tweener tweener;
    private Animator anim;
    private AudioSource audio;
    [SerializeField] private AudioClip moveAudio;
    [SerializeField] private AudioClip pelAudio;
    
    private string lastTrigger = "";

    private KeyCode lastinput = KeyCode.None;
    private KeyCode currentinput = KeyCode.None;
    

    private Vector3 CurrentPosition => transform.position;
    /*
     * checks what cell the last input and current input entered by user goes to 
     */
    private Vector3 LastInputNextCell => levelmap.GetCentre(CurrentPosition + GetMovementDirection(lastinput));
    private Vector3 CurrentInputNextCell => levelmap.GetCentre(CurrentPosition + GetMovementDirection(currentinput));
    
    private float Duration => (CurrentPosition - CurrentInputNextCell).magnitude/speed;
    
    private float MoveAudioPitch => moveAudio.length / anim.GetCurrentAnimatorStateInfo(0).length;
    private float PelAudioPitch => (pelAudio.length/2) / Duration;


    // Start is called before the first frame update

    void Start()
    {
        tweener = GetComponent<Tweener>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        transform.position = levelmap.GetCentre(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) lastinput = GetInput();
        Move();
        
        if (!tweener.IsTweening && lastTrigger != "Idle")
        {
            anim.SetTrigger("Idle");
            audio.Stop();
            lastTrigger = "Idle";
        }
    }

    private void Move()
    {
        if (tweener.IsTweening) return; 
        // if the last input can be navigated to then makes that the current input
        currentinput = (!levelmap.IsWall(LastInputNextCell) && lastinput != KeyCode.None) ? lastinput : currentinput; 
        
        if (levelmap.IsWall(CurrentInputNextCell) || currentinput == KeyCode.None) return; //if currentinput also can't be moved to then return 
        
        //makes sure to only trigger the animation if direction changes to avoid weird things with trigger parametres
        if (lastTrigger != UtilClass.KeyToAnimation[currentinput])
        {
            anim.SetTrigger(UtilClass.KeyToAnimation[currentinput]);    
            lastTrigger = UtilClass.KeyToAnimation[currentinput];
        }

        //print($"moving {UtilClass.KeyToAnimation[currentinput]}");
        tweener.AddTween(transform, CurrentPosition, CurrentInputNextCell, Duration);
        PlayAudio();
    }

    private KeyCode GetInput()
    {
        if(Input.GetKeyDown(KeyCode.W)) { return KeyCode.W; }
        if (Input.GetKeyDown(KeyCode.S)) { return KeyCode.S; }
        if (Input.GetKeyDown(KeyCode.A)) { return KeyCode.A; }
        return Input.GetKeyDown(KeyCode.D) ? KeyCode.D : lastinput;
    }

    private void PlayAudio()
    {
        AudioClip toPlay = levelmap.IsPell(CurrentInputNextCell) ? pelAudio : moveAudio;
        if (audio.clip == toPlay && audio.isPlaying) return;
        
        audio.clip = toPlay;
        audio.loop = true;
        audio.pitch = (toPlay == moveAudio) ? MoveAudioPitch : PelAudioPitch;
        
        audio.Play();
    }

    //maps keycode to direction
    private Vector3 GetMovementDirection(KeyCode input)
    {
        Vector3 direction = Vector3.zero;
        
        switch (input)
        {
            case KeyCode.W:
                direction = Vector3.up;
                break;
            case KeyCode.S:
                direction = Vector3.down;
                break;
            case KeyCode.A:
                direction = Vector3.left;
                break;
            case KeyCode.D:
                direction = Vector3.right;
                break;
                
        }
        
        return direction;
    }
    

}
