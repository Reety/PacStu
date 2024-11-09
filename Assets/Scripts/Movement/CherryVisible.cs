using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CherryVisible : MonoBehaviour
{

    private float speed;
    private bool passedCenter = false;
    private Camera mainCam;
    private bool destructionProcess = false;

    private Vector3 direction;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    public void Initialise(float speed)
    {
        this.speed = speed;
        direction =  UnityEngine.Vector3.ClampMagnitude(Vector3.zero - transform.position,1.0f);
        mainCam = Camera.main;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CherryMovement();
        
        if ((transform.position - Vector3.zero).magnitude < 0.5) passedCenter = true;
        
        if (passedCenter && !destructionProcess)
            StartCoroutine(nameof(InCameraView));
    }

    private void CherryMovement()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }
    
    IEnumerator InCameraView()
    {
        Vector3 viewPos = mainCam.WorldToViewportPoint(transform.position);
        
        if (!(viewPos.x < 0) && !(viewPos.x > 1) && !(viewPos.y < 0) && !(viewPos.y > 1)) yield break;
        
        destructionProcess = true;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);

    }
}
