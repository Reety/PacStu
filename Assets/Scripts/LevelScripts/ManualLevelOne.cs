using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ManualLevelOne : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap walls;
    public Tilemap interactables;
    void Awake()
    {
        //mirror along y-axis
        Tilemap yMap = Object.Instantiate(walls, walls.transform.position + Vector3.right,
            Quaternion.AngleAxis(180, Vector3.up),walls.transform);
        
        Tilemap pelYMap = Object.Instantiate(interactables, interactables.transform.position + Vector3.right,
            Quaternion.AngleAxis(180, Vector3.up),interactables.transform);
        
        //mirror along x-axis
        Object.Instantiate(walls, walls.transform.position,
            Quaternion.AngleAxis(180, Vector3.right),walls.transform);
        
        Tilemap pelXMap = Object.Instantiate(interactables, interactables.transform.position,
            Quaternion.AngleAxis(180, Vector3.right),interactables.transform);
        pelXMap.orientation = Tilemap.Orientation.Custom;
        pelXMap.orientationMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(180,Vector3.right));
        
        //mirror diag
        Object.Instantiate(yMap, yMap.transform.position,
            yMap.transform.rotation * Quaternion.AngleAxis(180, Vector3.right),walls.transform);
        
        Tilemap pelDiagMap = Object.Instantiate(pelYMap, pelYMap.transform.position,
            yMap.transform.rotation * Quaternion.AngleAxis(180, Vector3.right),interactables.transform);
        
        pelDiagMap.orientation = Tilemap.Orientation.Custom;
        pelDiagMap.orientationMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(180,Vector3.right));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
