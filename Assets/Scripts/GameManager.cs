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

    [Header("Player")] 
    [SerializeField] private Player player;
    #region Variable ui bind
    private int RunGold
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
            UILobby.Instance.goldText.text = _gold.ToString();
            UIStore.Instance.goldText.text = _gold.ToString();
        }
    }
    #endregion

    [Header("UI")] 
    [SerializeField] private GameObject canvasTryAgain;
    [SerializeField] private GameObject canvasStart;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }else{
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
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
        player.enabled = false;
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
        EnemiesManager.Instance.ChangeState(true);
        UIGame.Instance.dashSlider.value = 0;
        ChangeGameState(GameState.Running);
        player.enabled = true;
    }

    public void RealFinish()
    {
        EnemiesManager.Instance.ChangeState(false);
        Gold += RunGold;
        RunGold = 0;
        
        // ChangeGameState(GameState.Lobby);
        SaveSystem.SaveGame();
    }

    public void FinishRun()
    {
        Debug.Log("end game");
        ChangeGameState(GameState.WillTryAgain);
        EnemiesManager.Instance.ChangeState(false);
        player.enabled = false;
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
        }
    }

    public void DEV__EarnGold()
    {
        EarnGold(Random.Range(1,20));
    }

    public void DEV__MagnetActivate()
    {
        PowerUpsManager.Instance.powerUps[(int)PowerUpTypes.magnet].Collect();        
    }

    public void ToggleStore(bool open)
    {
        ChangeGameState(open ? GameState.Store : GameState.Lobby);
    }


    private void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
        StartCoroutine(nameof(CloseCanvas));
    }

    private IEnumerator CloseCanvas()
    {
        yield return new WaitForSeconds(0.2f);
        canvasStart.SetActive(false);
        canvasTryAgain.SetActive(false);
        UIGame.Instance.canvas.SetActive(false);
        UILobby.Instance.canvas.SetActive(false);
        UIStore.Instance.canvas.SetActive(false);
        OpenCanvas();
        yield return null;
    }

    private void OpenCanvas()
    {
        switch (gameState)
        {
            case GameState.Begin:
                canvasStart.SetActive(true);
                break;
            case GameState.Lobby:
                UILobby.Instance.canvas.SetActive(true);
                break;
            case GameState.Running:
                UIGame.Instance.canvas.SetActive(true);
                break;
            case GameState.WillTryAgain:
                canvasTryAgain.SetActive(true);
                break;
            case GameState.Store:
                UIStore.Instance.canvas.SetActive(true);
                break;
        }
    }
}
