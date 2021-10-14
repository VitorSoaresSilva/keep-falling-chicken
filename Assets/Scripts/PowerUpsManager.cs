using System.Collections;
using UnityEngine;

public enum PowerUpTypes
{
    magnet,
    shield,
    dash,
    doublePoints,
    COUNT
}


public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance { get; private set; }
    public PowerUp[] powerUps;
    private const int AmountPowerUps = (int)PowerUpTypes.COUNT;
    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
    }
    public void UpgradePowerUp(int index)// ui buttons
    {
        if (!powerUps[index].CanUpgrade || powerUps[index].Cost > GameManager.Instance.Gold) return;
        
        GameManager.Instance.Gold -= powerUps[index].Cost;
        powerUps[index].Upgrade();

        GameManager.Instance.uiStore.UpdateLevels(GetLevels());
        GameManager.Instance.uiStore.UpdateCosts(GetCostsText());
        
        SaveSystem.SaveGame();
    }

    public void PowerUpsInit(int[] levels)
    {
        powerUps = new PowerUp[AmountPowerUps];
        powerUps = GetComponents<PowerUp>();
        for (int i = 0; i < AmountPowerUps; i++)
        {
            powerUps[i].Init((PowerUpTypes)i,levels[i]);
        }
        GameManager.Instance.uiStore.UpdateLevels(levels);
        GameManager.Instance.uiStore.UpdateCosts(GetCostsText());
    }

    public int[] GetLevels()
    {
        int[] levels = new int[AmountPowerUps];
        for (int i = 0; i < AmountPowerUps; i++)
        {
            levels[i] = powerUps[i].level;
        }
        return levels;
    }

    public void UsePowerUp(int type)
    {
        switch ((PowerUpTypes)type)
        {
            case PowerUpTypes.dash:
                if (!powerUps[type].inUse)
                {
                    powerUps[type].Use();
                }
                break;
            case PowerUpTypes.magnet:
                
                break;
            case PowerUpTypes.shield:
                break;
            case PowerUpTypes.doublePoints:
                break;
            
        }
    }
    
    public string[] GetCostsText()
    {
        string[] costs = new string[AmountPowerUps];
        for (int i = 0; i < AmountPowerUps; i++)
        {
            costs[i] = powerUps[i].CostText;
        }
        return costs;
    }

    public static readonly int[][] CostValues =
    {
        new[]{15,20,30,40,50,60,70,80,90}, // magnet
        new[]{15,20,30,40,50,60,70,80,90}, // dash
        new[]{15,20,30,40,50,60,70,80,90}, // shield
        new[]{1,1,2,3,4,5,6,7,8}, // base
    };
    public static readonly float[][] Values =
    {
        new float[]{1,1,2,3,4,5,6,7,8,9}, // magnet
        new float[]{5,6,7,8,9,10,11,12,13,14}, // dash
        new float[]{5,6,7,8,9,10,11,12,13,14}, // shield
        new float[]{5,6,7,8,9,10,11,12,13,14}, // base
    };

    /*
     *
     *
     *
     * TODO: sistema de mudar a velocidade dos inimigos ta bem zaodo
     * e o sistema de dash estar no game manager tambem ta estranho
     * 
     */
}
