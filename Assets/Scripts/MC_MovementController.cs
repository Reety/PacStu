using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;


public class MC_MovementController : MonoBehaviour
{
    public Tilemap interactables;

    private float speed = 1.5f;
    
    private Tweener tweener;
    
    private Func<Vector3, Vector3> centre;
    private Predicate<Vector3> arrived;
    private Vector3 direction;

    private Animator animator;
    private static readonly int Right = Animator.StringToHash("Right");
    private static readonly int Left = Animator.StringToHash("Left");
    private static readonly int Up = Animator.StringToHash("Up");
    private static readonly int Down = Animator.StringToHash("Down");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        centre = x => interactables.GetCellCenterWorld(Vector3Int.FloorToInt(x));
        arrived = x => transform.position == centre(x);
        tweener = GetComponent<Tweener>();
        transform.position = interactables.GetCellCenterWorld(Vector3Int.FloorToInt(LevelOneMap.TopLeftStart));
        print($"{transform.position}");
        
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
    }

    private float DurationCalc(Vector3 end)
    {
        float distance = (transform.position - end).magnitude;
        return distance/speed;
    }

    private Vector3 nextCell(Vector3 direction)
    {
        return interactables.GetCellCenterWorld(Vector3Int.FloorToInt(transform.position + direction));
    }

    private void MoveRight()
    {
        Vector3 endPos = nextCell(Vector3.right);
        tweener.AddTween(transform,transform.position,endPos,DurationCalc(endPos));
    }

    private void Move()
    {
        Vector3 endPos = nextCell(direction);
        
        if (direction == Vector3.right)
            animator.SetTrigger(Right);
        else if (direction == Vector3.left)
            animator.SetTrigger(Left);
        else if (direction == Vector3.up)
            animator.SetTrigger(Up);
        else if (direction == Vector3.down)
            animator.SetTrigger(Down);
        
        tweener.AddTween(transform,transform.position,endPos,DurationCalc(endPos));
    }
}
