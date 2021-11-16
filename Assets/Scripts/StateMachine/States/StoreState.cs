using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreState : BaseState
{
    public override void PrepareState()
    {
        base.PrepareState();
        
        owner.UI.StoreView.OnLobbyClicked.AddListener(ClickLobby);
        owner.UI.StoreView.OnPlayClicked.AddListener(ClickPlay);
        PowerUpsManager.instance.onPowerUpLevelChange += owner.UI.StoreView.UpdateValues; 
        GameManager.instance.OnGoldChanged += owner.UI.StoreView.UpdateValues; 
        owner.UI.StoreView.UpdateValues();
        owner.UI.StoreView.ShowView();
    }

    public override void DestroyState()
    {
        owner.UI.StoreView.HideView();
        owner.UI.StoreView.OnPlayClicked.RemoveListener(ClickPlay);
        owner.UI.StoreView.OnLobbyClicked.RemoveListener(ClickLobby);
        PowerUpsManager.instance.onPowerUpLevelChange -= owner.UI.StoreView.UpdateValues;
        GameManager.instance.OnGoldChanged -= owner.UI.StoreView.UpdateValues; 
        base.DestroyState();
    }

    private void ClickLobby()
    {
        owner.ChangeState(new LobbyState());
    }

    private void ClickPlay()
    {
        owner.ChangeState(new GameState());
    }
}
