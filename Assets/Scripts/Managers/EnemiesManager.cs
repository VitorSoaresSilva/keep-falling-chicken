using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class EnemiesManager : Singleton<EnemiesManager>
{
    [SerializeField] private int amountEachEnemy = 2;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float distanceBetweenSpawns = 10;
    [SerializeField] private Transform positionOutOfCamera; 
    [SerializeField] private Transform transformToSpawn;
    public float currSpeed { get; private set; }
    public Vector3 scale;
    
    #region Initializition
        [HideInInspector] public float progress;
        [HideInInspector] public bool isDone;
    #endregion
    
    public int[] weightOfSpawnCategories;
    [SerializeField] private int[] poolOfCategoriesToSpawns;
    [SerializeField] private int indexOfCategoriesToSpawn = 0;
    private Coroutine spawnCoroutine;
    [SerializeField] private bool SpawnActive = true;

    [Header("Enemies pool")]
    private int enemiesPoolIndex = 0;
    [SerializeField] private Transform[] enemiesPool;
    private EnemyMove[] enemiesPoolScript;
    [SerializeField] private GameObject[] assetsOfEnemiesToSpawn;
    private int nextEnemiesShuffle;

    [Header("Gold Spawn")]
    [SerializeField] private GameObject[] assetsOfGoldToSpawn;
    
    [Header("Power Up Spawn")]
    [SerializeField] private GameObject[] assetsOfPowerUpToSpawn;
    


    
    IEnumerator InitializePool()
    {
        int count = 0;
        for (int i = 0; i < assetsOfEnemiesToSpawn.Length; i++)
        {
            for (int j = 0; j < amountEachEnemy; j++)
            {
                GameObject temp = Instantiate(assetsOfEnemiesToSpawn[i], positionOutOfCamera.position, Quaternion.identity,transform);
                Transform tempTransform = temp.transform;
                for (int k = 0; k < temp.transform.childCount; k++)
                {
                    Transform childTransform = tempTransform.GetChild(k).transform;
                    Vector3 scaleResult = Vector3.Scale(childTransform.localPosition, new Vector3(scale.x,scale.y,1));
                    childTransform.localPosition = scaleResult;
                }
                enemiesPool[i * amountEachEnemy + j] = tempTransform;
                enemiesPoolScript[i * amountEachEnemy + j] = temp.GetComponent<EnemyMove>();
                Initialize(i * amountEachEnemy + j);
                count++;
                progress = (float)count / (float)enemiesPool.Length;
            }
        }
        Shuffle(0,enemiesPool.Length,ref enemiesPoolScript);
        nextEnemiesShuffle = enemiesPool.Length - 1;
        yield return new WaitForSeconds(0.5f);
        isDone = true;
    }

    private void Start()
    {
        PowerUpsManager.instance.OnDashUsedChanged += HandleDashUsed;
        enemiesPool = new Transform[amountEachEnemy * assetsOfEnemiesToSpawn.Length];
        enemiesPoolScript = new EnemyMove[amountEachEnemy * assetsOfEnemiesToSpawn.Length];
        currSpeed = 0;
        InitializeSpawnPool();
        StartCoroutine(nameof(InitializePool));
        ActivateEnemies();
    }

    private void HandleDashUsed(float arg0)
    { 
        currSpeed += arg0;
    }

    public void InitializeSpawnPool()
    {
        int total = 0;
        foreach (var category in weightOfSpawnCategories)
        {
            total += category;
        }
        poolOfCategoriesToSpawns = new int[total];
        int index = 0;
        for (int i = 0; i < weightOfSpawnCategories.Length; i++)
        {
            for (int j = 0; j < weightOfSpawnCategories[j]; j++)
            {
                poolOfCategoriesToSpawns[index] = i;
                index++;
            }
        }
        Shuffle(0,total,ref poolOfCategoriesToSpawns);
    }
    public void ActivateEnemies()
    {
        currSpeed = baseSpeed;
        spawnCoroutine = StartCoroutine(nameof(Spawn));
    }

    public void StopEnemies()
    {
        StopCoroutine(spawnCoroutine);
    }

    private int GetUsable(int index)
    {
        
        return enemiesPoolScript.Where((t, i) => enemiesPoolScript[(i + index) % enemiesPoolScript.Length].transform.position.z < -20).Count();
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            if(!SpawnActive) yield return null;
            if (nextEnemiesShuffle == 0)
            {
                Shuffle(enemiesPoolIndex,GetUsable(enemiesPoolIndex),ref enemiesPoolScript);
                nextEnemiesShuffle = GetUsable(enemiesPoolIndex);
            }

            switch (poolOfCategoriesToSpawns[indexOfCategoriesToSpawn])
            {
                case 0:
                    // if (enemiesPool[enemiesPoolIndex].position.z < -20)
                    // {
                        nextEnemiesShuffle--;
                        Initialize(enemiesPoolIndex);
                        enemiesPoolScript[enemiesPoolIndex].Spawn();
                        // enemiesPoolScript[enemiesPoolIndex].transform.position
                        enemiesPoolIndex = (enemiesPoolIndex + 1) % enemiesPoolScript.Length;
                    // }
                    break;
                case 1:
                    Debug.Log("Power up");
                    SpawnPowerUps();
                    break;
                case 2:
                    Debug.Log("Gold");
                    SpawnGold();
                    break;
            }

            indexOfCategoriesToSpawn = (indexOfCategoriesToSpawn + 1) % poolOfCategoriesToSpawns.Length;
            
            yield return new WaitForSeconds(TimeBetweenSpawn());
        }
    }

    private void SpawnGold()
    {
        GameObject temp = Instantiate(assetsOfGoldToSpawn[Random.Range(0,assetsOfGoldToSpawn.Length)], positionOutOfCamera.position, Quaternion.identity,transform);
        Transform tempTransform = temp.transform;
        for (int k = 0; k < temp.transform.childCount; k++)
        {
            Transform childTransform = tempTransform.GetChild(k).transform;
            Vector3 scaleResult = Vector3.Scale(childTransform.localPosition, new Vector3(scale.x,scale.y,1));
            childTransform.localPosition = scaleResult;
        }

        if (temp.TryGetComponent(out SimpleMove simpleMove))
        {
            tempTransform.position = Vector3.Scale(simpleMove.GetRandomDirection(), scale) + transformToSpawn.position;
        }
        else
        {
            tempTransform.position = transformToSpawn.position;
        }
    }

    private void SpawnPowerUps()
    {
        GameObject temp = Instantiate(assetsOfPowerUpToSpawn[Random.Range(0,assetsOfPowerUpToSpawn.Length)], positionOutOfCamera.position, Quaternion.identity,transform);
        Transform tempTransform = temp.transform;
        if (temp.TryGetComponent(out SimpleMove simpleMove))
        {
            tempTransform.position = Vector3.Scale(simpleMove.GetRandomDirection(), scale) + transformToSpawn.position;
        }
        else
        {
            tempTransform.position = transformToSpawn.position;
        }
    }

    private void Initialize(int index)
    {
        Vector3[] positionsWaypoint = new Vector3[4];
        float distance = transformToSpawn.position.z / 2;
        for (int i = 0; i < positionsWaypoint.Length; i++)
        {
            Vector3 direction = enemiesPoolScript[index].GetRandomDirection();
            Vector3 positionToSpawn = transformToSpawn.position;
            positionsWaypoint[i] = positionToSpawn + Vector3.Scale(direction,scale);
            positionsWaypoint[i].z = positionToSpawn.z - distance * i;
        }
        
        enemiesPoolScript[index].Initialize(positionsWaypoint);
    }
    private float TimeBetweenSpawn()
    {
        return distanceBetweenSpawns / currSpeed;
    }
    private void Shuffle<T>(int begin,int n,ref T[] array)
    {
        for (int i = 0; i < n; i++)
        {
            int index = (begin + n - i - 1) % array.Length;
            int rand = (begin + Random.Range(0, n - 1)) % array.Length;
            if (index != rand)
            {
                Swap( array,index,  rand);
            }
        }
        
    }
    
    private void Swap<T>(IList<T> array,int a, int b)
    {
        T temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }

    public void Restart()
    {
        for (int i = 0; i < enemiesPool.Length; i++)
        {
            enemiesPool[i].position = positionOutOfCamera.position;
        }
        Shuffle(0,enemiesPool.Length,ref enemiesPoolScript);
        ActivateEnemies();
    }

    public void Destroy(Transform t)
    {
        t.position = positionOutOfCamera.position;
    }
}
