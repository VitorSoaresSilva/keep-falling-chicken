using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
   public TextMeshProUGUI runGoldText;
   public Slider dashSlider;
   public Button dash;
   public GameObject canvas;
   public static UIGame Instance { get; private set; }
   private void Awake()
   {
      if(Instance != null && Instance != this){
         Destroy(this.gameObject);
      }else{
         Instance = this;
      }
      canvas = this.gameObject;
   }
}
