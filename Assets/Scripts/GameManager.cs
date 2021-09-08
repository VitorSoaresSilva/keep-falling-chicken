using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get{ return _instance; } }
    public float scaleToTrackMove = 5;
    public float scaleToHeightMove = 5;
    public enum Track{ Left,  Middle, Right, NumberOfTypes}
    public float speedMovement = 0;
    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
        }
    }
}
