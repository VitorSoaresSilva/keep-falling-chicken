using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUDoublePoints : PowerUp
{

    public override void Use()
    {
        RunManager.instance.isDoublePointActive = true;
        
        StateMachine.instance.UI.GameView.dashIcon.SetActive(false);
        StateMachine.instance.UI.GameView.magnetIcon.SetActive(false);
        StateMachine.instance.UI.GameView.shieldsIcon.SetActive(false);
        StateMachine.instance.UI.GameView.doublePointsIcon.SetActive(true);
        StartCoroutine(nameof(Disable));
    }

    protected override void AfterDisable()
    {
        RunManager.instance.isDoublePointActive = false;
        StateMachine.instance.UI.GameView.doublePointsIcon.SetActive(false);
    }

    public override void StartRun()
    {
        RunManager.instance.isDoublePointActive = false;
        StateMachine.instance.UI.GameView.doublePointsIcon.SetActive(false);
    }
}
