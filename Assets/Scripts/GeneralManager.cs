using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GeneralManager : MonoBehaviour
{
    #region GameManager

        private State state;
    

    #endregion
    #region Player
        public int gold;
        public PowerUp[] powerUps;
        public PData pdata;
    #endregion
    #region RunManager
        public int runGold;
    #endregion
    #region UIManager
    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private GameObject canvasStore;
    private Button[] buttonsStore;
    private bool storeOpen;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (state == State.Running)
            {
                RunFinished();
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (state == State.Lobby)
            {
                RunStarted();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame(pdata);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            runGold += Random.Range(10, 50); // fake earn of money
            UIUpdate_Gold(runGold);
        }
    }

    public void GameStart()
    {
        // Debug.Log("Game start");
        // //Initialize
        // pdata = LoadGame();
        // gold = pdata.gold;
        //
        // powerUps = new PowerUp[(int)PowerUpTypes.COUNT];
        // for (int i = 0; i < (int)PowerUpTypes.COUNT; i++)
        // {
        //     Debug.Log(pdata.powerUpLevels[i]);
        //     powerUps[i] = new PowerUp(global::PowerUpTypes)i,pdata.powerUpLevels[i]);
        // }
        //
        //
        // UIUpdate_Gold(gold);
        // state = State.Lobby;
        // UIUpdate_State(state);

        
    }

    #region RunManagerFunctions
        void RunStarted()
        {
            runGold = 0;
            //cutscene
            UIUpdate_Gold(0);

            state = State.Running;
            UIUpdate_State(state);
        }

        void RunFinished()
        {
            // earn gold
            gold += runGold;
            UIUpdate_Gold(gold);
            runGold = 0;
            // save data
            pdata.gold = gold;
            
            SaveGame(pdata);
            
            //back to lobby
            state = State.Lobby;
            UIUpdate_State(state);
        }
    #endregion

    #region SaveSystem
        PData LoadGame()
        {
            string path = Application.persistentDataPath + "/player1.pd";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PData data = formatter.Deserialize(stream) as PData;
                stream.Close();
                return data;
            }
            else
            {
                Debug.LogError("Save file not found");
                return new PData();
            }   
        }

        void SaveGame(PData playerData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player1.pd";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream,playerData);
            stream.Close();
        }

    #endregion

    #region UI Manager

    void UIUpdate_Gold(int gold)
    {
        goldText.text = "$" + gold;
    }

    void UIUpdate_State(State state)
    {
        stateText.text = "Game is " + state;
    }

    public void UpgradePowerUp(int i)
    {
        // if (powerUps[i].Cost > gold || !powerUps[i].CanUpgrade) return;
        // powerUps[i].Upgrade();
        // gold -= powerUps[i].Cost;
        // UIUpdate_Gold(gold);
        //
        //
        // int[] levels = new int[(int)PowerUpTypes.COUNT];
        // for (int j = 0; j < (int)PowerUpTypes.COUNT; j++)
        // {
        //     levels[i] = powerUps[i].level;
        // }
        //
        // pdata.powerUpLevels = levels;
        // SaveGame(pdata);
    }

    public void ToggleStore()
    {
        storeOpen = !storeOpen;
        canvasStore.SetActive(storeOpen);
    }

    #endregion

    enum State
    {
        Lobby,
        Running,
    }
    
    [Serializable]
    public class PData
    {
        public int gold;
        public int[] powerUpLevels;
        public PData()
        {
            gold = 0;
            powerUpLevels = new int[(int)PowerUpTypes.COUNT];
            // Debug.Log("opa: " + powerUpLevels[0]);
        }
        public PData(int gold, int[] powerUpLevels)
        {
            this.gold = gold;
            this.powerUpLevels = powerUpLevels;
        }
    }

    enum PowerUpTypes
    {
        Magnet,
        Dash,
        Shield,
        Base,
        COUNT
    }

    // [Serializable]
    // public class PowerUpData
    // {
    //     public int[] levels;
    // }
    public class PowerUp
    {
        // public int level;
        // public global::PowerUpTypes_old TypeOld;
        // public int[] costs;
        //
        // public int Cost => costs[level];
        // public bool CanUpgrade => level < costs.Length;
        //
        // public PowerUp(global::PowerUpTypes_old typeOld)
        // {
        //     level = 0;
        //     this.TypeOld = typeOld;
        //     costs = CostValues[(int)typeOld];
        // }
        // public PowerUp(global::PowerUpTypes_old typeOld,int level)
        // {
        //     this.level = level;
        //     this.TypeOld = typeOld;
        //     costs = CostValues[(int)typeOld];
        // }
        //
        // public void Upgrade()
        // {
        //     level++;
        // }
    }
    
    private static readonly int[][] CostValues =
    {
        new int[]{15,20,30,40,50}, // magnet
        new int[]{15,20,30,40,50}, // dash
        new int[]{15,20,30,40,50}, // shield
        new int[]{15,20,30,40,50}, // base
    };
    private static readonly float[][] Values =
    {
        new float[]{5,6,7,8,9,10}, // magnet
        new float[]{5,6,7,8,9,10}, // dash
        new float[]{5,6,7,8,9,10}, // shield
        new float[]{5,6,7,8,9,10}, // base
    };
}
