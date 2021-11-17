using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameState : BaseState
{
    public bool loadGameContent = true;
    public bool destroyGameContent = true;
    public bool startNewRun = false;
    public bool skipToFinish = false;
    public bool nextLevel = false;

    
    public override void PrepareState()
    {
        base.PrepareState();

        if (skipToFinish)
        {
            owner.ChangeState(new GameOverState()); // TODO: pass here the game result
            return;
        }

        owner.UI.GameView.OnPauseClicked += PauseClicked;
        GameManager.instance.sceneLoaded += HandleSceneLoaded;
        GameManager.instance.sceneUnloaded += HandleSceneUnloaded;
        RunManager.instance.OnPlayerLoses += FinishClicked;
        RunManager.instance.OnScoreChanged += owner.UI.GameView.UpdateScoreValue;
        RunManager.instance.OnGoldChanged += owner.UI.GameView.UpdateGoldValue;
        RunManager.instance.OnDistanceChange += owner.UI.GameView.UpdateSlider;
        RunManager.instance.OnBossFightCloseToBegin += HandleBossScene;
        RunManager.instance.OnPlayerWin += HandlePlayerWin;
        PowerUpsManager.instance.OnValueToDashChanged += HandleValueToDash;
        PowerUpsManager.instance.OnDashCanBeUsedChanged += HandleDashCanBeUsed;
        
        // PowerUpsManager.instance.OnDashUsedChanged += HandleDashUsed;
        

        if (startNewRun)
        {
            RunManager.instance.currentState = RunManager.State.LevelOne;
            if (SceneManager.GetSceneByBuildIndex((int)Enums.SceneIndexes.LevelOne).isLoaded)
            {
                RunManager.instance.RestartRun();
                GameManager.instance.SetMenuCameraActive(false);
                owner.UI.GameView.ShowView();
            }
            else
            {
                GameManager.instance.SetStateLoadScene(true);
                GameManager.instance.UnloadAnotherScenes(new []{(int)Enums.SceneIndexes.Manager});    
            }
        }else if (nextLevel)
        {
            GameManager.instance.SetStateLoadScene(true);
            GameManager.instance.UnloadAnotherScenes(new []{(int)Enums.SceneIndexes.Manager});
        }
        else
        {
            GameManager.instance.SetMenuCameraActive(false);
            owner.UI.GameView.ShowView();
        }
    }

    
    public override void DestroyState()
    {
        if (destroyGameContent)
        {
            GameManager.instance.UnloadAnotherScenes(new []{(int)Enums.SceneIndexes.Manager});
        }
        owner.UI.GameView.HideView();
        
        owner.UI.GameView.OnPauseClicked -= PauseClicked;
        GameManager.instance.sceneUnloaded -= HandleSceneUnloaded;
        GameManager.instance.sceneLoaded -= HandleSceneLoaded;
        RunManager.instance.OnPlayerLoses -= FinishClicked;
        RunManager.instance.OnScoreChanged -= owner.UI.GameView.UpdateScoreValue;
        RunManager.instance.OnGoldChanged -= owner.UI.GameView.UpdateGoldValue;
        RunManager.instance.OnDistanceChange -= owner.UI.GameView.UpdateSlider;
        RunManager.instance.OnBossFightCloseToBegin -= HandleBossScene;
        RunManager.instance.OnPlayerWin -= HandlePlayerWin;
        PowerUpsManager.instance.OnValueToDashChanged -= HandleValueToDash;
        PowerUpsManager.instance.OnDashCanBeUsedChanged -= HandleDashCanBeUsed;
        // PowerUpsManager.instance.OnDashUsedChanged -= HandleDashUsed;
        GameManager.instance.SetMenuCameraActive(true);
        base.DestroyState();
    }

    private void HandleDashUsed(bool value)
    {
        // owner.UI.GameView.buttonActiveDash.interactable = !value;
    }

    private void HandleDashCanBeUsed(bool value)
    {
        owner.UI.GameView.buttonActiveDash.interactable = value;
    }

    private void HandleValueToDash(float value)
    {
        owner.UI.GameView.dashSlider.value = value;
    }

    private void PauseClicked()
    {
        destroyGameContent = false;
        owner.ChangeState(new PauseState<GameState>());
    }

    private void FinishClicked()
    {
        owner.ChangeState(new GameOverState());
    }

    private void HandleSceneLoaded()
    {
        GameManager.instance.SetMenuCameraActive(false);
        switch (RunManager.instance.currentState)
        {
            case RunManager.State.LevelOne:
                RunManager.instance.StartRun();
                owner.UI.GameView.ShowView();
                break;
            case RunManager.State.Boss:
                destroyGameContent = false;
                owner.ChangeState(new BossState());
                break;
            case RunManager.State.LevelTwo:
                RunManager.instance.StartNextLevel();
                owner.UI.GameView.ShowView();
                break;
        }
    }
    private void HandleBossScene()
    {
        RunManager.instance.currentState = RunManager.State.Boss;
        owner.UI.GameView.HideView();
        RunManager.instance.PauseRun();
        GameManager.instance.SetStateLoadScene(true,GameManager.LoadImageType.Boss);
        GameManager.instance.SetMenuCameraActive(true);
        GameManager.instance.UnloadAnotherScenes(new []{(int)Enums.SceneIndexes.Manager});
    }

    private void HandleSceneUnloaded()
    {
        switch (RunManager.instance.currentState)
        {
            case RunManager.State.LevelOne:
                GameManager.instance.LoadScenes(new []{(int)Enums.SceneIndexes.LevelOne});
                break;
            case RunManager.State.Boss:
                GameManager.instance.LoadScenes(new []{(int)Enums.SceneIndexes.Boss});
                
                break;
            case RunManager.State.LevelTwo:
                GameManager.instance.LoadScenes(new []{(int)Enums.SceneIndexes.LevelTwo});
                break;
        }
    }

    private void HandlePlayerWin()
    {
        owner.ChangeState(new WinState());
    }

}
