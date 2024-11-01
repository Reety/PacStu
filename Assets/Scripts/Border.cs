using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

public class Border : MonoBehaviour
{
    [SerializeField] private LineRenderer titleBorder;
    [SerializeField] private int width;

    [SerializeField] private RectTransform heading;
    // Start is called before the first frame update
    
    
    void Start()
    {
        titleBorder.startWidth = 0.1f;
        titleBorder.useWorldSpace = true;
        Vector3[] titleCorners = new Vector3[4];
        heading.GetWorldCorners(titleCorners);
        titleBorder.positionCount = 4;//600;

        Vector3 sign = Vector3.one;
        int i = 0;
        
        foreach (var corner in (titleCorners))
        {
            sign.x = (corner.x < heading.position.x) ? -sign.x : sign.x;
            sign.y = (corner.y < heading.position.y) ? -sign.y : sign.y;

            print($"({corner} is compared to {heading.position} so sign is {sign}");
            Vector3 newCorner = new Vector3(corner.x + sign.x, corner.y + sign.y);
            titleBorder.SetPosition(i,newCorner);
            Debug.Log(newCorner);
            sign = Vector3.one;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
