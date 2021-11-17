using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : BaseState
{
    public override void PrepareState()
    {
        base.PrepareState();
        GameManager.instance.SetStateLoadScene(false);
        GameManager.instance.SetMenuCameraActive(false);
        
        RunManager.instance.OnPlayerLoses += FinishClicked;
        RunManager.instance.OnBossWin += HandleBossWin;
        owner.UI.BossView.OnPauseClicked += PauseClicked;
        
        owner.UI.BossView.ShowView();
        RunManager.instance.StartFakeBoss();
    }

    public override void DestroyState()
    {
        owner.UI.BossView.HideView();
        //TODO: verify if we need to destroy content ow if we destroy in gameState
        
        RunManager.instance.OnPlayerLoses -= FinishClicked;
        RunManager.instance.OnBossWin -= HandleBossWin;
        owner.UI.BossView.OnPauseClicked -= PauseClicked;
        
        base.DestroyState();
    }

    private void PauseClicked()
    {
        owner.ChangeState(new PauseState<BossState>());
    }
    
    private void FinishClicked()
    {
        owner.ChangeState(new GameOverState());
    }

    private void HandleBossWin()
    {
        RunManager.instance.currentState = RunManager.State.LevelTwo;
        GameManager.instance.SetMenuCameraActive(true);
        owner.ChangeState(new GameState(){nextLevel = true});
    }
}
