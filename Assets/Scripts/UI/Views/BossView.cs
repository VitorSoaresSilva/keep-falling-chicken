using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossView : BaseView
{
    
    public UnityAction OnPauseClicked;
    
    public void ClickPause()
    {
        OnPauseClicked?.Invoke();
    }
}
