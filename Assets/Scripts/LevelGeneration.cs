using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    
    public Tilemap wallMap;
    public WallTiles wall;
    public Tilemap pelMap;

    public TileBase ppellet;
    public TileBase sPellet;
    
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

        string str = "";
        
        for (int n = 0; n < fullMap.Length; n++) {

            // Print the row number
            str += $"Row({n}): ";

            for (int k = 0; k < fullMap[n].Length; k++) {

                // Print the elements in the row
                str += $"{fullMap[n][k]} ";

            }

            str += "\n";
        }
        
        print(str);
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
}
