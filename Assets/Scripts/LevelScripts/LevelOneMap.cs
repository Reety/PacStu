using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class LevelOneMap 
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
    
    private static Vector3 startPos = new Vector3(-(levelMap.GetLength(1)-1),levelMap.GetLength(0)-1);

    private static Vector3[,] _mapPositions = new Vector3[levelMap.GetLength(0), levelMap.GetLength(1)];
    public static Vector3[,] MapPositions
    {
        get => _mapPositions;
    }

    public static Vector3 TopLeftStart => _mapPositions[1, 1];

    static LevelOneMap()
    {
        Vector3 currPos = startPos;
        
        for (int i = 0; i < _mapPositions.GetLength(0); i++)
        {
            for (int j = 0; j < _mapPositions.GetLength(1); j++)
            {
                _mapPositions[i, j] = currPos;
                //Debug.Log($"Set [{i}][{j}] as: {currPos}");
                currPos += Vector3.right;
            }
            currPos = new Vector3(startPos.x, currPos.y - 1);
        }
    }
}
