using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    private bool isActive = true;
    [SerializeField] private float timeBetweenSpawns = 2;
    [SerializeField] private Transform positionToSpawn;
    private void Start(){
        ChangeState(true);
    }

    public void ChangeState(bool newState){
        isActive = newState;
        if(newState == true){
            StartCoroutine(nameof(SpawnObstacles));
        }else{
            StopCoroutine(nameof(SpawnObstacles));
        }
    }
    IEnumerator SpawnObstacles(){
        while(true){
            GameObject temp =  Instantiate(obstacles[GetRandomIndex()],positionToSpawn) as GameObject;
            MovableObstacle movableObstacle = temp.GetComponent<MovableObstacle>();
            Vector3 direction = movableObstacle.GetRandomPosition();
            Debug.Log(positionToSpawn.position.z);
            temp.transform.position = Vector3.Scale(direction,new Vector3(GameManager.Instance.scaleToTrackMove,GameManager.Instance.scaleToHeightMove,positionToSpawn.position.z));
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    private int GetRandomIndex(){
        return Random.Range(0,obstacles.Length);
    }

}
