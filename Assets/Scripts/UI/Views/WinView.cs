using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinView : BaseView
{
    [SerializeField] private TextMeshProUGUI ScoreLabel;
    [SerializeField] private TextMeshProUGUI GoldLabel;
    public UnityAction OnReplayClicked;
    public UnityAction OnMenuClicked;
    public UnityAction OnStoreClicked;
    public RunData data;

    public void ReplayClicked()
    {
        OnReplayClicked?.Invoke();
    }

    public void MenuClicked()
    {
        OnMenuClicked?.Invoke();
    }

    public void StoreClicked()
    {
        OnStoreClicked?.Invoke();
    }

    public void UpdateValues()
    {
        
        ScoreLabel.text = data.score.ToString().PadLeft(4,'0');
        GoldLabel.text = data.gold.ToString().PadLeft(4,'0');
    }
}
