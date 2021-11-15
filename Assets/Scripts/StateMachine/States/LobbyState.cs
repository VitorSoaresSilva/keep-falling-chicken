using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbyState : BaseState
{
    public override void PrepareState()
    {
        base.PrepareState();
        
        owner.UI.LobbyView.OnPlayClicked.AddListener(PlayClicked);
        owner.UI.LobbyView.OnConfigClicked.AddListener(ConfigClicked);
        owner.UI.LobbyView.OnStoreClicked.AddListener(StoreClicked);
        GameManager.instance.OnGoldChanged += owner.UI.LobbyView.UpdateValues;
        owner.UI.LobbyView.UpdateValues();
        owner.UI.LobbyView.ShowView();

    }

    private void PlayClicked()
    {
        //TODO: verify if game content is already loaded
        // RunManager.instance.StartRun();
        owner.ChangeState(new GameState(){startNewRun = true});
    }

    public override void DestroyState()
    {
        owner.UI.LobbyView.HideView();
        owner.UI.LobbyView.OnPlayClicked.RemoveListener(PlayClicked);
        owner.UI.LobbyView.OnConfigClicked.RemoveListener(ConfigClicked);
        GameManager.instance.OnGoldChanged -= owner.UI.LobbyView.UpdateValues;
        base.DestroyState();
    }
    private void ConfigClicked()
    {
        owner.ChangeState(new ConfigState<LobbyState>{ lastState = this});
    }
    private void StoreClicked()
    {
        owner.ChangeState(new StoreState());
    }
    
}
