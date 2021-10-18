using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI[] levelsText;
    public TextMeshProUGUI[] costsText;
    public GameObject canvas;
    public static UIStore Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
    }
    public void UpdateLevels(int[] levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levelsText[i].text = levels[i].ToString();
        }
    }

    public void UpdateCosts(string[] costs)
    {
        for (int i = 0; i < costs.Length; i++)
        {
            costsText[i].text = costs[i];
        }
    }
    
}
