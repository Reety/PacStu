using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    public static List<Vector3> Directions = new List<Vector3>()
    {
        Vector3.up, Vector3.down, Vector3.left, Vector3.right
    };
    
    public static Dictionary<KeyCode, int> KeyToAnimation = new Dictionary<KeyCode, int>()
    {
        [KeyCode.W] = Animator.StringToHash("Up"),
        [KeyCode.A] = Animator.StringToHash("Left"),
        [KeyCode.S] = Animator.StringToHash("Down"),
        [KeyCode.D] = Animator.StringToHash("Right"),
        [KeyCode.None] = Animator.StringToHash("Idle"),
    };

    public static int Up = Animator.StringToHash("Up");
    public static int Left = Animator.StringToHash("Left");
    public static int Down = Animator.StringToHash("Down");
    public static int Right = Animator.StringToHash("Right");
    
    public static int Idle = Animator.StringToHash("Idle");
    public static int Death = Animator.StringToHash("Death");
    public static int Respawn = Animator.StringToHash("Respawn");

    public static int Scared = Animator.StringToHash("Scared");
    public static int Recovering = Animator.StringToHash("Recovering");
        
    public static Matrix4x4
        Rotate0 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0)), 
        Rotate90 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90f)), 
        Rotate180 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180f)), 
        Rotate270 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270f));
    public static int[] GetRow(this int[,] srcArray, int row)
    {
        int size = 4;
        
        int columns = srcArray.GetLength(1);
        int[] newRow = new int[columns];

        Buffer.BlockCopy(srcArray,row*columns*size,newRow,0,columns*size);

        return newRow;
    }

    public static TimeSpan ParseStringToTimeSpan(string time)
    {
        var timeParts = time.Split(':');
        if (timeParts.Length is 0 or > 3) return TimeSpan.Zero;

        var minutes = int.Parse(timeParts[0]);
        var seconds = int.Parse(timeParts[1]);
        var milli = int.Parse(timeParts[2]);

        return new TimeSpan(0, 0, minutes, seconds, milli);
    }
}
