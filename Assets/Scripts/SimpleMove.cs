using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Vector3[] directionsToSpawn;
    void Update()
    {
        transform.position -= (Vector3.forward * Time.deltaTime * EnemiesManager.instance.currSpeed);
        if (transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
    

    public Vector3 GetRandomDirection()
    {
        if (directionsToSpawn.Length > 0)
        {
            return directionsToSpawn[Random.Range(0,directionsToSpawn.Length)];            
        }
        return Vector3.zero;
    }
}
