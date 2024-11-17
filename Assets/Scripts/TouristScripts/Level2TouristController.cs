using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LevelScripts;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum TouristMood
{
    Normal,
    Alert,
    Scared
}
public class Level2TouristController : MonoBehaviour
{
    private float speed = 4.5f;
    private float regularSpeed = 4.5f;
    private float slowSpeed = 1.5f;
    private float alertSpeed = 5.0f;
    private float scaredSpeed = 8.5f;
    
    private bool hypnotised = false;
    
    private TouristMood currentMood = TouristMood.Normal;
    private float Duration => 1 / speed;
    public static Level2TouristController Instance;
    private Tweener touristTweener;
    
    public GameObject TouristYellow;
    public GameObject TouristPink;
    public GameObject TouristBlue;
    public GameObject TouristRed;
    

    private TouristBase[] tourists = new TouristBase[4];

    public static event Action OnGhostScared;
    
    public TouristState CurrentState = TouristState.TouristNormal;
    
    public float counter = 0;
    public static int GhostCounter = 10;
    public BGMState BGMContext = BGMState.NormalBGM;
    private List<Animator> touristAnims => tourists.Select(x => x.GetComponent<Animator>()).ToList();
    private Dictionary<TouristNo,Light2D> touristLight { get; } = new Dictionary<TouristNo,Light2D>();
    private Dictionary<TouristNo,ParticleSystem> touristAlert { get; } = new Dictionary<TouristNo,ParticleSystem>();

    private IMapController map;
    
    private List<TouristBase> aliveTourists = new List<TouristBase>();

    private List<Func<Transform, Vector3>> directionMethods = new();

    private bool AnyTouristDead => tourists.Any(x => x.CurrentState == UtilClass.Death);

    private bool AllTouristDead => tourists.All(x => x.CurrentState == UtilClass.Death);
    
    private bool TouristEscaped => tourists.Any(x=>!x.isActiveAndEnabled);

    private Dictionary<int, Vector3> MovementToLightDirection = new Dictionary<int, Vector3>()
    {
        [UtilClass.Down] = Vector3.zero,
        [UtilClass.Left] = new Vector3(0, 0, -90),
        [UtilClass.Right] = new Vector3(0, 0, 90),
        [UtilClass.Up] = new Vector3(0, 0, 180),
    };
    

