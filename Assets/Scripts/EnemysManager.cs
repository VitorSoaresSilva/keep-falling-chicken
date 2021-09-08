using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysManager : MonoBehaviour
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
            MovebleObstacle movebleObstacle = temp.GetComponent<MovebleObstacle>();
            movebleObstacle.speed = GameManager.Instance.speedMovement;
            Vector3 direction = movebleObstacle.GetRandomPosition();
            temp.transform.position = Vector3.Scale(direction,new Vector3(GameManager.Instance.scaleToTrackMove,GameManager.Instance.scaleToHeightMove,positionToSpawn.position.z));
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    private int GetRandomIndex(){
        return Random.Range(0,obstacles.Length);
    }

}
