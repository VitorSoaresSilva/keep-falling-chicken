using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameState : BaseState
{
    public bool loadGameContent = true;
    public bool destroyGameContent = true;
    public bool startNewRun = true;
    // public bool newGame = true;

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
        RunManager.instance.OnScoreChanged += owner.UI.GameView.UpdateScoreValue;
        RunManager.instance.OnGoldChanged += owner.UI.GameView.UpdateGoldValue;
        RunManager.instance.OnDistanceChange += owner.UI.GameView.UpdateSlider;

        if (startNewRun)
        {
            // SceneManager.
            /*
             * levelOne is loaded?
             * Enemies manager restart position
             * run manager start run
             */
            if (SceneManager.GetSceneByBuildIndex((int)Enums.SceneIndexes.LevelOne).isLoaded)
            {
                RunManager.instance.RestartRun();
                GameManager.instance.SetMenuCameraActive(false);
                owner.UI.GameView.ShowView();
            }
            else
            {
                GameManager.instance.LoadScenes(new []{(int)Enums.SceneIndexes.LevelOne});    
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
            //TODO: unload
            // SceneManager.sceneCount
            //     
            //     
            // SceneManager.UnloadSceneAsync((int)Enums.SceneIndexes.LevelOne);
            // SceneManager.UnloadSceneAsync((int)Enums.SceneIndexes.Boss);
            // SceneManager.UnloadSceneAsync((int)Enums.SceneIndexes.LevelTwo);
        }
        
        owner.UI.GameView.HideView();

        owner.UI.GameView.OnFinishClicked -= FinishClicked;
        owner.UI.GameView.OnPauseClicked -= PauseClicked;
        GameManager.instance.sceneLoaded -= HandleSceneLoaded;
        RunManager.instance.OnScoreChanged -= owner.UI.GameView.UpdateScoreValue;
        RunManager.instance.OnGoldChanged -= owner.UI.GameView.UpdateGoldValue;
        RunManager.instance.OnDistanceChange -= owner.UI.GameView.UpdateSlider;
        GameManager.instance.SetMenuCameraActive(true);
        base.DestroyState();
    }

    private void PauseClicked()
    {
        destroyGameContent = false;
        owner.ChangeState(new PauseState());
    }

    private void FinishClicked()
    {
        owner.ChangeState(new GameOverState());
    }

    private void HandleSceneLoaded()
    {
        GameManager.instance.SetMenuCameraActive(false);
        RunManager.instance.StartRun();
        owner.UI.GameView.ShowView();
    }
}
