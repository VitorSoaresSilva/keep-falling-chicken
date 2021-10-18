using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    private bool isActive = true;
    [SerializeField] private float timeBetweenSpawns = 2;
    [SerializeField] private Transform positionToSpawn;
    public float currSpeedMovement;
    [SerializeField] private float speedMovement;
    public static EnemiesManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ChangeState(bool newState){
        isActive = newState;
        if(newState){
            // Time.timeScale = 1;
            currSpeedMovement = speedMovement;
            StartCoroutine(nameof(SpawnObstaclesWithChild));
        }else{
            // Time.timeScale = 0;
            currSpeedMovement = 0;
            StopCoroutine(nameof(SpawnObstaclesWithChild));
        }
    }
    IEnumerator SpawnObstacles(){       
        while(true){
            GameObject temp =  Instantiate(obstacles[GetRandomIndex()],positionToSpawn);
            MovableObstacle movableObstacle = temp.GetComponent<MovableObstacle>();
            Vector3 direction = movableObstacle.GetRandomPosition();
            temp.transform.position = Vector3.Scale(direction,new Vector3(GameManager.Instance.scaleToTrackMove,GameManager.Instance.scaleToHeightMove,positionToSpawn.position.z));
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    IEnumerator SpawnObstaclesWithChild(){       
        while(true){ 
            GameObject temp =  Instantiate(obstacles[GetRandomIndex()],positionToSpawn.position,Quaternion.identity);
            MovableObstacle movableObstacle = temp.GetComponent<MovableObstacle>();
            Vector3 direction = movableObstacle.GetRandomPosition();
            temp.transform.position = Vector3.Scale(direction,new Vector3(GameManager.Instance.scaleToTrackMove, GameManager.Instance.scaleToHeightMove,positionToSpawn.position.z));
            for(int i = 0; i < temp.transform.childCount; i++)
            {
                Vector3 scaleREsult = Vector3.Scale(temp.transform.GetChild(i).transform.localPosition,
                    new Vector3(GameManager.Instance.scaleToTrackMove,
                        GameManager.Instance.scaleToHeightMove,1));
                temp.transform.GetChild(i).transform.localPosition = scaleREsult;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    private int GetRandomIndex(){
        return Random.Range(0,obstacles.Length);
    }

    public void ChangeSpeed(float value)
    {
        currSpeedMovement = value;
    }

    

}
