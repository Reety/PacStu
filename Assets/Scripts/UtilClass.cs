using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    public static int[] GetRow(this int[,] srcArray, int row)
    {
        int size = 4;
        
        int columns = srcArray.GetLength(1);
        int[] newRow = new int[columns];

        Buffer.BlockCopy(srcArray,row*columns*size,newRow,0,columns*size);

        return newRow;
    }

    public static void InsertRow(this int[,] fullArray, int[] row, int rowIndex)
    {
        
    }
}
