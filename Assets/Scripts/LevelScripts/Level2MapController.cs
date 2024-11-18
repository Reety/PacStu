using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LevelScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level2MapController : MonoBehaviour, IMapController
{
     public static Level2MapController Instance;
     private Tilemap walls;
     private Tilemap interactables;
     private Grid grid;
     private TileBase sPellet;
     private WallTiles wallTiles;
    
     private Bounds spawnArea;
     public Bounds SpawnArea => spawnArea;
     private Bounds topSpawn;
     public Bounds TopSpawn => topSpawn;
     public Bounds BottomSpawn; 
     
     public Vector3[] MazeExits { get; } = new Vector3[2];
    
     private Vector3[][] vectorMap;
     public Vector3 PlayerStartPos => GetCentre(vectorMap[1][1]);

     public Vector3[] GhostStartPositions {
         get
         {
             int row = vectorMap.Length/2 - 1;
             int col = vectorMap[row].Length/2 - 1;
             return new[]
             {
                 GetCentre(vectorMap[row][col]),
                 GetCentre(vectorMap[row][col + 1]),
                 GetCentre(vectorMap[row+2][col]),
                 GetCentre(vectorMap[row+2][col + 1])
             };
         }

     }
    

     private List<Vector3> teleportArea = new();


     // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        spawnArea = new Bounds(new Vector3(1,0.5f), new Vector3(6,5));
        topSpawn = new Bounds(new Vector3(1,1.75f), new Vector3(6, 2.5f));
        BottomSpawn = new Bounds(new Vector3(1,-0.75f), new Vector3(6, 2.5f));
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Initialise(Tilemap walls, Tilemap interactables, Vector3[][] vectorMap, List<Vector3> teleportPoints, TileBase sPel)
    {
        this.walls = walls;
        wallTiles = walls.GetComponent<WallTiles>();
        this.interactables = interactables;
        this.vectorMap = vectorMap;
        sPellet = sPel;

        grid = this.walls.layoutGrid;
        teleportArea = teleportPoints.Select(GetCentre).ToList();
        
        MazeExits[0] = teleportPoints.Where(t => !IsWall(t)).
            OrderBy(t => t.x).
            Where((t,index) => index == teleportPoints.Count/2-1).First();

        MazeExits[1] = teleportPoints.Where(t => !IsWall(t)).
            OrderBy(t => t.x)
            .Where((t, index) => index == teleportPoints.Count / 2).First();
        
        CreateOuterWallColliders();
        interactables.ClearAllTiles();

    }

    private void CreateOuterWallColliders()
    {
        GameObject outerWall = new GameObject();
        outerWall.AddComponent<EdgeCollider2D>();
        outerWall.layer = LayerMask.NameToLayer("MazeBounds");
            
        var topWall = vectorMap[0];
        var bottomWall = vectorMap[^1];
        var topWallCollider = Instantiate(outerWall, Vector3.zero, Quaternion.identity).GetComponent<EdgeCollider2D>();
        var bottomWallCollider = Instantiate(outerWall, Vector3.zero, Quaternion.identity).GetComponent<EdgeCollider2D>();
        var leftWallCollider = Instantiate(outerWall, Vector3.zero, Quaternion.identity).GetComponent<EdgeCollider2D>();
        var rightWallCollider = Instantiate(outerWall, Vector3.zero, Quaternion.identity).GetComponent<EdgeCollider2D>();
            
        topWallCollider.points = new Vector2[] { GetCentre(topWall[0]), GetCentre(topWall[^1]) };
        bottomWallCollider.points = new Vector2[] { GetCentre(bottomWall[0]), GetCentre(bottomWall[^1]) };
        leftWallCollider.points = new Vector2[] { GetCentre(topWall[0]), GetCentre(bottomWall[0]) };
        rightWallCollider.points = new Vector2[] { GetCentre(topWall[^1]), GetCentre(bottomWall[^1]) };
    }
    public Vector3 GetCentre(Vector3 point) => grid.GetCellCenterWorld(Vector3Int.FloorToInt(point));
    
    public bool IsWall(Vector3 point) => walls.HasTile(Vector3Int.FloorToInt(point));

    public bool IsPell(Vector3 point) => interactables.HasTile((Vector3Int.FloorToInt(point)));

    public bool IsTeleportArea(Vector3 point) => teleportArea.Contains(point);
}
