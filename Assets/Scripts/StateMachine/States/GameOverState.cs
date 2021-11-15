using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : BaseState
{
    public RunData gameResult;

    public override void PrepareState()
    {
        base.PrepareState();

        owner.UI.GameOverView.OnMenuClicked += MenuClicked;
        owner.UI.GameOverView.OnReplayClicked += ReplayClicked;
        owner.UI.GameOverView.OnStoreClicked += StoreClicked;
        owner.UI.GameOverView.data = RunManager.instance.GetData();;
        owner.UI.GameOverView.UpdateValues();
        RunManager.instance.StopRun();
        owner.UI.GameOverView.ShowView();
    }

    public override void DestroyState()
    {
        owner.UI.GameOverView.HideView();
        
        owner.UI.GameOverView.OnMenuClicked -= MenuClicked;
        owner.UI.GameOverView.OnReplayClicked -= ReplayClicked;
        owner.UI.GameOverView.OnStoreClicked -= StoreClicked;
        Time.timeScale = 1;
        base.DestroyState();
    }

    private void ReplayClicked()
    {
        owner.ChangeState(new GameState(){startNewRun = true});
    }
    private void StoreClicked()
    {
        owner.ChangeState(new StoreState());
    }

    private void MenuClicked()
    {
        GameManager.instance.EarnData(RunManager.instance.runData);
        // RunManager.instance.runData = new RunData();
        owner.ChangeState(new LobbyState());
    }
}
