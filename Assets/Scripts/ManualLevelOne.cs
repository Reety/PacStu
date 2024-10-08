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
    void Start()
    {
        //mirror along y-axis
        Tilemap yMap = Object.Instantiate(walls, walls.transform.position + (Vector3.right * 2),
            Quaternion.AngleAxis(180, Vector3.up),transform);
        
        Tilemap pelYMap = Object.Instantiate(interactables, interactables.transform.position + (Vector3.right * 2),
            Quaternion.AngleAxis(180, Vector3.up),transform);
        
        //mirror along x-axis
        Object.Instantiate(walls, walls.transform.position + Vector3.up,
            Quaternion.AngleAxis(180, Vector3.right),transform);
        
        Tilemap pelXMap = Object.Instantiate(interactables, interactables.transform.position + Vector3.up,
            Quaternion.AngleAxis(180, Vector3.right),transform);
        pelXMap.orientation = Tilemap.Orientation.Custom;
        pelXMap.orientationMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(180,Vector3.right));
        
        //mirror diag
        Object.Instantiate(yMap, yMap.transform.position + Vector3.up,
            yMap.transform.rotation * Quaternion.AngleAxis(180, Vector3.right),transform);
        
        Tilemap pelDiagMap = Object.Instantiate(pelYMap, pelYMap.transform.position + Vector3.up,
            yMap.transform.rotation * Quaternion.AngleAxis(180, Vector3.right),transform);
        
        pelDiagMap.orientation = Tilemap.Orientation.Custom;
        pelDiagMap.orientationMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis(180,Vector3.right));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
