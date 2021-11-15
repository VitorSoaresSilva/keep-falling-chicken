using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigState<T> : BaseState where T: BaseState,new()
{
    public T lastState;
    public override void PrepareState()
    {
        base.PrepareState();
        owner.UI.ConfigView.OnExitClicked.AddListener(ExitClicked);
        owner.UI.ConfigView.ShowView();
    }

    public override void DestroyState()
    {
        owner.UI.ConfigView.HideView();
        owner.UI.ConfigView.OnExitClicked.RemoveListener(ExitClicked);
        base.DestroyState();
    }

    private void ExitClicked()
    {
        owner.ChangeState(new T());
    }
}
