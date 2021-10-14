using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PowerUp: MonoBehaviour
{
    public int level;
    public PowerUpTypes type;
    public int[] costs;
    private float[] values;
    public bool inUse;
    // public 
    public abstract void Use();
    public abstract void StartRun();
    public abstract void Collect();

    public int Cost => level < costs.Length ? costs[level] : 0;
    public string CostText => level < costs.Length ? costs[level].ToString() : "Level max";
    public float Value => values[level];
    public bool CanUpgrade => level < costs.Length;

    public void Init(PowerUpTypes type,int level)
    {
        this.level = level;
        this.type = type;
        costs = PowerUpsManager.CostValues[(int)type];
        values = PowerUpsManager.Values[(int)type];
        inUse = false;
    }

    public void Init(PowerUpTypes type) {
        costs = PowerUpsManager.CostValues[(int)type];
        values = PowerUpsManager.Values[(int)type];
        inUse = false;
        this.type = type;
        level = 0;
    }

    public void Upgrade()
    {
        level++;
    }
    
    /*
     * class coletavel
     * type poder
     *
     * shield
     * double points
     * magnet
     *
     * random shield
     *
     * peguei um shield
     */
}

