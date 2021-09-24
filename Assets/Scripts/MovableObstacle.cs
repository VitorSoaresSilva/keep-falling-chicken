using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle : MonoBehaviour
{
    public Vector3[] directionsToSpawn;
    private void Update(){
        transform.position += -Vector3.forward * GameManager.Instance.speedMovement * Time.deltaTime;
    }
    public Vector3 GetRandomPosition(){
        return directionsToSpawn[Random.Range(0,directionsToSpawn.Length)];
    }
}
