using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUDash : PowerUp
{
   [SerializeField] private float DashSpeed = 25;
   [SerializeField] private int amountCoinsToDash = 20;
   private int currAmountCoinsToDash;
   public int Dash
   {
       get => currAmountCoinsToDash;
       set
       {
           if (value < amountCoinsToDash)
           {
               currAmountCoinsToDash = value;
               UIGame.Instance.dash.enabled = false;
           }
           else
           {
               UIGame.Instance.dash.enabled = true;
               currAmountCoinsToDash = amountCoinsToDash;
           }
           UIGame.Instance.dashSlider.value = (float)currAmountCoinsToDash / amountCoinsToDash;
       }
   }

   public override void Use()
    {
        EnemiesManager.Instance.ChangeSpeed(DashSpeed + GameManager.Instance.speedMovement);
        UIGame.Instance.dash.enabled = false;
        inUse = true;
        StartCoroutine(nameof(DisableDash));
    }

    public override void StartRun()
    {
        StopAllCoroutines();
        Dash = 0;
        UIGame.Instance.dash.enabled = false;
        UIGame.Instance.dashSlider.value = 0;
    }

    public override void Collect()
    {
        Dash++;
    }


    private IEnumerator DisableDash()
    {
        float currTime = 0;
        while (currTime < Value)
        {
            UIGame.Instance.dashSlider.value = 1 - currTime / Value;
            currTime += Time.deltaTime;
            yield return null;
        }
        UIGame.Instance.dashSlider.value = 0;
        EnemiesManager.Instance.ChangeSpeed(GameManager.Instance.speedMovement);
        Dash = 0;
        inUse = false;
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
