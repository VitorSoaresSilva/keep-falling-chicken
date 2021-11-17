using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
// using UnityEngine.UIElements;
using UnityEngine.UI;
public class StoreView : BaseView
{
    public UnityEvent OnPlayClicked;
    public UnityEvent OnLobbyClicked;

    public TextMeshProUGUI[] costsText;
    public TextMeshProUGUI[] levelsText;
    public TextMeshProUGUI goldText;

    // public Sprite levelOn;
    // public Sprite levelOff;

    public Sprite levelOnTex;
    public Sprite levelOffTex;
    public Image[][] imagesLevels;
    public Image[] imagesLevelMagnet;
    public Image[] imagesLevelShield;
    public Image[] imagesLevelDash;
    public Image[] imagesLevelDouble;

    private void Initialize()
    {
        imagesLevels = new Image[][] { new Image[6],new Image[6],new Image[6],new Image[6] };
        imagesLevels[0] = imagesLevelMagnet;
        imagesLevels[1] = imagesLevelDash;
        imagesLevels[2] = imagesLevelShield;
        imagesLevels[3] = imagesLevelDouble;
    }

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
        if (imagesLevels == null)
        {
            Initialize();
        }
        int[] levels = PowerUpsManager.instance.GetLevels();
        string[] costs = PowerUpsManager.instance.GetCostsText();
        goldText.text = GameManager.instance.playerData.gold.ToString();
        
        for (int i = 0; i < levels.Length; i++)
        {
            costsText[i].text = costs[i];
            
            for (int j = 0; j < imagesLevels[i].Length; j++)
            {
                imagesLevels[i][j].sprite = j < levels[i] ? levelOnTex : levelOffTex;
            }
        }
    }
}
