using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

public class GameManager : PersistentSingleton<GameManager>
{
    [Header("Save Game")]
    // private RunData runData;
    [SerializeField]private PlayerData _playerData;
    public PlayerData playerData
    {
        get => _playerData;
        private set => _playerData = value;
    }

    public Transform playerTransform;
    public Player playerScript;
    
    [Header("Audio")]
    public ConfigData ConfigData;
    public AudioMixer AudioMixer;
    
    [Header("Loading")]
    public GameObject loadPanel;
    public Slider progressBar;
    public RawImage loadImage;
    public Texture2D[] loadImages;


    [Header("Level")] 
    public Enums.SceneIndexes lastScene = Enums.SceneIndexes.Manager;
    public Enums.SceneIndexes nextScene = Enums.SceneIndexes.LevelOne;

    #region Actions
    public UnityAction sceneLoaded;
    public UnityAction OnGoldChanged;

    

    #endregion
    private void Start()
    {
        SaveSystem.Load(Functions.GetSaveFileName(Enums.SaveGames.PlayerData), out _playerData);
        SaveSystem.Load(Functions.GetSaveFileName(Enums.SaveGames.ConfigData), out ConfigData);
        PowerUpsManager.instance.PowerUpsInit(playerData.powerUpsLevels);
        PowerUpsManager.instance.onPowerUpChange += SavePlayerData;
    }

    protected override void OnDestroy()
    {
        if (PowerUpsManager.instanceExists)
        {
            PowerUpsManager.instance.onPowerUpChange -= SavePlayerData;
        }
        base.OnDestroy();
    }

    public void StartRun()
    {
        playerTransform.position = Vector3.zero;
    }

    public void __DeleteSave()
    {
        _playerData = new PlayerData();
        SaveSystem.Save(Functions.GetSaveFileName(Enums.SaveGames.PlayerData), _playerData);
    }

    #region PlayerDataChanges
    private void SavePlayerData()
    {
        playerData.powerUpsLevels = PowerUpsManager.instance.GetLevels();
        SaveSystem.Save(Functions.GetSaveFileName(Enums.SaveGames.PlayerData),playerData);
    }
    public bool TryToSpendMoney(int value)
    {
        if (value > playerData.gold)
        {
            return false;
        }
        playerData.gold -= value;
        OnGoldChanged?.Invoke();
        SavePlayerData();
        return true;
    }

    public void EarnData(RunData runData)
    {
        playerData.gold += runData.gold;
        playerData.diamond += runData.diamond;
        playerData.highScore = runData.score > playerData.highScore ? runData.score : playerData.highScore;
        SavePlayerData();
    }
    #endregion



    #region AudioManager
        public void SetVolumes(bool save = true)
        {
            if (AudioMixer == null)
            {
                return;
            }
            AudioMixer.SetFloat(Enums.Audios.Effects.ToString(), Functions.LogarithmicDbTransform(Mathf.Clamp01(ConfigData.effectsVolume)));
            AudioMixer.SetFloat(Enums.Audios.Music.ToString(), Functions.LogarithmicDbTransform(Mathf.Clamp01(ConfigData.musicVolume)));

            if (save)
            {
                SaveSystem.Save(Functions.GetSaveFileName(Enums.SaveGames.ConfigData),ConfigData);   
            }
        }
    #endregion
    
    #region LoadScenes

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadScenes(int[] scenes)
    {
        loadImage.texture = loadImages[Random.Range(0,loadImages.Length-1)];
        loadPanel.SetActive(true);
        foreach (var scene in scenes)
        {
            if (!SceneManager.GetSceneByBuildIndex(scene).isLoaded)
            {
                scenesLoading.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            }
        }
        StartCoroutine(GetSceneLoadProgress());
        StartCoroutine(GetTotalProgress());
    }

    private float totalSceneProgress;
    private float totalSpawnProgress;
    private IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }
                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                yield return null;
            }
        }
    }

    private IEnumerator GetTotalProgress()
    {
        while (EnemiesManager.instance == null || !EnemiesManager.instance.isDone)
        {
            if (EnemiesManager.instance == null)
            {
                totalSpawnProgress = 0;
            }
            else
            {
                totalSpawnProgress = Mathf.Round(EnemiesManager.instance.progress * 100f);
            }
            float totalProgress = Mathf.Round((totalSceneProgress + totalSpawnProgress)/2);
            progressBar.value = Mathf.RoundToInt(totalProgress);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        sceneLoaded?.Invoke();
        loadPanel.SetActive(false);
    }
    #endregion


    #region Cameras

    // [Header("Cameras")] 
    [SerializeField] private Camera menuCamera;
    // [SerializeField] private Camera gameCamera;
    // [SerializeField] private Camera SkyBoxCamera;

    public void SetMenuCameraActive(bool value)
    {
        menuCamera.enabled = value; 
        SceneDataHolder.instance.ChangeSceneCamerasStatus(!value);
    }

    #endregion

}

