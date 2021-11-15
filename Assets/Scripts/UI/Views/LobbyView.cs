using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LobbyView : BaseView
{
    public UnityEvent OnPlayClicked;
    public UnityEvent OnConfigClicked;
    public UnityEvent OnStoreClicked;

    public TextMeshProUGUI goldText;
    public void ClickPlay()
    {
        OnPlayClicked?.Invoke();
    }
    public void ClickConfig()
    {
        OnConfigClicked?.Invoke();
    }

    public void ClickStore()
    {
        OnStoreClicked?.Invoke();
    }

    public void UpdateValues()
    {
        goldText.text = GameManager.instance.playerData.gold.ToString();
    }
}
