using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouristController : MonoBehaviour
{
    public GameObject TouristYellow;
    public GameObject TouristPink;
    public GameObject TouristBlue;
    public GameObject TouristRed;

    public static event Action OnGhostScared;
    public static event Action OnGhostRecovered;
    
    public float counter = 0;
    public static int GhostCounter = 10;

    private List<Animator> animators;
    private Animator YellowAnimator => TouristYellow.GetComponent<Animator>();
    private Animator BlueAnimator => TouristBlue.GetComponent<Animator>();
    private Animator RedAnimator => TouristRed.GetComponent<Animator>();
    private Animator PinkAnimator => TouristPink.GetComponent<Animator>();
    
    void Awake()
    {
        animators = new List<Animator>();
        PPCollision.OnCollision += TouristScared;
        animators.Add(YellowAnimator);
        animators.Add(BlueAnimator);
        animators.Add(PinkAnimator);
        animators.Add(RedAnimator);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }
    

    private void TouristScared()
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("Scared", true);
        }

        BGM.instance.bgmState = BGMState.GhostScared;
        OnGhostScared?.Invoke();
        StartCoroutine(StartGhostCounter());
    }

    private IEnumerator StartGhostCounter()
    {
        while (GhostCounter > 0) 
        {
            yield return new WaitForSeconds(1f);
            GhostCounter--;
            if (GhostCounter == 3) animators.ForEach(x => x.SetBool("Recovering",true));
        }
        
        OnGhostRecovered?.Invoke();
        BGM.instance.bgmState = BGMState.NormalBGM;
        animators.ForEach(x => x.SetBool("Recovering",false));
        animators.ForEach(x => x.SetBool("Scared",false));
        GhostCounter = 10;

    }

    private void OnDestroy()
    {
        PPCollision.OnCollision -= TouristScared;
    }
}
