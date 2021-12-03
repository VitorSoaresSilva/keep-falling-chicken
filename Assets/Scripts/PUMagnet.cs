using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUMagnet : PowerUp
{
    [SerializeField] private GameObject magnetCollider;
    public override void Use()
    {
        inUse = true;
        magnetCollider.SetActive(true);
        SceneDataHolder.instance.player.magnetObject.SetActive(true);
        StateMachine.instance.UI.GameView.dashIcon.SetActive(false);
        StateMachine.instance.UI.GameView.doublePointsIcon.SetActive(false);
        StateMachine.instance.UI.GameView.shieldsIcon.SetActive(false);
        StateMachine.instance.UI.GameView.magnetIcon.SetActive(true);
        
        
        StartCoroutine(nameof(Disable));
    }

    public override void StartRun()
    {
        base.StartRun();
        magnetCollider.SetActive(false);
    }

    protected override void AfterDisable()
    {
        base.AfterDisable();
        magnetCollider.SetActive(false);
        SceneDataHolder.instance.player.magnetObject.SetActive(false);
        StateMachine.instance.UI.GameView.magnetIcon.SetActive(false);
    }
}
