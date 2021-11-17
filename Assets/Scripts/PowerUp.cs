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
    public bool CanUpgrade => level < costs.Length;
    public string CostText => level < costs.Length ? costs[level].ToString() : "Max";
    public float Value => values[level];
    public int Cost => level < costs.Length ? costs[level] : 0;
    public virtual void Collect(){}
    public virtual void StartRun(){}

    public virtual IEnumerator Disable()
    {
        float currTime = 0;
        while (currTime < Value)
        {
            currTime += Time.deltaTime;
            yield return null;
        }
        AfterDisable();
    }

    protected virtual void AfterDisable()
    {
        inUse = false;
    }
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
}

