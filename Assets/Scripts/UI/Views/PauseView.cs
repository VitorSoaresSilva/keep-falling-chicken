using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseView : BaseView
{
    public UnityAction OnResumeClicked;
    public UnityAction OnMenuClicked;
    public UnityAction OnConfigClicked;
    
    public void ResumeClick()
    {
        OnResumeClicked?.Invoke();
    }

    public void MenuClick()
    {
        OnMenuClicked?.Invoke();
    }
    public void ClickConfig()
    {
        OnConfigClicked?.Invoke();
    }
}
