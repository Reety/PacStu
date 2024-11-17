using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristBase : MonoBehaviour
{
    public TouristNo TouristType;
    public Vector3 LastPosition;
    public int LastTrigger = 1;
    
    public Vector3 SpawnPoint;
    public Animator TouristAnimator => GetComponent<Animator>();
    public int CurrentState => TouristAnimator.GetCurrentAnimatorStateInfo(0).tagHash;
    //public Tweener Tweener;
    public Vector3 Position => transform.position;

    public void Initialise(TouristNo ttype, Vector3 spawnPoint)
    { 
        TouristType = ttype;
        SpawnPoint = spawnPoint;
        //Tweener = gameObject.AddComponent<Tweener>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
