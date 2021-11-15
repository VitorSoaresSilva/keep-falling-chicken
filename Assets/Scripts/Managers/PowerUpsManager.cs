using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
public enum PowerUpTypes
{
    magnet,
    shield,
    dash,
    doublePoints,
    COUNT
}
[RequireComponent(typeof(PUMagnet))]
[RequireComponent(typeof(PUShield))]
[RequireComponent(typeof(PUDash))]
[RequireComponent(typeof(PUDoublePoints))]
public class PowerUpsManager : PersistentSingleton<PowerUpsManager>
{
    public PowerUp[] powerUps;
    public int AmountPowerUps = (int)PowerUpTypes.COUNT;
    public UnityAction onPowerUpChange;
    private PUMagnet _puMagnet;
    
    public void UpgradePowerUp(int index)// ui buttons
    {
        if (!powerUps[index].CanUpgrade) return;
        if (!GameManager.instance.TryToSpendMoney(powerUps[index].Cost)) return;
        powerUps[index].Upgrade();
        onPowerUpChange?.Invoke();
    }

    public void PowerUpsInit(int[] levels)
    {
        powerUps = new PowerUp[AmountPowerUps];
        powerUps = GetComponents<PowerUp>();
        for (int i = 0; i < AmountPowerUps; i++)
        {
            powerUps[i].Init((PowerUpTypes)i,levels[i]);
        }
        onPowerUpChange?.Invoke();
        // UIStore.Instance.UpdateLevels(levels);
        // UIStore.Instance.UpdateCosts(GetCostsText());
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
    public int[] GetLevels()
    {
        int[] levels = new int[AmountPowerUps];
        for (int i = 0; i < AmountPowerUps; i++)
        {
            levels[i] = powerUps[i].level;
        }
        return levels;
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
        new float[]{5,6,7,8,9,10,11,12,13,14}, // magnet
        new float[]{5,6,7,8,9,10,11,12,13,14}, // dash
        new float[]{5,6,7,8,9,10,11,12,13,14}, // shield
        new float[]{5,6,7,8,9,10,11,12,13,14}, // base
    };
}
