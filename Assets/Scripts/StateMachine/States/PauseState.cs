using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState<T> : BaseState where T: BaseState,new()
{
    // public T lastState;
    public bool resumeGame = true;
    public override void PrepareState()
    {
        base.PrepareState();

        Time.timeScale = 0;

        owner.UI.PauseView.OnMenuClicked += MenuClicked;
        owner.UI.PauseView.OnResumeClicked += ResumeClicked;
        owner.UI.PauseView.OnConfigClicked += ConfigClicked;
        
        owner.UI.PauseView.ShowView();
    }

    public override void DestroyState()
    {
        owner.UI.PauseView.HideView();
        
        owner.UI.PauseView.OnMenuClicked -= MenuClicked;
        owner.UI.PauseView.OnResumeClicked -= ResumeClicked;
        owner.UI.PauseView.OnConfigClicked -= ConfigClicked;

        if (resumeGame)
        {
            Time.timeScale = 1;
        }
        
        base.DestroyState();
    }

    private void MenuClicked()
    {
        resumeGame = false;
        owner.ChangeState(new GameState{skipToFinish = true});
    }
    private void ResumeClicked()
    {
        resumeGame = true;
        owner.ChangeState(new GameState());
    }
    private void ConfigClicked()
    {
        resumeGame = false;
        owner.ChangeState(new ConfigState<PauseState<T>>());
    }
}
