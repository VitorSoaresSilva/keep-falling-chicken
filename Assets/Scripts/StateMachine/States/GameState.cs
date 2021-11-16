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

    
    public override void PrepareState()
    {
        base.PrepareState();

        if (skipToFinish)
        {
            owner.ChangeState(new GameOverState()); // TODO: pass here the game result
            return;
        }

        owner.UI.GameView.OnFinishClicked += FinishClicked;
        owner.UI.GameView.OnPauseClicked += PauseClicked;
        GameManager.instance.sceneLoaded += HandleSceneLoaded;
        GameManager.instance.sceneUnloaded += HandleSceneUnloaded;
        RunManager.instance.OnScoreChanged += owner.UI.GameView.UpdateScoreValue;
        RunManager.instance.OnGoldChanged += owner.UI.GameView.UpdateGoldValue;
        RunManager.instance.OnDistanceChange += owner.UI.GameView.UpdateSlider;
        RunManager.instance.OnBossFightCloseToBegin += HandleBossScene;

        if (startNewRun)
        {
            if (SceneManager.GetSceneByBuildIndex((int)Enums.SceneIndexes.LevelOne).isLoaded)
            {
                RunManager.instance.RestartRun();
                GameManager.instance.SetMenuCameraActive(false);
                owner.UI.GameView.ShowView();
            }
            else
            {
                GameManager.instance.SetStateLoadScene(true,GameManager.LoadImageType.Normal);
                GameManager.instance.LoadScenes(new []{(int)Enums.SceneIndexes.LevelOne});    
                GameManager.instance.UnloadAnotherScenes(new []{(int)Enums.SceneIndexes.LevelOne,(int)Enums.SceneIndexes.Manager});
            }
        }else
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
        owner.UI.GameView.OnFinishClicked -= FinishClicked;
        owner.UI.GameView.OnPauseClicked -= PauseClicked;
        GameManager.instance.sceneLoaded -= HandleSceneLoaded;
        RunManager.instance.OnScoreChanged -= owner.UI.GameView.UpdateScoreValue;
        RunManager.instance.OnGoldChanged -= owner.UI.GameView.UpdateGoldValue;
        RunManager.instance.OnDistanceChange -= owner.UI.GameView.UpdateSlider;
        RunManager.instance.OnBossFightCloseToBegin -= HandleBossScene;
        GameManager.instance.SetMenuCameraActive(true);
        base.DestroyState();
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
        switch (RunManager.instance.currentState)
        {
            case RunManager.State.LevelOne:
                GameManager.instance.SetMenuCameraActive(false);
                RunManager.instance.StartRun();
                owner.UI.GameView.ShowView();
                break;
            case RunManager.State.Boss:
                destroyGameContent = false;
                GameManager.instance.SetMenuCameraActive(false);
                owner.ChangeState(new BossState());
                break;
            case RunManager.State.LevelTwo:
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
                RunManager.instance.currentState = RunManager.State.Boss;
                GameManager.instance.LoadScenes(new []{(int)Enums.SceneIndexes.Boss});
                break;
            case RunManager.State.Boss:
                
                break;
            case RunManager.State.LevelTwo:
                break;
        }
    }

}
