using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LevelScripts;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum TouristState
{
    TouristScared,
    TouristNormal,
    TouristDead
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
    private float speed = 3.5f;
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
    
    private List<TouristBase> scaredTourists = new();
    
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
        
        if (touristTweener.TweenExists(tourist.transform) || tourist.CurrentState==UtilClass.Death) return;
        
        if (map.SpawnArea.Contains(tourist.Position))
        {
            MoveCell(tourist,
                map.TopSpawn.Contains(tourist.Position) ? UpCell(tourist.transform) : DownCell(tourist.transform));

            return;
        }
        
        
        var possibleMoves =
            directionMethods.Select(direction => direction(tourist.transform)).Where(x => !map.IsWall(x) && !map.IsTeleportArea(x) && !map.SpawnArea.Contains(x)).ToList();

        var filteredMoves = new List<Vector3>(possibleMoves);
        
        if (filteredMoves.Count(x => x != tourist.LastPosition) > 0)
        {
            //var contains = possibleMoves.Any(x => x == tourist.LastPosition);
            var success = filteredMoves.Remove(tourist.LastPosition);
        }

        switch (tourist.TouristType)
        {
            case TouristNo.TouristBlue:
                BlueMove(tourist,filteredMoves);
                break;
            case TouristNo.TouristPink:
                PinkMove(tourist, filteredMoves);
                break;
            case TouristNo.TouristRed:
                RedMove(tourist,filteredMoves);
                break;
            case TouristNo.TouristYellow:
                YellowMove(tourist,filteredMoves);
            break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void BlueMove(TouristBase tourist, List<Vector3> possibleMoves) //tries to be as far from pacstu as possible
    {
        var finalMove = possibleMoves.Select(x =>
            new
            {
                dist = DistanceFromPacStu(x), 
                dir = x 
            })
            .OrderByDescending(x => x.dist).First().dir;
        
        MoveCell(tourist,finalMove);
    }
    
    private void PinkMove(TouristBase tourist, List<Vector3> possibleMoves) //tries to be as close to pacstu as possible
    {
        var finalMove = possibleMoves.Select(x =>
                new
                {
                    dist = DistanceFromPacStu(x), 
                    dir = x 
                })
            .OrderBy(x => x.dist).First().dir;
        
        MoveCell(tourist,finalMove);
    }

    private void RedMove(TouristBase tourist, List<Vector3> possibleMoves)
    {
        var randomInd = Random.Range(0, possibleMoves.Count);
        
        MoveCell(tourist,possibleMoves[randomInd]);
    }

    private void YellowMove(TouristBase tourist, List<Vector3> possibleMoves)
    {
        Dictionary<Vector3,RaycastHit2D> directionDistance = new Dictionary<Vector3, RaycastHit2D>();

        int wallMask = (1 << LayerMask.NameToLayer("MazeBounds"));
        
        RaycastHit2D upHit = Physics2D.Raycast(tourist.Position, Vector2.up, Mathf.Infinity, wallMask);

        RaycastHit2D downHit = Physics2D.Raycast(tourist.Position, Vector2.down, Mathf.Infinity, wallMask);
        
        RaycastHit2D leftHit = Physics2D.Raycast(tourist.Position, Vector2.left, Mathf.Infinity, wallMask);

        RaycastHit2D rightHit = Physics2D.Raycast(tourist.Position, Vector2.right, Mathf.Infinity, wallMask);

        if(upHit) directionDistance.Add(Vector3.up, upHit);
        if(downHit) directionDistance.Add(Vector3.down, downHit);
        if(leftHit) directionDistance.Add(Vector3.left, leftHit);
        if(rightHit) directionDistance.Add(Vector3.right,rightHit);

        var closestWall = directionDistance.OrderBy(x => x.Value.distance).First();
        //Debug.DrawLine(tourist.Position,closestWall.Value.point);
        if (possibleMoves.Count > 1)
        {
            if (closestWall.Key == Vector3.up) possibleMoves.Remove(LeftCell(tourist.transform));
            if (closestWall.Key == Vector3.down) possibleMoves.Remove(RightCell(tourist.transform));
            if (closestWall.Key == Vector3.left) possibleMoves.Remove(DownCell(tourist.transform));
            if (closestWall.Key == Vector3.right) possibleMoves.Remove(UpCell(tourist.transform));
        }
        
        //Debug.Log($"{closestWall.Key}");
        
        var finalMove = possibleMoves.Select(x =>
                new
                {
                    dist = Vector3.Distance(closestWall.Value.point,x), 
                    dir = x 
                })
            .OrderBy(x => x.dist).First().dir;
        
        possibleMoves.ForEach(x=>Debug.DrawLine(closestWall.Value.point,x,Color.cyan));
        
        MoveCell(tourist,finalMove);

    }
    
    private float DistanceFromPacStu(Vector3 pos)
    {
        return (MainSceneManager.MSManager.PlayableCharacter.position - pos).sqrMagnitude;
    }

    
    private Vector3 UpCell(Transform tourist) => tourist.position + Vector3.up;
    private Vector3 DownCell(Transform tourist) => tourist.position + Vector3.down;
    private Vector3 LeftCell(Transform tourist) => tourist.position + Vector3.left;
    private Vector3 RightCell(Transform tourist) => tourist.position + Vector3.right;
    
    private void MoveCell(TouristBase tourist,Vector3 endPos)
    {
        tourist.LastPosition = tourist.Position;
        touristTweener.AddTween(tourist.transform, tourist.Position, endPos, Duration);
    }

    private void TouristScared()
    {
        scaredTourists.Clear();
        foreach (var tourist in tourists)
        {
            tourist.TouristAnimator.SetBool(UtilClass.Scared, true);
            
            scaredTourists.Add(tourist);
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
            if (GhostCounter == 3) touristAnims.ForEach(x => x.SetBool(UtilClass.Recovering,true));
        }
        
        OnGhostRecovered?.Invoke();
        scaredTourists.ForEach(x => x.TouristAnimator.SetBool(UtilClass.Recovering,false));
        scaredTourists.ForEach(x => x.TouristAnimator.SetBool(UtilClass.Scared,false));
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
        var touristToKill = tourists.FirstOrDefault(x => x.gameObject == tourist);
        if (touristToKill == null) return;
        var colliderToKill = tourist.GetComponent<Collider2D>();
        colliderToKill.enabled = false;
        scaredTourists.Remove(touristToKill);
        touristToKill.TouristAnimator.SetBool(UtilClass.Death,true);
        touristToKill.TouristAnimator.SetBool(UtilClass.Recovering,false);
        touristToKill.TouristAnimator.SetBool(UtilClass.Scared,false);
        BGM.instance.PlayGhostDead();
        touristTweener.CancelTween(touristToKill.transform);
        StartCoroutine(DeathCounter(touristToKill,colliderToKill));
    }

    private IEnumerator DeathCounter(TouristBase deadTourist, Collider2D touristCollider)
    {
        touristTweener.AddTween(deadTourist.transform, deadTourist.Position, deadTourist.SpawnPoint, 5.0f);
        yield return new WaitForSeconds(5);
        deadTourist.TouristAnimator.SetBool(UtilClass.Death,false);
        touristCollider.enabled = true;
    }

    public bool TouristScared(GameObject tourist)
    {
        if (scaredTourists.Count == 0) return false;
        
        return scaredTourists.Exists(x => x.gameObject == tourist);
    }
}
