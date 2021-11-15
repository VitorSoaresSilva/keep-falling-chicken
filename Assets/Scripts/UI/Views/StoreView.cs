using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StoreView : BaseView
{
    public UnityEvent OnPlayClicked;
    public UnityEvent OnLobbyClicked;

    public TextMeshProUGUI[] costsText;
    public TextMeshProUGUI[] levelsText;
    public TextMeshProUGUI goldText;
    // public UnityAction OnPowerUpUpgraded;
    
    public void ClickPlay()
    {
        OnPlayClicked?.Invoke();
    }

    public void ClickLobby()
    {
        OnLobbyClicked?.Invoke();
    }

    public void UpgradePowerUp(int powerUp)
    {
        PowerUpsManager.instance.UpgradePowerUp(powerUp);
    }

    public void UpdateValues()
    {
        int[] levels = PowerUpsManager.instance.GetLevels();
        string[] costs = PowerUpsManager.instance.GetCostsText();
        goldText.text = GameManager.instance.playerData.gold.ToString();
        
        for (int i = 0; i < levels.Length; i++)
        {
            costsText[i].text = costs[i];
            levelsText[i].text = levels[i].ToString();
        }
    }
}
