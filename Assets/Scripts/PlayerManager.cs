using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    // [SerializeField] private int gold;
    // [SerializeField] private int diamonds;
    


    
    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
    }
    
        /*
         * passar o runGold para gold
         * destroy player
         * avisar o game manager pra mudar de cena
         * 
         */
        
        /*
         * player colidiu
         *
         * pausar o jogo
         * --setar o novo gold
         * mostrar tela de derrota
         * perguntar pra onde ele vai
         * ver se ele vai continuar o jogo
         */
    
}
