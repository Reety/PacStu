using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelScripts
{
    public class LevelMap : MonoBehaviour
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

        private Tilemap walls;
        private Tilemap interactables;
        private Grid grid;

        private Vector3[][] vectorMap;
    
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Initialize(Tilemap walls, Tilemap interactables, Vector3[][] vectorMap)
        {
            this.walls = walls;
            this.interactables = interactables;
            this.vectorMap = vectorMap;

            grid = this.walls.layoutGrid;
        }

        public Vector3 GetCentre(Vector3 point) => grid.GetCellCenterWorld(Vector3Int.FloorToInt(point));
    
        public bool IsWall(Vector3 point) => walls.HasTile(Vector3Int.FloorToInt(point));
        
    
    }
}
