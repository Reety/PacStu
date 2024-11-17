using System;
using System.Collections;
using System.Collections.Generic;
using LevelScripts;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private Vector3 oppositePosition;
    // Start is called before the first frame update

    public void Initialise(Vector3 opp)
    {
        oppositePosition = opp;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
        other.transform.position = oppositePosition;

        if (other.CompareTag("Enemy"))
        {
            //print("enemy out");
            other.gameObject.SetActive(false);
        }
    }
}
