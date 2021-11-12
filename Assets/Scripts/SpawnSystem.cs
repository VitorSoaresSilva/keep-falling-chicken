using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
// using UnityEngine.Events;
public class SpawnSystem : MonoBehaviour
{
    public Action batata;
    
    public GameObject[] assets;
    [SerializeField] private Transform[] blocks;


    [Header("")] 
    public int amountOfEach = 5;
    public Transform pointToSpawn;
    public Transform pointToSpawnOutScreen;

    private int index = 0;
    private int Index
    {
        get => index;
        set => index = (value % (assets.Length * amountOfEach));
    }
    
    private void Awake()
    {
        blocks = new Transform[assets.Length * amountOfEach];
        for (int i = 0; i < assets.Length; i++)
        {
            for (int j = 0; j < amountOfEach; j++)
            {
                GameObject temp = Instantiate(assets[i], pointToSpawnOutScreen.position, Quaternion.identity,
                    this.transform);
                MovableV2 movableObstacle =  temp.AddComponent<MovableV2>();
                // movableObstacle.enabled = false;
                blocks[i * amountOfEach + j] = temp.transform;
            }
        }
        Shuffle(0,blocks.Length-1);
    }

    private void Start()
    {
        StartCoroutine(nameof(Spawn));
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            batata?.Invoke();
            blocks[Index].position = pointToSpawn.position;
            Index++;
            print(EnemiesManager.Instance.Distance);
            yield return new WaitForSeconds(EnemiesManager.Instance.timeBetweenSpawn);
        }
    }



    private void Shuffle(int begin,int end)
    {
        int tempIndex = end;
        for (int i = end; i > begin; i--)
        {
            int indexToSwap = Random.Range(begin, i);
            // Debug.Log(indexToSwap);
            if (indexToSwap != i)
            {
                Swap(ref blocks[i], ref blocks[indexToSwap]);
            }
        }
    }

    private void Swap(ref Transform a,ref Transform b)
    {
        (a, b) = (b, a);
    }
}
