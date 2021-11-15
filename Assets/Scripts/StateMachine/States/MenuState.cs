using UnityEngine;
/// <summary>
/// Menu state that show Menu View and add interpret user interaction with that view
/// </summary>
public class MenuState : BaseState
{
    public override void PrepareState()
    {
        base.PrepareState();

        owner.UI.MenuView.OnQuitClicked += QuitClicked;
        owner.UI.MenuView.OnStartClicked.AddListener(StartClicked);
        owner.UI.MenuView.OnConfigClicked.AddListener(ConfigClicked);
        owner.UI.MenuView.OnStoreClicked.AddListener(StoreClicked);
        
        
        owner.UI.MenuView.ShowView();
    }
    public override void DestroyState()
    {
        owner.UI.MenuView.HideView();

        owner.UI.MenuView.OnQuitClicked -= QuitClicked;
        owner.UI.MenuView.OnStartClicked.RemoveListener(StartClicked);
        owner.UI.MenuView.OnConfigClicked.RemoveListener(ConfigClicked);
        owner.UI.MenuView.OnStoreClicked.RemoveListener(StoreClicked);
        
        base.DestroyState();
    }
    private void StartClicked()
    {
        owner.ChangeState(new LobbyState());
    }
    private void StoreClicked()
    {
        owner.ChangeState(new StoreState());
    }
    private void QuitClicked()
    {
        Application.Quit();
    }
    private void ConfigClicked()
    {
        owner.ChangeState(new ConfigState<MenuState>{ lastState = this});
    }

    
}
