using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum GameState
{
    Lobby,
    Running,
    WillTryAgain,
    Begin,
    Store
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Movement")]
    public float scaleToTrackMove = 5;
    public float scaleToHeightMove = 5;
    public float speedMovement = 5;

    [Header("Game state")]
    private GameState gameState = GameState.Begin;

    [Header("Gold")] 
    
    [SerializeField] private int _gold;
    [SerializeField] private int _runGold;
    #region Variable ui bind
    public int RunGold
    {
        get => _runGold;
        set
        {
            _runGold = value;
            UIGame.Instance.runGoldText.text = _runGold.ToString();
        }
    }

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            uiLobby.goldText.text = _gold.ToString();
            uiStore.goldText.text = _gold.ToString();
        }
    }
    #endregion

    [Header("UI")] 
    [SerializeField] private GameObject canvasLobby;
    [SerializeField] private GameObject canvasGame;
    [SerializeField] private GameObject canvasTryAgain;
    [SerializeField] private GameObject canvasStart;
    [SerializeField] private GameObject canvasStore;
    public UIController uiController;
    
    // public UIGame uiGame;
    private UILobby uiLobby;
    public UIStore uiStore;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }

        // uiGame = canvasGame.GetComponent<UIGame>();
        uiLobby = canvasLobby.GetComponent<UILobby>();
        uiStore = canvasStore.GetComponent<UIStore>();
        ChangeGameState(GameState.Begin);
    }

    public void GameAwake()
    {
        PlayerData data = SaveSystem.LoadGame();
        
        
        //TODO: load the configs
        
        /*
         * if !configData.hasAlreadyWatchedCutscene -> play cutscene
         */
        Gold = data.gold;
        PowerUpsManager.Instance.PowerUpsInit(data.powerUpLevels);
        ChangeGameState(GameState.Lobby);
    }

    public PlayerData GetPlayerData()
    {
        int[] levels = PowerUpsManager.Instance.GetLevels();
        PlayerData playerData = new PlayerData(Gold, levels);
        return playerData;
    }


    public void StartRun()
    {
        RunGold = 0;
        PowerUpsManager.Instance.powerUps[(int)PowerUpTypes.dash].StartRun();
        UIGame.Instance.dashSlider.value = 0;
        ChangeGameState(GameState.Running);
        EnemiesManager.Instance.ChangeState(true);
    }

    public void RealFinish()
    {
        EnemiesManager.Instance.ChangeState(false);
        Gold += RunGold;
        RunGold = 0;
        
        ChangeGameState(GameState.Lobby);
        SaveSystem.SaveGame();
    }

    public void FinishRun()
    {
        EnemiesManager.Instance.ChangeState(false);
        ChangeGameState(GameState.WillTryAgain);
        //TODO: verificar se o cara vai jogar de novo ou nao
    }

    public void ContinueRun()
    {
        EnemiesManager.Instance.ChangeState(false);
        ChangeGameState(GameState.Running);
    }
    public void EarnGold(int amount)
    {
        RunGold += amount;
        if (!PowerUpsManager.Instance.powerUps[(int)PowerUpTypes.dash].inUse)
        {
            PowerUpsManager.Instance.powerUps[(int)PowerUpTypes.dash].Collect();
            // Dash++;
            //TODO: mudar o valor do Dash
        }
    }

    public void DEV__EarnGold()
    {
        EarnGold(Random.Range(1,20));
    }

    public void ToggleStore(bool open)
    {
        ChangeGameState(open ? GameState.Store : GameState.Lobby);
    }


    private void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
        UIGame.Instance.canvas.SetActive(false);
        canvasStart.SetActive(false);
        canvasTryAgain.SetActive(false);
        canvasLobby.SetActive(false);
        canvasStore.SetActive(false);
        switch (gameState)
        {
            case GameState.Begin:
                canvasStart.SetActive(true);
                break;
            case GameState.Lobby:
                /*
                 * Update lobby ui
                 * destroy player
                 */
                canvasLobby.SetActive(true);
                break;
            case GameState.Running:
                /*
                 * update game hud
                 */
                UIGame.Instance.canvas.SetActive(true);
                break;
            case GameState.WillTryAgain:
                /*
                 * show canvas to try again
                 */
                canvasTryAgain.SetActive(true);
                break;
            case GameState.Store:
                canvasStore.SetActive(true);
                break;
        }
    }
}
