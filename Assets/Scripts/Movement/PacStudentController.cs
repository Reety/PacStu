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
    private AudioSource audioSrc;
    [SerializeField] private AudioClip moveAudio;
    [SerializeField] private AudioClip pelAudio;

    private int lastTrigger = 0;

    private KeyCode lastinput = KeyCode.None;
    private KeyCode currentinput = KeyCode.None;
    private static readonly int Idle = Animator.StringToHash("Idle");

    public int CurrentState => anim.GetCurrentAnimatorStateInfo(0).tagHash;
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
        audioSrc = GetComponent<AudioSource>();
        transform.position = levelmap.GetCentre(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) lastinput = GetInput();
        Move();
        
        if (!tweener.IsTweening && lastTrigger != Idle)
        {
            anim.SetTrigger(Idle);
            audioSrc.Stop();
            lastTrigger = Idle;
        }
    }

    private void Move()
    {
        if (tweener.IsTweening) return; 
        // if the last input can be navigated to then makes that the current input
        //currentinput = (!levelmap.IsWall(LastInputNextCell) && lastinput != KeyCode.None) ? lastinput : currentinput; 
        currentinput = (lastinput != KeyCode.None) ? lastinput : currentinput; 
        //if (levelmap.IsWall(CurrentInputNextCell) || currentinput == KeyCode.None) return; //if currentinput also can't be moved to then return 
        if (currentinput == KeyCode.None) return; 
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
        if (audioSrc.clip == toPlay && audioSrc.isPlaying) return;
        
        audioSrc.clip = toPlay;
        audioSrc.loop = true;
        audioSrc.pitch = (toPlay == moveAudio) ? MoveAudioPitch : PelAudioPitch;
        
        audioSrc.Play();
    }

    //maps keycode to direction
    private Vector3 GetMovementDirection(KeyCode input)
    {
        var direction = input switch
        {
            KeyCode.W => Vector3.up,
            KeyCode.S => Vector3.down,
            KeyCode.A => Vector3.left,
            KeyCode.D => Vector3.right,
            _ => Vector3.zero
        };

        return direction;
    }
    

}
