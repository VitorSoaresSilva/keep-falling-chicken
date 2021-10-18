using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public GameObject canvas;
    public static UILobby Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
    }
}
