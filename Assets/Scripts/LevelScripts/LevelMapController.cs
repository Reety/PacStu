using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelScripts
{
    public class LevelMapController : MonoBehaviour
    {
        /*
     * 0 - empty
     * 1 - outside corner
     * 2 - outside wall
     * 3 - inside corner
     * 4 - inside wall
     * 5 - standard pellet
     * 6 - power pellet
     * 7 - t-junction
     */
    
    
        //private static Vector3[,] cord = new Vector3[levelMap.GetLength(0),levelMap.GetLength(1)];
        public static LevelMapController Instance;
        private Tilemap walls;
        private Tilemap interactables;
        private Grid grid;
        private TileBase sPellet;
        private WallTiles wallTiles;

        public Bounds SpawnArea;
        public Bounds TopSpawn; 
        public Bounds BottomSpawn; 

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
            SpawnArea = new Bounds(new Vector3(1,0.5f), new Vector3(6,5));
            TopSpawn = new Bounds(new Vector3(1,1.75f), new Vector3(6, 2.5f));
            BottomSpawn = new Bounds(new Vector3(1,-0.75f), new Vector3(6, 2.5f));
        }

        // Update is called once per frame
        void Update()
        {
            if (!interactables.ContainsTile(sPellet)) MainSceneManager.MSManager.GameOver();
        }

        public void Initialize(Tilemap walls, Tilemap interactables, Vector3[][] vectorMap, List<Vector3> teleportPoints, TileBase sPel)
        {
            this.walls = walls;
            wallTiles = walls.GetComponent<WallTiles>();
            this.interactables = interactables;
            this.vectorMap = vectorMap;
            sPellet = sPel;

            grid = this.walls.layoutGrid;
            teleportArea = teleportPoints.Select(GetCentre).ToList();
            
            CreateOuterWallColliders();
            

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

        /*public bool IsOutsideWall(Vector3 point) => walls.GetTile(Vector3Int.FloorToInt(point)) == wallTiles.OutsideWall 
                                                    || walls.GetTile(Vector3Int.FloorToInt(point)) == wallTiles.TJunction
                                                    || IsOutsideCorner(point)
                                                    || IsTeleportArea(point);*/
        public bool IsOutsideCorner(Vector3 point) => (walls.GetTile(Vector3Int.FloorToInt(point)) == wallTiles.OutsideCorner);

    }
}
