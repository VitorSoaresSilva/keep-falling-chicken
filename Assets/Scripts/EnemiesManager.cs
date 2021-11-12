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

    [SerializeField] private float currSpeedMovement;
    public float CurrSpeedMovement
    {
        get => currSpeedMovement;
        private set
        {
            currSpeedMovement = value;
            timeBetweenSpawn = currSpeedMovement!= 0 ? Distance / CurrSpeedMovement : 0;
        }
    }

    [SerializeField] private float distance;
    public float Distance
    {
        get => distance;
        set
        {
            distance = value;
            timeBetweenSpawn = currSpeedMovement!= 0 ? Distance / CurrSpeedMovement : 0;
        }
    }
    
    [SerializeField] private float speedMovement;


    public float timeBetweenSpawn;
    
    
    
    
    
    public static EnemiesManager Instance { get; private set; }
    private GameObject holder;

    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
        timeBetweenSpawn = currSpeedMovement!= 0 ? Distance / CurrSpeedMovement : 0;
    }

    public void ChangeState(bool newState){
        isActive = newState;
        if(newState){
            StartRun();
        }else{
            StopRun();
        }
    }

    public void StartRun()
    {
        CurrSpeedMovement = speedMovement;
        StartCoroutine(nameof(SpawnObstaclesWithChild));
        holder = new GameObject("Holder");
        Instantiate(holder, Vector3.zero, Quaternion.identity);
    }

    public void StopRun()
    {
        CurrSpeedMovement = 0;
        StopCoroutine(nameof(SpawnObstaclesWithChild));
        Destroy(holder);
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
            GameObject temp =  Instantiate(obstacles[GetRandomIndex()],positionToSpawn.position,Quaternion.identity,positionToSpawn);
            MovableObstacle movableObstacle = temp.GetComponent<MovableObstacle>();
            Vector3 direction = movableObstacle.GetRandomPosition();
            temp.transform.position = Vector3.Scale(direction,new Vector3(GameManager.Instance.scaleToTrackMove, GameManager.Instance.scaleToHeightMove,positionToSpawn.position.z));
            for(int i = 0; i < temp.transform.childCount; i++)
            {
                Vector3 scaleResult = Vector3.Scale(temp.transform.GetChild(i).transform.localPosition,
                    new Vector3(GameManager.Instance.scaleToTrackMove,
                        GameManager.Instance.scaleToHeightMove,1));
                temp.transform.GetChild(i).transform.localPosition = scaleResult;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    private int GetRandomIndex(){
        return Random.Range(0,obstacles.Length);
    }

    public void ChangeSpeed(float value)
    {
        CurrSpeedMovement = value;
    }

    public void KillAllObstacles()
    {
    }

    

}
