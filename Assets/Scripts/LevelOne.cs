using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelOne : MonoBehaviour
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
    
    private static int[,] levelMap = 
    { 
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4}, 
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4}, 
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3}, 
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5}, 
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4}, 
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3}, 
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4}, 
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4}, 
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3}, 
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0}, 
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0}, 
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0}, 
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}, 
    };
    
    
    //private static Vector3[,] cord = new Vector3[levelMap.GetLength(0),levelMap.GetLength(1)];

    public RuleTile OutsideWall;
    public RuleTile InsideWall;

    public TileBase TJunction;

    public Tilemap Walls;

    private Vector3 startPos = new Vector3(-(levelMap.GetLength(1)-1),levelMap.GetLength(0)-1);
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 currPos = startPos;
        string str = "";
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                if (levelMap[i, j] == 1 || levelMap[i, j] == 2)
                    Walls.SetTile(Vector3Int.FloorToInt(currPos), OutsideWall);
                
                if (levelMap[i, j] == 3 || levelMap[i, j] == 4)
                    Walls.SetTile(Vector3Int.FloorToInt(currPos), InsideWall);
                if (levelMap[i, j] == 7) 
                    Walls.SetTile(Vector3Int.FloorToInt(currPos), TJunction);
                
                //str += $"{currPos} ";
                currPos += Vector3.right;
            }
            //Debug.Log(str);
            currPos = new Vector3(startPos.x, currPos.y - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
