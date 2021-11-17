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
    public UnityAction onPowerUpLevelChange;
    // private PUMagnet _puMagnet;
    public bool playerInvincible = false;
    public UnityAction<float> OnValueToDashChanged;
    public UnityAction<bool> OnDashCanBeUsedChanged;
    public UnityAction<float> OnDashUsedChanged;
    
    
    public void UpgradePowerUp(int index)// ui buttons
    {
        if (!powerUps[index].CanUpgrade) return;
        if (!GameManager.instance.TryToSpendMoney(powerUps[index].Cost)) return;
        powerUps[index].Upgrade();
        onPowerUpLevelChange?.Invoke();
    }

    public void PowerUpsInit(int[] levels)
    {
        powerUps = new PowerUp[AmountPowerUps];
        powerUps = GetComponents<PowerUp>();
        for (int i = 0; i < AmountPowerUps; i++)
        {
            powerUps[i].Init((PowerUpTypes)i,levels[i]);
        }
        onPowerUpLevelChange?.Invoke();
        // UIStore.Instance.UpdateLevels(levels);
        // UIStore.Instance.UpdateCosts(GetCostsText());
    }

    

    public void Use(PowerUpTypes type)
    {
        if (!powerUps[(int)type].inUse)
        {
            powerUps[(int)type].Use();
        }
    }

    public void CollectPowerUp(PowerUpTypes type)
    {
        if (!powerUps[(int)type].inUse)
        {
            if (type == PowerUpTypes.dash)
            {
                powerUps[(int)type].Collect();
            }
            else
            {
                powerUps[(int)type].Use();
            }
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
        new[]{15,20,30,40,50,60}, // magnet
        new[]{15,20,30,40,50,60}, // dash
        new[]{15,20,30,40,50,60}, // shield
        new[]{15,20,30,40,50,60}, // base
    };
    public static readonly float[][] Values =
    {
        new float[]{5,6,7,8,9,10,11}, // magnet
        new float[]{5,6,7,8,9,10,11}, // dash
        new float[]{5,6,7,8,9,10,11}, // shield
        new float[]{5,6,7,8,9,10,11}, // base
    };
}
