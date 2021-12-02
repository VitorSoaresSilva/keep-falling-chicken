using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossView : BaseView
{
    public Joystick joystick;
    public UnityAction OnPauseClicked;
    public Animator animatorTutorial;
    
    public void ClickPause()
    {
        OnPauseClicked?.Invoke();
    }
}
