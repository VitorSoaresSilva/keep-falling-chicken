using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUShield : PowerUp
{
    public override void Use()
    {
        SceneDataHolder.instance.player.shieldObject.SetActive(true);
        SceneDataHolder.instance.player.OnPlayerInivincibleHit.AddListener(HandlePlayerHit);
        inUse = true;
        PowerUpsManager.instance.playerInvincible = true;
        StateMachine.instance.UI.GameView.shieldsIcon.SetActive(true);
        StartCoroutine(nameof(Disable));
    }

    protected override void AfterDisable()
    {
        base.AfterDisable();
        PowerUpsManager.instance.playerInvincible = false;
        SceneDataHolder.instance.player.shieldObject.SetActive(false);
        SceneDataHolder.instance.player.OnPlayerInivincibleHit.RemoveListener(HandlePlayerHit);
        
    }

    private void HandlePlayerHit()
    {
        StopAllCoroutines();
        inUse = false;
        PowerUpsManager.instance.playerInvincible = false;
        SceneDataHolder.instance.player.OnPlayerInivincibleHit.RemoveListener(HandlePlayerHit);
        SceneDataHolder.instance.player.shieldObject.SetActive(false);
        StateMachine.instance.UI.GameView.shieldsIcon.SetActive(false);
    }
}
