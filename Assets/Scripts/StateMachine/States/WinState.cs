using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : BaseState
{
    
    public override void PrepareState()
    {
        base.PrepareState();

        owner.UI.WinView.OnMenuClicked += MenuClicked;
        owner.UI.WinView.OnReplayClicked += ReplayClicked;
        owner.UI.WinView.OnStoreClicked += StoreClicked;
        
        owner.UI.WinView.data = RunManager.instance.GetData();;
        GameManager.instance.EarnData(RunManager.instance.GetData());
        
        owner.UI.WinView.UpdateValues();
        RunManager.instance.StopRun();
        owner.UI.WinView.ShowView();
    }

    public override void DestroyState()
    {
        owner.UI.WinView.HideView();
        
        owner.UI.WinView.OnMenuClicked -= MenuClicked;
        owner.UI.WinView.OnReplayClicked -= ReplayClicked;
        owner.UI.WinView.OnStoreClicked -= StoreClicked;
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
        owner.ChangeState(new LobbyState());
    }
}
