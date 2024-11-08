using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;
using Vector3 = UnityEngine.Vector3;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;
    // Start is called before the first frame update
    float vertSize => Camera.main.orthographicSize;
    float horSize => vertSize * Camera.main.aspect;

    private Func<Vector3>[] positionFunctions = new Func<Vector3>[2];

    private float spawnTime = 10f;
    private float counter = 0;
    void Awake()
    {
        positionFunctions[0] = RandomVertPoint;
        positionFunctions[1] = RandomHorzPoint;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= spawnTime)
        {
            Vector3 position = positionFunctions[UnityEngine.Random.Range(0, positionFunctions.Length)].Invoke();
            Instantiate(cherryPrefab, position, Quaternion.identity);
            counter = 0;
        }
        
    }

    public Vector3 RandomVertPoint()
    { 
        float y = UnityEngine.Random.Range(1,11)%2 == 0 ? -vertSize-1 : vertSize+1;
        float x = UnityEngine.Random.Range(-horSize-1, horSize+1);
        return new Vector3(x, y, 0);
    }
    
    public Vector3 RandomHorzPoint()
    {
        float x = UnityEngine.Random.Range(1,11)%2 == 0 ? -horSize-1 : horSize+1;
        float y = UnityEngine.Random.Range(-vertSize-1, vertSize+1);
        return new Vector3(x, y, 0);
    }
}
