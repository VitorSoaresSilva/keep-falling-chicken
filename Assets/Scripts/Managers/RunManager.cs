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
    public UnityAction<float> OnDistanceTargetChange;
    public UnityEvent OnPlayerTakeDamage;
    public UnityAction OnBossFightCloseToBegin;

    public UnityAction OnPlayerLoses;
    public UnityAction OnPlayerWin;
    public UnityAction OnBossWin;
    


    public float timeBaseToBoss = 120;
    public float timeBaseToWin = 120;
    public float timeCurrent;
    [SerializeField] private float[] hitsPunishment;
    [SerializeField] private int timesPlayerHits;
    public enum State
    {
        LevelOne,
        Boss,
        LevelTwo
    }

    public State currentState;
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
        Debug.Log("Start run");
        runData = new RunData();
        currentState = State.LevelOne;
        timeCurrent = 0;
        OnDistanceTargetChange.Invoke(-1);
        OnDistanceTargetChange.Invoke(0.6f);
        OnGoldChanged?.Invoke(runData.gold);
        isRunning = true;
        scoreCoroutine = StartCoroutine(nameof(Score));
    }

    public void RestartRun()
    {
        StartRun();
        EnemiesManager.instance.Restart();
        timeCurrent = 0;
        timesPlayerHits = 0;
    }

    public void StopRun()
    {
        StopCoroutine(scoreCoroutine);
        if (EnemiesManager.instanceExists)
        {
            EnemiesManager.instance.StopEnemies();
        }
        GameManager.instance.EarnData(runData);
    }

    public void PauseRun()
    {
        StopCoroutine(scoreCoroutine);
        EnemiesManager.instance.StopEnemies();
    }

    public void CollectGold(int gold)
    {
        runData.gold += gold;
        OnGoldChanged?.Invoke(runData.gold);
    }

    // public void CollectDiamond(int diamond)
    // {
    //     runData.diamond += diamond;
    // }
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
                OnBossFightCloseToBegin?.Invoke();
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
            // OnDistanceTargetChange?.Invoke( hitsPunishment[timesPlayerHits - 1]);
            Invoke(nameof(PlayerLosesCall),1);
        }
        else
        {
            OnDistanceTargetChange?.Invoke( hitsPunishment[timesPlayerHits - 1]);
        }
    }

    public void HandlePlayerUsesDash()
    {
        timesPlayerHits--;
        OnDistanceTargetChange?.Invoke( -hitsPunishment[timesPlayerHits - 1]);
    }

    private void PlayerLosesCall()
    {
        OnPlayerLoses?.Invoke();
    }

    public void StartNextLevel()
    {
        StartCoroutine(nameof(ScoreLevelTwo));
    }
    IEnumerator ScoreLevelTwo()
    {
        while (isRunning)
        {
            runData.score += 1;
            OnScoreChanged?.Invoke(runData.score);
            timeCurrent++;
            OnDistanceChange?.Invoke(timeCurrent / timeBaseToWin);
            if (timeCurrent >= timeBaseToWin)
            {
                
                SpawnZequinha();
            }
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    public void ZequinhaCollected()
    {
        OnPlayerWin?.Invoke();
    }

    public void ZequinhaIsNear()
    {
        SceneDataHolder.instance.player.enabled = false;
        StateMachine.instance.UI.GameView.HideView();
        SceneDataHolder.instance.zequinha.GetComponent<SimpleMove>().enabled = false;
        SceneDataHolder.instance.playerScriptWin.enabled = true;
        
    }

    public void SpawnZequinha()
    {
        PauseRun();
        SceneDataHolder.instance.zequinha.SetActive(true);
    }


}
