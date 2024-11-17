using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelScripts
{
    public interface IMapController 
    {
        Bounds SpawnArea { get; }
        Bounds TopSpawn { get; }
    
        Vector3 PlayerStartPos { get; }
        Vector3[] GhostStartPositions { get; }

        public void Initialise(Tilemap walls, Tilemap interactables, Vector3[][] vectorMap, List<Vector3> teleportPoints, TileBase sPel);

        public Vector3 GetCentre( Vector3 point);
        public bool IsWall(Vector3 point);

        public bool IsPell(Vector3 point);
        public bool IsTeleportArea(Vector3 point);
    }
}
