using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TouristState
{
    TouristScared,
    TouristNormal
}
public class TouristController : MonoBehaviour
{
    public static TouristController instance;
    public GameObject TouristYellow;
    public GameObject TouristPink;
    public GameObject TouristBlue;
    public GameObject TouristRed;

    public static event Action OnGhostScared;
    public static event Action OnGhostRecovered;
    public TouristState CurrentState = TouristState.TouristNormal;
    
    public float counter = 0;
    public static int GhostCounter = 10;

    private List<Animator> animators;
    private List<Animator> scaredTourists = new List<Animator>();
    private Animator YellowAnimator => TouristYellow.GetComponent<Animator>();
    private Animator BlueAnimator => TouristBlue.GetComponent<Animator>();
    private Animator RedAnimator => TouristRed.GetComponent<Animator>();
    private Animator PinkAnimator => TouristPink.GetComponent<Animator>();
    
    public void Initialise()
    {
        instance = this;
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
        CurrentState = (scaredTourists.Count == 0) ? TouristState.TouristNormal : TouristState.TouristScared;


    }
    

    private void TouristScared()
    {
        scaredTourists.Clear();
        foreach (Animator animator in animators)
        {
            animator.SetBool("Scared", true);
            scaredTourists.Add(animator);
        }

        //CurrentState = TouristState.TouristScared;
        OnGhostScared?.Invoke();
        StartCoroutine(StartGhostCounter());
    }

    private IEnumerator StartGhostCounter()
    {
        while (GhostCounter > 0) 
        {
            yield return new WaitForSeconds(1f);
            GhostCounter--;
            if(scaredTourists.Count == 0) break;
            if (GhostCounter == 3) animators.ForEach(x => x.SetBool("Recovering",true));
        }
        
        OnGhostRecovered?.Invoke();
        scaredTourists.ForEach(x => x.SetBool("Recovering",false));
        scaredTourists.ForEach(x => x.SetBool("Scared",false));
        scaredTourists.Clear();
        //CurrentState = TouristState.TouristNormal;
        GhostCounter = 10;

    }

    private void OnDestroy()
    {
        PPCollision.OnCollision -= TouristScared;
    }

    public void KillTourist(GameObject tourist)
    {
        Animator touristToKill = animators.FirstOrDefault(x => x.gameObject == tourist);
        Collider2D colliderToKill = tourist.GetComponent<Collider2D>();
        colliderToKill.enabled = false;
        scaredTourists.Remove(touristToKill);
        touristToKill.SetBool(UtilClass.Death,true);
        touristToKill.SetBool("Recovering",false);
        touristToKill.SetBool("Scared",false);
        BGM.instance.PlayGhostDead();
        StartCoroutine(DeathCounter(touristToKill,colliderToKill));
    }

    private IEnumerator DeathCounter(Animator touristAni, Collider2D touristCollider)
    {
        yield return new WaitForSeconds(5);
        touristAni.SetBool(UtilClass.Death,false);
        touristCollider.enabled = true;
    }

    public bool TouristScared(GameObject tourist)
    {
        if (scaredTourists.Count == 0) return false;
        
        return scaredTourists.Exists(x => x.gameObject == tourist);
    }
}
