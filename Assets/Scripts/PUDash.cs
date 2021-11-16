using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUDash : PowerUp
{
   [SerializeField] private float DashSpeed = 25;
   [SerializeField] private int amountCoinsToDash = 20;
   private int currAmountCoinsToDash;

   public override void Use()
    {
        PowerUpsManager.instance.OnDashCanBeUsedChanged?.Invoke(false);
        PowerUpsManager.instance.OnDashUsedChanged?.Invoke(DashSpeed);
        inUse = true;
        PowerUpsManager.instance.playerInvincible = true;
        StartCoroutine(nameof(Disable));
    }

   public override void StartRun()
    {
        StopAllCoroutines();
        currAmountCoinsToDash = 0;
        PowerUpsManager.instance.OnValueToDashChanged?.Invoke(0);
        PowerUpsManager.instance.OnDashCanBeUsedChanged?.Invoke(false);
    }

    public override void Collect()
    {
        currAmountCoinsToDash++;
        if (currAmountCoinsToDash >= amountCoinsToDash)
        {
            //TODO active button
            PowerUpsManager.instance.OnDashCanBeUsedChanged?.Invoke(true);
        }
        PowerUpsManager.instance.OnValueToDashChanged?.Invoke(Mathf.Clamp01((float)currAmountCoinsToDash / amountCoinsToDash));
    }


    public override IEnumerator Disable()
    {
        float currTime = 0;
        while (currTime < Value)
        {
            //UIGame.Instance.dashSlider.value = 1 - currTime / Value;
            PowerUpsManager.instance.OnValueToDashChanged?.Invoke(Mathf.Clamp01(1 - currTime / Value));
            currTime += Time.deltaTime;
            yield return null;
        }
        //UIGame.Instance.dashSlider.value = 0;
        //EnemiesManager.Instance.ChangeSpeed(GameManager.Instance.speedMovement);
        PowerUpsManager.instance.OnValueToDashChanged?.Invoke(0);
        // PowerUpsManager.instance.OnDashCanBeUsedChanged?.Invoke(false);
        PowerUpsManager.instance.OnDashUsedChanged?.Invoke(-DashSpeed);
        currAmountCoinsToDash = 0;
        inUse = false;
        PowerUpsManager.instance.playerInvincible = false;
    }
    
    
    
    
    /*
     * Dash
     * --começo
     *  mudar a velocidade de movimento dos inimigos
     * ativar um efeito
     * começar a mover o slider
     *
     * --quando terminar
     * eu vou explodir tudo o que estiver num raio x
     * voltar a velocidade ao normal
     */
}
