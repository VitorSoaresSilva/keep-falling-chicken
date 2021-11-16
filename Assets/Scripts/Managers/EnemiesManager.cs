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
    [SerializeField] private Transform positionToSpawn;
    [SerializeField] private GameObject ColliderToBoss;
    public float currSpeed { get; private set; }

    private float scaleToX = 3;
    private float scaleToY = 3;
    
    #region Initializition
        public float progress;
        public bool isDone;
    #endregion
    
    public int[] weightOfSpawnCategories;
    private int[] poolOfCategoriesToSpawns;
    private int indexOfCategoriesToSpawn = 0;
    private Coroutine spawnCoroutine;
    [SerializeField] private bool SpawnActive = true;

    [Header("Enemies pool")]
    [SerializeField] private int enemiesPoolIndex = 0;
    [SerializeField] private Transform[] enemiesPool;
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
                    Vector3 scaleResult = Vector3.Scale(childTransform.localPosition, new Vector3(scaleToX,scaleToY,1));
                    childTransform.localPosition = scaleResult;
                }
                enemiesPool[i * amountEachEnemy + j] = tempTransform;
                count++;
                progress = (float)count / (float)enemiesPool.Length;
            }
        }
        Shuffle(0,enemiesPool.Length,enemiesPool);
        nextEnemiesShuffle = enemiesPool.Length - 1;
        yield return new WaitForSeconds(0.5f);
        isDone = true;
    }

    private void Start()
    {
        ColliderToBoss.SetActive(false);
        enemiesPool = new Transform[amountEachEnemy * assetsOfEnemiesToSpawn.Length];
        currSpeed = 0;
        InitializeSpawnPool();
        StartCoroutine(nameof(InitializePool));
        ActivateEnemies();
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
        for (int i = 0; i < total; i++)
        {
            for (int j = 0; j < weightOfSpawnCategories[j]; j++)
            {
                poolOfCategoriesToSpawns[index] = i;
                index ++;
            }
        }
        Shuffle(0,total,poolOfCategoriesToSpawns);
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
        return enemiesPool.Where((t, i) => enemiesPool[(i + index) % enemiesPool.Length].position.z < -20).Count();
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            if(!SpawnActive) yield return null;
            if (nextEnemiesShuffle == 0)
            {
                Shuffle(enemiesPoolIndex,GetUsable(enemiesPoolIndex),enemiesPool);
                nextEnemiesShuffle = GetUsable(enemiesPoolIndex);
            }

            switch (poolOfCategoriesToSpawns[indexOfCategoriesToSpawn])
            {
                case 1:
                    nextEnemiesShuffle--;
                    MoveToPosition(enemiesPoolIndex);
                    enemiesPoolIndex = (enemiesPoolIndex + 1) % enemiesPool.Length;
                    break;
                case 2:
                    Debug.Log("Power up");
                    break;
                case 3:
                    Debug.Log("Gold");
                    SpawnGold();
                    break;
                default:
                    nextEnemiesShuffle--;
                    MoveToPosition(enemiesPoolIndex);
                    enemiesPoolIndex = (enemiesPoolIndex + 1) % enemiesPool.Length;
                    break;
            }

            indexOfCategoriesToSpawn = (indexOfCategoriesToSpawn + 1) % poolOfCategoriesToSpawns.Length;
            
            yield return new WaitForSeconds(TimeBetweenSpawn());
        }
    }

    public void SpawnColliderToBoss()
    {
        ColliderToBoss.SetActive(true);
        ColliderToBoss.transform.position = positionToSpawn.position;
    }

    private void SpawnGold()
    {
        GameObject temp = Instantiate(assetsOfGoldToSpawn[Random.Range(0,assetsOfGoldToSpawn.Length)], positionOutOfCamera.position, Quaternion.identity,transform);
        Transform tempTransform = temp.transform;
        for (int k = 0; k < temp.transform.childCount; k++)
        {
            Transform childTransform = tempTransform.GetChild(k).transform;
            Vector3 scaleResult = Vector3.Scale(childTransform.localPosition, new Vector3(scaleToX,scaleToY,1));
            childTransform.localPosition = scaleResult;
        }

        tempTransform.position = positionToSpawn.position;
    }

    private void SpawnPowerUps()
    {
        GameObject temp = Instantiate(assetsOfPowerUpToSpawn[Random.Range(0,assetsOfGoldToSpawn.Length)], positionOutOfCamera.position, Quaternion.identity,transform);
        Transform tempTransform = temp.transform;
        tempTransform.position = positionToSpawn.position;
    }

    private void MoveToPosition(int index)
    {
        enemiesPool[index].position = positionToSpawn.position;
    }

    private float TimeBetweenSpawn()
    {
        return distanceBetweenSpawns / currSpeed;
    }
    private void Shuffle<T>(int begin,int n,T[] array)
    {
        for (int i = 0; i < n; i++)
        {
            int index = (begin + n - i - 1) % array.Length;
            int rand = (begin + Random.Range(0, n - 1)) % array.Length;
            if (index != rand)
            {
                Swap(ref array[index], ref array[rand]);
            }
        }
        
    }
    
    private void Swap<T>(ref T a,ref T b)
    {
        (a, b) = (b, a);
    }

    public void Restart()
    {
        for (int i = 0; i < enemiesPool.Length; i++)
        {
            enemiesPool[i].position = positionOutOfCamera.position;
        }
        Shuffle(0,enemiesPool.Length,enemiesPool);
        ActivateEnemies();
    }
}
