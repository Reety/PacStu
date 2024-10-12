using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    
    public Tilemap wallMap;
    public WallTiles wall;
    public Tilemap pelMap;

    public TileBase ppellet;
    public TileBase sPellet;
    
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

    private const int Empty = 0, OCorner = 1, OWall = 2, ICorner = 3, IWall = 4, Spel = 5, PPel = 6, TJunc = 7;
    
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
    
    private Vector3 startPos = new Vector3(-(levelMap.GetLength(1)-1),levelMap.GetLength(0)-1);

    private static int[][] fullMap = new int[levelMap.GetLength(0) * 2 - 1][];

    // Start is called before the first frame update
    void Start()
    {
        /*wallMap.ClearAllTiles();
        pelMap.ClearAllTiles();*/
        
        wall = wallMap.GetComponent<WallTiles>();

        for (int i = 0; i < fullMap.Length; i++)
        {
            fullMap[i] = new int[levelMap.GetLength(1) * 2];
        }
        
        addTopLeft();
        addTopRight();
        addBottomHalf();

        /*string str = "";
        
        for (int n = 0; n < fullMap.Length; n++) {

            // Print the row number
            str += $"Row({n}): ";

            for (int k = 0; k < fullMap[n].Length; k++) {

                // Print the elements in the row
                str += $"{fullMap[n][k]} ";

            }

            str += "\n";
        }
        
        print(str);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addTopLeft()
    {
        for (int i = 0; i <= levelMap.GetUpperBound(0); i++)
        {
            levelMap.GetRow(i).CopyTo(fullMap[i],0);
        }
    }

    private void addTopRight()
    {
        for (int i = 0; i <= levelMap.GetUpperBound(0); i++)
        {
            levelMap.GetRow(i).Reverse().ToArray().CopyTo(fullMap[i],levelMap.GetLength(1));
        }
    }

    private void addBottomHalf()
    {
        int[][] quadrant = fullMap.Where(
            (row, index) => index < levelMap.GetUpperBound(0)).Select(
            x => x).Reverse().ToArray();

        quadrant.CopyTo(fullMap, levelMap.GetLength(0));
    }

    private void placeTiles()
    {
        int row = 0;
        int col = 0;
        
        Vector3 currPos = startPos;
        
        for (row = 0; row < fullMap.Length; row++)
        {
            for (col = 0; col < fullMap[row].Length; col++)
            {
                var tileCode = fullMap[row][col];
                if (tileCode is OCorner or ICorner)
                {
                    
                }
                /*outside corner pieces
                 * D-R -> 0
                 * D-L -> 90
                 * U-R -> 270
                 * U-L -> 180
                 * */
                currPos += Vector3.right;
            }
            currPos = new Vector3(startPos.x, currPos.y - 1);
        }
    }

    private void placeCorner(int tileCode, int row, int col, Vector3 position)
    {
        Vector3Int pos = Vector3Int.FloorToInt(position);
        Matrix4x4 rotation;
        
        if (row == 0 && col == 0) 
            wallMap.SetTile(pos,wall.OutsideCorner);
        else
        {
            switch (tileCode)
            {
                case ICorner:
                    if (fullMap[row - 1][col] is ICorner or IWall or TJunc)
                        rotation = fullMap[row][col + 1] is ICorner or IWall ? UtilClass.Rotate270 : UtilClass.Rotate180;
                    else 
                        rotation = fullMap[row][col + 1] is ICorner or IWall ? UtilClass.Rotate0 : UtilClass.Rotate180;
                    
                    break;
                
                case OCorner:
                    break;
            }
        }
    }

    private Matrix4x4 findCornerRotation(int row, int col)
    {
        
        return UtilClass.Rotate0;
    }
    
    
}
