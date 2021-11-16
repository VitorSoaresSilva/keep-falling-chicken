using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : BaseState
{
    public override void PrepareState()
    {
        base.PrepareState();
        GameManager.instance.SetStateLoadScene(false);
        owner.UI.BossView.OnPauseClicked += PauseClicked;
        owner.UI.GameView.ShowView();
        
    }

    public override void DestroyState()
    {
        owner.UI.GameView.HideView();
        //TODO: verify if we need to destroy content ow if we destroy in gameState
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
}
