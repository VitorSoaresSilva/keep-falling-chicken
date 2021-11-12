using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle : MonoBehaviour
{
    public Vector3[] directionsToSpawn;
    private void Update(){
        transform.position -= (Vector3.forward * EnemiesManager.Instance.CurrSpeedMovement * Time.deltaTime);
        if (transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
    public Vector3 GetRandomPosition(){
        return directionsToSpawn[Random.Range(0,directionsToSpawn.Length)];
    }
}
