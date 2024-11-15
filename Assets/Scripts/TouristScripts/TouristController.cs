using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LevelScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum TouristState
{
    TouristScared,
    TouristNormal
}

public enum TouristNo
{
    TouristBlue = 0,
    TouristPink = 1,
    TouristRed = 2,
    TouristYellow = 3,
}
public class TouristController : MonoBehaviour
{
    private float speed = 2f;
    private float Duration => 1 / speed;
    public static TouristController Instance;
    private Tweener touristTweener;
    
    public GameObject TouristYellow;
    public GameObject TouristPink;
    public GameObject TouristBlue;
    public GameObject TouristRed;
    

    private TouristBase[] tourists = new TouristBase[4];

    public static event Action OnGhostScared;
    public static event Action OnGhostRecovered;
    
    public TouristState CurrentState = TouristState.TouristNormal;
    
    public float counter = 0;
    public static int GhostCounter = 10;

    private List<Transform> touristTrans => tourists.Select(x => x.transform).ToList();
    private List<Animator> touristAnims => tourists.Select(x => x.GetComponent<Animator>()).ToList();
    
    private List<Animator> scaredTourists = new();
    
    private LevelMapController map;

    private List<Func<Transform, Vector3>> directionMethods = new();
    
    

    public void Initialise()
    {
        touristTweener = gameObject.AddComponent<Tweener>();
        map = LevelMapController.Instance;
        PPCollision.OnCollision += TouristScared;

        tourists[(int)TouristNo.TouristBlue] = TouristBlue.AddComponent<TouristBase>();
        tourists[(int)TouristNo.TouristPink] = TouristPink.AddComponent<TouristBase>();
        tourists[(int)TouristNo.TouristRed] = TouristRed.AddComponent<TouristBase>();
        tourists[(int)TouristNo.TouristYellow] = TouristYellow.AddComponent<TouristBase>();
        
        directionMethods.Add(RightCell);
        directionMethods.Add(LeftCell);
        directionMethods.Add(UpCell);
        directionMethods.Add(DownCell);
        
        PlaceTourists();
        
    }
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainSceneManager.CurrentGameState != MainGameState.GamePlaying) return;
        CurrentState = (scaredTourists.Count == 0) ? TouristState.TouristNormal : TouristState.TouristScared;
        
        foreach (var touristBase in tourists)
        {
            MoveTourist(touristBase);
        }
        
    }

    private void PlaceTourists()
    {
        var count = 0;
        foreach (var position in map.GhostStartPositions)
        {
            TouristNo touristType = (TouristNo)count;
            tourists[count].Initialise(touristType,position);
            touristTrans[count].position = position;
            count++;
        }
    }

    private void MoveTourist(TouristBase tourist)
    {
        if (touristTweener.TweenExists(tourist.transform)) return;

        tourist.LastPosition = tourist.Position;
        
        if (map.SpawnArea.Contains(tourist.Position))
        {
            if (map.TopSpawn.Contains(tourist.Position)) 
                MoveUp(tourist.transform);
            else
                MoveDown(tourist.transform);
        }
        
        
        var possibleMoves =
            directionMethods.Select(direction => direction(tourist.transform)).Where(x => !map.IsWall(x)).ToList();
        
        switch (tourist.TouristType)
        {
            case TouristNo.TouristBlue:
                BlueMove(tourist,possibleMoves);
                break;
            case TouristNo.TouristPink:
                //
                break;
            case TouristNo.TouristRed:
                //
                break;
            case TouristNo.TouristYellow:
                //
            break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void BlueMove(TouristBase tourist, List<Vector3> possibleMoves) //tries to be as far from pacstu as possible
    {
        if (possibleMoves.Count(x => x != tourist.LastPosition) > 0)
            possibleMoves.Remove(tourist.LastPosition);
        
        var finalMove = possibleMoves.Select(x =>
            new
            {
                dist = DistanceFromPacStu(x), 
                dir = x 
            })
            .OrderBy(x => x.dist)
            .First().dir;
        
        MoveCell(tourist.transform,finalMove);
    }

    private float DistanceFromPacStu(Vector3 pos)
    {
        return (MainSceneManager.MSManager.PlayableCharacter.position - pos).sqrMagnitude;
    }

    private TouristNo GetTouristType(Transform tourist)
    {
        int index = Array.IndexOf(tourists, tourist.gameObject);

        return (TouristNo)index;
    }

    
    private Vector3 UpCell(Transform tourist) => tourist.position + Vector3.up;
    private Vector3 DownCell(Transform tourist) => tourist.position + Vector3.down;
    private Vector3 LeftCell(Transform tourist) => tourist.position + Vector3.left;
    private Vector3 RightCell(Transform tourist) => tourist.position + Vector3.right;
    
    private void MoveCell(Transform tourist,Vector3 endPos) => touristTweener.AddTween(tourist, tourist.position,endPos,Duration);
    private void MoveUp(Transform tourist) => touristTweener.AddTween(tourist, tourist.position,tourist.position + Vector3.up,Duration);
    private void MoveDown(Transform tourist) => touristTweener.AddTween(tourist, tourist.position,tourist.position + Vector3.down,Duration);
    private void MoveLeft(Transform tourist) => touristTweener.AddTween(tourist, tourist.position,tourist.position + Vector3.left,Duration);
    private void MoveRight(Transform tourist) => touristTweener.AddTween(tourist, tourist.position,tourist.position + Vector3.right,Duration);
    private void TouristScared()
    {
        scaredTourists.Clear();
        foreach (Animator animator in touristAnims)
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
            if (GhostCounter == 3) touristAnims.ForEach(x => x.SetBool("Recovering",true));
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
        Animator touristToKill = touristAnims.FirstOrDefault(x => x.gameObject == tourist);
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
