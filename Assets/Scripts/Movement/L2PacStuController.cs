using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LevelScripts;
using Movement;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class L2PacStuController : MonoBehaviour, IPlayableCharacter
{
    private float speed = 5.5f;

    private IMapController levelmap;

    public static event Action OnPacStuDeath;

    private Collider2D playerCollider;
    private Tweener tweener;
    private Animator anim;
    private AudioSource audioSrc;
    private ParticleController particle;
    [SerializeField] private AudioClip moveAudio;
    [SerializeField] private AudioClip pelAudio;
    [SerializeField] private AudioClip hitWall;
    [SerializeField] private AudioClip dying;

    private int lastTrigger = 0;
    private bool pacstuDying = false;
        

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

    private Vector3 lastLerpablePosition;
    private float Duration => (CurrentPosition - CurrentInputNextCell).magnitude/speed;
    private float HitWallDuration => (CurrentPosition - lastLerpablePosition).magnitude/speed;
    private float MoveAudioPitch => moveAudio.length / anim.GetCurrentAnimatorStateInfo(0).length;
    private float PelAudioPitch => pelAudio.length / Duration;


    // Start is called before the first frame update

    public void Initialise()
    {
        levelmap = Level2MapController.Instance;
        tweener = GetComponent<Tweener>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        particle = GetComponent<ParticleController>();
        playerCollider = GetComponent<Collider2D>();
        transform.position = levelmap.PlayerStartPos;
    }

    // Update is called once per frame
    void Update()
    {
 
        if (Level2Manager.L2Manager.CurrentGameState != MainGameState.GamePlaying) return;
        
        if (Input.anyKeyDown) lastinput = GetInput();
        Move();
        
        
        if (!tweener.IsTweening && !pacstuDying)
        {
            if (CurrentState != Idle) anim.SetTrigger(Idle);
            audioSrc.loop = false;
            lastTrigger = Idle;
        }
    }



    private void Move()
    {
        if (tweener.IsTweening || pacstuDying) return; 
        
        // if the last input can be navigated to then makes that the current input
        currentinput = (!levelmap.IsWall(LastInputNextCell) && lastinput != KeyCode.None) ? lastinput : currentinput;
        
        if (!levelmap.IsWall(CurrentPosition))
        {
            lastLerpablePosition = CurrentPosition;
        }
            //return; //if currentinput also can't be moved to then return 
        if (currentinput == KeyCode.None) return; 
        //makes sure to only trigger the animation if direction changes to avoid weird things with trigger parametres
        if (lastTrigger != UtilClass.KeyToAnimation[currentinput])
        {
            anim.SetTrigger(UtilClass.KeyToAnimation[currentinput]);    
            lastTrigger = UtilClass.KeyToAnimation[currentinput];
        }
        //print("moving");
        //print($"moving {UtilClass.KeyToAnimation[currentinput]}");
        tweener.AddTween(transform, CurrentPosition, CurrentInputNextCell, Duration);
        if (!particle.emitParticle) particle.StartParticle();
        
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
        AudioClip toPlay = moveAudio;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        //print("Collision!");
        if (other.gameObject.CompareTag("Wall"))
        {
            SetSpecialAudio(hitWall);
            tweener.CancelTween(transform); 
            particle.StopParticle();
            tweener.AddTween(transform, CurrentPosition, lastLerpablePosition, HitWallDuration);
            currentinput = KeyCode.None;
            audioSrc.Play();
            StartCoroutine(Level2TouristController.Instance.SetAlert());
        }
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyCollision(other.collider);
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Teleport"))
            tweener.CancelTween(transform);
        
        
        if (other.gameObject.CompareTag("PowerPellet"))
        {
            SetSpecialAudio(pelAudio);
            audioSrc.Play();
            Destroy(other.gameObject);
            Level2TouristController.Instance.SlowDownTourists();
        }
        
        if (other.gameObject.CompareTag("Flashlight"))
        {
            StartCoroutine(Death());
        }
    }

    private void EnemyCollision(Collider2D other)
    {
        Level2TouristController.Instance.KillTourist(other.gameObject);
    }

    private IEnumerator Death()
    {
        tweener.CancelTween(transform);
        particle.StopParticle();
        pacstuDying = true;
        playerCollider.enabled = false;
        anim.SetTrigger(UtilClass.Death);
        particle.PlayDeathParticle();
        SetSpecialAudio(dying);
        audioSrc.Play();
        OnPacStuDeath?.Invoke();
        while (CurrentState != UtilClass.Respawn)
        {
            yield return null;
        }
        
        Reset();
        
        while (CurrentState != UtilClass.Idle)
        {
            yield return null;
        }
        pacstuDying = false;
        playerCollider.enabled = true;
    }

    private void Reset()
    {
        transform.position = levelmap.PlayerStartPos;
        lastinput = KeyCode.None;
        currentinput = KeyCode.None;
        //anim.SetTrigger(UtilClass.Idle);
    }

    private void SetSpecialAudio(AudioClip clip)
    {
        audioSrc.Stop();
        audioSrc.loop = false;
        audioSrc.clip = clip;
        audioSrc.pitch = 1;
    }
}
