using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ManualLevelOne : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap walls;
    
    void Start()
    {
        //mirror along y-axis
        Tilemap yMap = Object.Instantiate(walls, walls.transform.position + (Vector3.right * 2),
            Quaternion.AngleAxis(180, Vector3.up),transform);
        //mirror along x-axis
        Object.Instantiate(walls, walls.transform.position,
            Quaternion.AngleAxis(180, Vector3.right),transform);
        //mirror diag
        Object.Instantiate(yMap, yMap.transform.position,
            yMap.transform.rotation * Quaternion.AngleAxis(180, Vector3.right),transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
