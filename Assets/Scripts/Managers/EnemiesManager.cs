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
    [SerializeField] private GameObject[] assetsToSpawn;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float distanceBetweenSpawns = 10;
    [SerializeField] private Transform positionOutOfCamera; 
    [SerializeField] private Transform positionToSpawn;
    public float currSpeed { get; private set; }

    [SerializeField] private bool SpawnActive = true;
    private int nextShuffle;
    private float scaleToX = 3;
    private float scaleToY = 3;
    #region Initializition
        public float progress;
        public bool isDone;
    #endregion

    [SerializeField] private int poolIndex = 0;
    [SerializeField] private Transform[] pool;

    private Coroutine spawnCoroutine;

    IEnumerator InitializePool()
    {
        int count = 0;
        for (int i = 0; i < assetsToSpawn.Length; i++)
        {
            for (int j = 0; j < amountEachEnemy; j++)
            {
                GameObject temp = Instantiate(assetsToSpawn[i], positionOutOfCamera.position, Quaternion.identity,transform);
                Transform tempTransform = temp.transform;
                for (int k = 0; k < temp.transform.childCount; k++)
                {
                    Transform childTransform = tempTransform.GetChild(k).transform;
                    Vector3 scaleResult = Vector3.Scale(childTransform.localPosition, new Vector3(scaleToX,scaleToY,1));
                    childTransform.localPosition = scaleResult;
                }
                pool[i * amountEachEnemy + j] = tempTransform;
                count++;
                progress = (float)count / (float)pool.Length;
            }
        }
        Shuffle(0,pool.Length);
        nextShuffle = pool.Length - 1;
        yield return new WaitForSeconds(0.5f);
        isDone = true;
    }

    private void Start()
    {
        pool = new Transform[amountEachEnemy * assetsToSpawn.Length];
        currSpeed = 0;
        StartCoroutine(nameof(InitializePool));
        ActivateEnemies();
    }

    public void ActivateEnemies()
    {
        Debug.Log("Enemies Activated");
        currSpeed = baseSpeed;
        spawnCoroutine = StartCoroutine(nameof(Spawn));
    }

    public void StopEnemies()
    {
        StopCoroutine(spawnCoroutine);
    }

    private int GetUsable(int index)
    {
        return pool.Where((t, i) => pool[(i + index) % pool.Length].position.z < -20).Count();
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            if(!SpawnActive) yield return null;
            if (nextShuffle == 0)
            {
                Shuffle(poolIndex,GetUsable(poolIndex));
                nextShuffle = GetUsable(poolIndex);
            }
            nextShuffle--;
            MoveToPosition(poolIndex);
            poolIndex = (poolIndex + 1) % pool.Length;
            yield return new WaitForSeconds(TimeBetweenSpawn());
        }
    }

    private void MoveToPosition(int index)
    {
        pool[index].position = positionToSpawn.position;
    }

    private float TimeBetweenSpawn()
    {
        return distanceBetweenSpawns / currSpeed;
    }
    private void Shuffle(int begin,int n)
    {
        for (int i = 0; i < n; i++)
        {
            int index = (begin + n - i - 1) % pool.Length;
            int rand = (begin + Random.Range(0, n - 1)) % pool.Length;
            if (index != rand)
            {
                Swap(ref pool[index], ref pool[rand]);
            }
        }
        
    }
    
    private void Swap(ref Transform a,ref Transform b)
    {
        (a, b) = (b, a);
    }

    public void Restart()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i].position = positionOutOfCamera.position;
        }
        Shuffle(0,pool.Length);
        ActivateEnemies();
    }
}
