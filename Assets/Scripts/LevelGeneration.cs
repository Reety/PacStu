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
        
        AddTopLeft();
        AddTopRight();
        AddBottomHalf();
        
        PlaceTiles();

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

    private void AddTopLeft()
    {
        for (int i = 0; i <= levelMap.GetUpperBound(0); i++)
        {
            levelMap.GetRow(i).CopyTo(fullMap[i],0);
        }
    }

    private void AddTopRight()
    {
        for (int i = 0; i <= levelMap.GetUpperBound(0); i++)
        {
            levelMap.GetRow(i).Reverse().ToArray().CopyTo(fullMap[i],levelMap.GetLength(1));
        }
    }

    private void AddBottomHalf()
    {
        int[][] quadrant = fullMap.Where(
            (row, index) => index < levelMap.GetUpperBound(0)).Select(
            x => x).Reverse().ToArray();

        quadrant.CopyTo(fullMap, levelMap.GetLength(0));
    }
    private void PlaceTiles()
    {
        int row = 0;
        int col = 0;


        startPos = new Vector3(50, 50);
        Vector3 currPos = startPos;
        
        for (row = 0; row < fullMap.Length; row++)
        {
            for (col = 0; col < fullMap[row].Length; col++)
            {
                var tileCode = fullMap[row][col];
                if (row == 0 && col == 0) wallMap.SetTile(Vector3Int.FloorToInt(currPos),wall.OutsideCorner);
                else switch (tileCode)
                {
                    case OCorner or ICorner:
                        PlaceCorner(tileCode,row,col,currPos);
                        break;
                    case OWall or IWall:
                        PlaceWall(tileCode,row,col,currPos);
                        break;
                    case TJunc:
                        break;
                }

                currPos += Vector3.right;
            }
            currPos = new Vector3(startPos.x, currPos.y - 1);
        }
    }

    /*corner pieces rotation
     * D-R -> 0
     * D-L -> 270
     * U-R -> 90
     * U-L -> 180
     * */
    private void PlaceCorner(int tileCode, int row, int col, Vector3 position)
    {
        Vector3Int pos = Vector3Int.FloorToInt(position);
        Matrix4x4 rotation;

        wallMap.SetTile(pos, tileCode is OCorner ? wall.OutsideCorner : wall.InsideCorner);

        if (IsAboveTileAWall(row,col) && !IsBelowTileAWall(row,col))
            rotation = (IsLeftTileAWall(row, col)) ? UtilClass.Rotate180 : UtilClass.Rotate90;
        else if (IsAboveTileAWall(row, col) && IsBelowTileAWall(row, col))
        {
            if (!IsTopRightAWall(row, col))
                rotation = UtilClass.Rotate90;
            else if (!IsTopLeftAWall(row, col))
                rotation = UtilClass.Rotate180;
            else if (!IsBottomLeftAWall(row, col))
                rotation = UtilClass.Rotate270;
            else rotation = UtilClass.Rotate0;
        }
        else 
            rotation = (IsLeftTileAWall(row, col)) ? UtilClass.Rotate270 : UtilClass.Rotate0;
        
        wallMap.SetTransformMatrix(pos, rotation);
        print($"{pos} at array {row}{col} has transform {wallMap.GetTransformMatrix(pos).rotation.eulerAngles}");
    }

    private void PlaceWall(int tileCode, int row, int col, Vector3 position)
    {
        Vector3Int pos = Vector3Int.FloorToInt(position);
        Matrix4x4 rotation = UtilClass.Rotate90;
        
        wallMap.SetTile(pos, tileCode is OWall ? wall.OutsideWall : wall.InsideWall);

        if (IsAboveTileAWall(row,col)) 
            wallMap.SetTransformMatrix(pos, rotation);
    }

    private static bool IsAboveTileAWall(int row, int col)
    {
        if (row - 1 < 0) return false; //checks it's not on the top row to avoid out of bounds exception
        else if (fullMap[row - 1][col] is ICorner or IWall or OCorner or OWall or TJunc) return true;

        return false;
    }

    private static bool IsTopRightAWall(int row, int col)
    {
        if (row - 1 < 0 || col + 1 > fullMap[row].Length-1) return false;
        else if (fullMap[row - 1][col + 1] is ICorner or IWall or OCorner or OWall) return true;
        return false;
    }
    
    private static bool IsTopLeftAWall(int row, int col)
    {
        if (row - 1 < 0 || col - 1 < 0) return false;
        else if (fullMap[row - 1][col - 1] is ICorner or IWall or OCorner or OWall) return true;
        return false;
    }
    
    private static bool IsBottomLeftAWall(int row, int col)
    {
        if (row + 1 > fullMap.Length-1 || col - 1 < 0) return false;
        else if (fullMap[row + 1][col - 1] is ICorner or IWall or OCorner or OWall) return true;
        return false;
    }
    
    private static bool IsBottomRightAWall(int row, int col)
    {
        if (row + 1 > fullMap.Length-1 || col + 1 > fullMap[row].Length-1) return false;
        else if (fullMap[row + 1][col + 1] is ICorner or IWall or OCorner or OWall) return true;
        return false;
    }


    private static bool IsBelowTileAWall(int row, int col)
    {
        if (row + 1 >= fullMap.Length-1) return false; //checks it's not on the bottom row to avoid out of bounds exception
        else if (fullMap[row + 1][col] is ICorner or IWall or OCorner or OWall or TJunc) return true;

        return false;
    }

    private static bool IsLeftTileAWall(int row, int col)
    {
        if (col - 1 < 0) return false;
        else if (fullMap[row][col - 1] is ICorner or IWall or OCorner or OWall or TJunc) return true;

        return false;
    }
    
    
}