    public void Initialise()
    {
        touristTweener = gameObject.AddComponent<Tweener>();
        map = Level2MapController.Instance;
        FlashLightCollision.OnCollision += TouristScared;

        tourists[(int)TouristNo.TouristBlue] = TouristBlue.AddComponent<TouristBase>();
        tourists[(int)TouristNo.TouristPink] = TouristPink.AddComponent<TouristBase>();
        tourists[(int)TouristNo.TouristRed] = TouristRed.AddComponent<TouristBase>();
        tourists[(int)TouristNo.TouristYellow] = TouristYellow.AddComponent<TouristBase>();
        
        touristLight.Add(TouristNo.TouristBlue, TouristBlue.GetComponentInChildren<Light2D>());
        touristLight.Add(TouristNo.TouristPink, TouristPink.GetComponentInChildren<Light2D>());
        touristLight.Add(TouristNo.TouristRed, TouristRed.GetComponentInChildren<Light2D>());
        touristLight.Add(TouristNo.TouristYellow, TouristYellow.GetComponentInChildren<Light2D>());
        
        touristAlert.Add(TouristNo.TouristBlue, TouristBlue.GetComponentInChildren<ParticleSystem>());
        touristAlert.Add(TouristNo.TouristPink, TouristPink.GetComponentInChildren<ParticleSystem>());
        touristAlert.Add(TouristNo.TouristRed, TouristRed.GetComponentInChildren<ParticleSystem>());
        touristAlert.Add(TouristNo.TouristYellow, TouristYellow.GetComponentInChildren<ParticleSystem>());
        
        directionMethods.Add(RightCell);
        directionMethods.Add(LeftCell);
        directionMethods.Add(UpCell);
        directionMethods.Add(DownCell);
        
        aliveTourists = tourists.ToList();
        
        PlaceTourists();
        
    }
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Level2Manager.L2Manager.CurrentGameState != MainGameState.GamePlaying) return;

        if (TouristEscaped || AllTouristDead)
        {
            Level2Manager.L2Manager.GameOver();
        }
        
        if (CurrentState == TouristState.TouristScared)
            currentMood = TouristMood.Scared;
        
        SetSpeed();
        
        foreach (var touristBase in tourists)
        {
            MoveTourist(touristBase);
        }
        
    }
    
    
    private void SetSpeed()
    {
        if (hypnotised)
            speed = slowSpeed;
        else
        {
            speed = currentMood switch
            {
                TouristMood.Normal => regularSpeed,
                TouristMood.Alert => alertSpeed,
                TouristMood.Scared => scaredSpeed,
                _ => speed
            };
        }
    }

    private void PlaceTourists()
    {
        var count = 0;
        foreach (var position in map.GhostStartPositions)
        {
            TouristNo touristType = (TouristNo)count;
            tourists[count].Initialise(touristType,position);
            tourists[count].transform.position = position;
            tourists[count].SpawnPoint = position;
            count++;
        }
    }

    private void MoveTourist(TouristBase tourist)
    {
        
        if (touristTweener.TweenExists(tourist.transform) || tourist.CurrentState == UtilClass.Death) return;
        
        
        if (map.SpawnArea.Contains(tourist.Position))
        {
            MoveCell(tourist,
                map.TopSpawn.Contains(tourist.Position) ? UpCell(tourist.transform) : DownCell(tourist.transform));

            return;
        }
        
        if (CurrentState == TouristState.TouristScared)
        {
            ScaredMove(tourist);
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
        
        if (currentMood == TouristMood.Alert)
        {
            HuntMove(tourist,filteredMoves);
            return;
        } 
        RandomMove(tourist,filteredMoves);
        
        
    }

    private void ScaredMove(TouristBase tourist) //runs to exit
    {
        var possibleMoves =
            directionMethods.Select(direction => direction(tourist.transform)).Where(x => !map.IsWall(x) && !map.SpawnArea.Contains(x)).ToList();

        var filteredMoves = new List<Vector3>(possibleMoves);
        
        if (filteredMoves.Count(x => x != tourist.LastPosition) > 0)
        {
            //var contains = possibleMoves.Any(x => x == tourist.LastPosition);
            var success = filteredMoves.Remove(tourist.LastPosition);
        }

        var finalMove = filteredMoves.Select(x =>
                new
                {
                    dist = Vector3.Distance(x, ClosestExit(tourist.transform)),
                    dir = x
                })
            .OrderBy(x => x.dist).Select(x => x.dir).First();
        
        MoveCell(tourist,finalMove);
    }

    private Vector3 ClosestExit(Transform tourist)
    {
        var closestExit = Level2MapController.Instance.MazeExits.OrderBy(x=>Vector3.Distance(x,tourist.position)).First();
        return closestExit;

    }


    private void RandomMove(TouristBase tourist, List<Vector3> possibleMoves)
    {
        var randomInd = Random.Range(0, possibleMoves.Count);
        
        MoveCell(tourist,possibleMoves[randomInd]);
    }


    
    private float DistanceFromPacStu(Vector3 pos)
    {
        return (Level2Manager.L2Manager.PlayableCharacter.position - pos).sqrMagnitude;
    }

    
    private Vector3 UpCell(Transform tourist) => tourist.position + Vector3.up;
    private Vector3 DownCell(Transform tourist) => tourist.position + Vector3.down;
    private Vector3 LeftCell(Transform tourist) => tourist.position + Vector3.left;
    private Vector3 RightCell(Transform tourist) => tourist.position + Vector3.right;
    
    private void MoveCell(TouristBase tourist,Vector3 endPos)
    {
        tourist.LastPosition = tourist.Position;
        int triggerKey = UtilClass.DirectionToAnimation[Vector3.Normalize(endPos-tourist.Position)];
        if (CurrentState == TouristState.TouristNormal)
        {
            if (tourist.LastTrigger != triggerKey)
            {
                tourist.TouristAnimator.SetTrigger(triggerKey);
                tourist.LastTrigger = triggerKey;
            }
        }
        
        touristLight[tourist.TouristType].transform.rotation = Quaternion.Euler(MovementToLightDirection[triggerKey]);
        touristTweener.AddTween(tourist.transform, tourist.Position, endPos, Duration);
    }

    private void TouristScared()
    {
        //scaredTourists.Clear();
        foreach (var tourist in tourists)
        {
            tourist.TouristAnimator.SetBool(UtilClass.Scared, true);
            //touristTweener.CancelTween(tourist.transform);
            //scaredTourists.Add(tourist);
        }

        CurrentState = TouristState.TouristScared;
        OnGhostScared?.Invoke();
    }
    
    private void HuntMove(TouristBase tourist, List<Vector3> possibleMoves) //tries to be as close to pacstu as possible
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
    
    private void OnDestroy()
    {
        FlashLightCollision.OnCollision -= TouristScared;
    }

    public void SlowDownTourists()
    {
        if (hypnotised) return;
        hypnotised = true;
        StartCoroutine(TouristSlow());
    }

    IEnumerator TouristSlow()
    {
        yield return new WaitForSeconds(10.0f);
        hypnotised = false;
    }
    

    public void KillTourist(GameObject tourist)
    {
        var touristToKill = tourists.FirstOrDefault(x => x.gameObject == tourist);
        if (touristToKill == null) return;
        var colliderToKill = tourist.GetComponent<Collider2D>();
        colliderToKill.enabled = false;
        touristToKill.TouristAnimator.SetBool(UtilClass.Death,true);
        aliveTourists.Remove(touristToKill);
        touristLight[touristToKill.TouristType].enabled = false;
        touristTweener.CancelTween(touristToKill.transform);
        //touristTweener.AddTween(tourist.transform, touristToKill.Position, touristToKill.SpawnPoint, Duration);

    }

    public IEnumerator SetAlert()
    {
        if (CurrentState == TouristState.TouristScared) yield break;
        aliveTourists.ForEach(x=>touristAlert[x.TouristType].Play());
        currentMood = TouristMood.Alert;
        yield return new WaitForSeconds(5.0f);
        currentMood = TouristMood.Normal;
    }
    

}
