using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using Random = UnityEngine.Random;

public class RunManager : Singleton<RunManager>
{
    public RunData runData { get; private set; }
    private Coroutine scoreCoroutine;
    private bool isRunning;

    public UnityAction<int> OnGoldChanged;
    public UnityAction<int> OnScoreChanged;


    public UnityAction OnGamePaused;
    public UnityAction OnGameResume;
    public UnityAction<float> OnDistanceChange;
    public UnityEvent OnPlayerTakeDamage;
    


    public float timeBaseToBoss = 120;
    public float timeCurrent;
    [SerializeField] private float[] hitsPunishment;
    [SerializeField] private int timesPlayerHits;

    private void Start()
    {
        OnPlayerTakeDamage.AddListener(HandlePlayerHits);
    }

    private void OnDisable()
    {
        OnPlayerTakeDamage.RemoveListener(HandlePlayerHits);
    }

    public void StartRun()
    {
        runData = new RunData();
        isRunning = true;
        scoreCoroutine = StartCoroutine(nameof(Score));
        timeCurrent = 0;
OnGoldChanged?.Invoke(runData.gold);
    }

    public void RestartRun()
    {
        StartRun();
        EnemiesManager.instance.Restart();
    }

    public void StopRun()
    {
        StopCoroutine(scoreCoroutine);
        EnemiesManager.instance.StopEnemies();
        GameManager.instance.EarnData(runData);
    }

    public void PauseRun()
    {
        
    }

    public void CollectGold(int gold)
    {
        runData.gold += gold;
        OnGoldChanged?.Invoke(runData.gold);
    }

    public void CollectDiamond(int diamond)
    {
        runData.diamond += diamond;
    }
    public RunData GetData()
    {
        return runData;
    }

    IEnumerator Score()
    {
        while (isRunning)
        {
            runData.score += 1;
            OnScoreChanged?.Invoke(runData.score);
            timeCurrent++;
            OnDistanceChange?.Invoke(timeCurrent / timeBaseToBoss);
            if (timeCurrent >= timeBaseToBoss)
            {
                Debug.Log("Boss fight!");
                // EnemiesManager.instance.
            }
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    private void HandlePlayerHits()
    {
        timesPlayerHits++;
        if (timesPlayerHits > hitsPunishment.Length)
        {
            Debug.Log("Player dies");
        }
        else
        {
            timeCurrent =Mathf.Clamp(hitsPunishment[timesPlayerHits - 1],0,timeBaseToBoss);
            OnDistanceChange?.Invoke(timeCurrent / timeBaseToBoss);
        }
    }
    
}
