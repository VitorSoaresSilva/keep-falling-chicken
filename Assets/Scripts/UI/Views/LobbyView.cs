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
    public GameObject creditsPanel;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI scoreText;
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
        scoreText.text = GameManager.instance.playerData.highScore.ToString();
    }
    public void Credits(bool value)
    {
        creditsPanel.SetActive(value);
    }
}
