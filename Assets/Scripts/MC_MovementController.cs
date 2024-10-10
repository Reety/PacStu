using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;


public class McMovementController : MonoBehaviour
{
    public Tilemap interactables;
    private Tweener tweener;
    private new AudioSource audio;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip moveSound;
    

    private float speed = 1.5f;
    
    
    private Func<Vector3, Vector3> centre;
    private Predicate<Vector3> arrived;
    private Vector3 direction;

    private Animator animator;
    private static readonly int Right = Animator.StringToHash("Right");
    private static readonly int Left = Animator.StringToHash("Left");
    private static readonly int Up = Animator.StringToHash("Up");
    private static readonly int Down = Animator.StringToHash("Down");

    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        
        centre = x => interactables.GetCellCenterWorld(Vector3Int.FloorToInt(x));
        arrived = x => transform.position == centre(x);
        tweener = GetComponent<Tweener>();
        transform.position = interactables.GetCellCenterWorld(Vector3Int.FloorToInt(LevelOneMap.TopLeftStart));
        prevPos = transform.position;
        //print($"{transform.position}");

    }

    // Update is called once per frame
    void Update()
    {
        CycleLeftQuad();
    }
    

    private void CycleLeftQuad()
    {
        if (arrived(LevelOneMap.TopLeftStart))
            direction = Vector3.right;
        if (arrived(LevelOneMap.MapPositions[1, 12]))
            direction = Vector3.down;
        if (arrived(LevelOneMap.MapPositions[5,12]))
            direction = Vector3.left;
        if (arrived(LevelOneMap.MapPositions[5, 9]))
            direction = Vector3.down;
        if (arrived(LevelOneMap.MapPositions[8, 9]))
            direction = Vector3.right;
        if (arrived(LevelOneMap.MapPositions[8, 12]))
            direction = Vector3.down;
        if (arrived(LevelOneMap.MapPositions[11, 12]))
            direction = Vector3.left;
        if (arrived(LevelOneMap.MapPositions[11, 9]))
            direction = Vector3.down;
        if (arrived(LevelOneMap.MapPositions[14, 9]))
            direction = Vector3.left;
        if (arrived(LevelOneMap.MapPositions[14, 6]))
            direction = Vector3.up;
        if (arrived(LevelOneMap.MapPositions[8, 6]))
            direction = Vector3.left;
        if (arrived(LevelOneMap.MapPositions[8, 1]))
            direction = Vector3.up;

        Move();
        MovementAudio();
    }

    private float DurationCalc(Vector3 end)
    {
        float distance = (transform.position - end).magnitude;
        return distance/speed;
    }

    private Vector3 NextCell(Vector3 dir)
    {
        return interactables.GetCellCenterWorld(Vector3Int.FloorToInt(transform.position + dir));
    }

    private void MoveRight()
    {
        Vector3 endPos = NextCell(Vector3.right);
        tweener.AddTween(transform,transform.position,endPos,DurationCalc(endPos));
    }

    private void Move()
    {
        Vector3 endPos = NextCell(direction);
        
        tweener.AddTween(transform,transform.position,endPos,DurationCalc(endPos));
        
        if (direction == Vector3.right)
            animator.SetTrigger(Right);
        else if (direction == Vector3.left)
            animator.SetTrigger(Left);
        else if (direction == Vector3.up)
            animator.SetTrigger(Up);
        else if (direction == Vector3.down)
            animator.SetTrigger(Down);
        

    }

    private void MovementAudio()
    {
        if (tweener.IsTweening && !audio.isPlaying)
        {
            //print("audio start");
            audio.clip = moveSound;
            bgmSource.volume = 0.5f;
            audio.pitch = MovementSoundSpeed();
            audio.volume = 0.2f;
            audio.Play();
        } else if (!tweener.IsTweening && audio.isPlaying)
        {
            //print("audio end");
            audio.Pause();
            bgmSource.volume = 1.0f;
        }
    }

    private float MovementSoundSpeed()
    {
        float moveLength = animator.GetCurrentAnimatorStateInfo(0).length;
        return moveSound.length/moveLength;
    }
    
}
