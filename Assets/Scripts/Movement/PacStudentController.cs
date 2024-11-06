using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LevelScripts;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] public Transform PacStudent;
    private Tweener tweener;
    
    private KeyCode lastinput;
    private KeyCode currentinput;
    
    private LevelMap levelmap;

    private Vector3 LastInputNextCell => levelmap.GetCentre(transform.position + GetMovementDirection(currentinput));
    private Vector3 CurrentInputNextCell => levelmap.GetCentre(transform.position + GetMovementDirection(currentinput));

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) lastinput = GetInput();
        
        

    }

    private void Move()
    {
        if (tweener.IsTweening) return;
        
        currentinput = !levelmap.IsWall(LastInputNextCell) ? lastinput : currentinput;
        
        if (levelmap.IsWall(CurrentInputNextCell)) return;
        
        
    }

    private KeyCode GetInput()
    {
        if(Input.GetKey(KeyCode.W)) { return KeyCode.W; }
        if (Input.GetKey(KeyCode.S)) { return KeyCode.S; }
        if (Input.GetKey(KeyCode.A)) { return KeyCode.A; }
        return Input.GetKey(KeyCode.D) ? KeyCode.D : lastinput;
    }

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
