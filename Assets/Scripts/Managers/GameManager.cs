using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Texture2D[] loadImagesNormal;
    public Texture2D[] loadImagesBoss;


    [Header("Level")] 
    // public

    #region Actions
    public UnityAction sceneLoaded;
    public UnityAction sceneUnloaded;
    public UnityAction OnGoldChanged;

    

    #endregion
    private void Start()
    {
        SaveSystem.Load(Functions.GetSaveFileName(Enums.SaveGames.PlayerData), out _playerData);
        SaveSystem.Load(Functions.GetSaveFileName(Enums.SaveGames.ConfigData), out ConfigData);
        SetVolumes(false);
        PowerUpsManager.instance.PowerUpsInit(playerData.powerUpsLevels);
        PowerUpsManager.instance.onPowerUpLevelChange += SavePlayerData;
        // RunManager.instance.OnBossFightCloseToBegin += Han;
    }

    protected override void OnDestroy()
    {
        if (PowerUpsManager.instanceExists)
        {
            PowerUpsManager.instance.onPowerUpLevelChange -= SavePlayerData;
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

    public enum LoadImageType
    {
        Normal,
        Boss,
    }

    public void SetStateLoadScene(bool value,LoadImageType loadImageType = LoadImageType.Normal)
    {
        if (!value)
        {
            loadPanel.SetActive(false);
            return;
        }
        switch (loadImageType)
        {
            case LoadImageType.Normal:
                loadImage.texture = loadImagesNormal[Random.Range(0,loadImagesNormal.Length-1)];
                break;
            case LoadImageType.Boss:
                loadImage.texture = loadImagesBoss[Random.Range(0,loadImagesBoss.Length-1)];
                break;
            default:
                loadImage.texture = loadImagesNormal[Random.Range(0,loadImagesNormal.Length-1)];
                break;
        }
        loadPanel.SetActive(true);
    }
    
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadScenes(int[] scenes)
    {
        foreach (var scene in scenes)
        {
            if (!SceneManager.GetSceneByBuildIndex(scene).isLoaded)
            {
                scenesLoading.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            }
        }
        StartCoroutine(GetSceneLoadProgress(scenesLoading));
        StartCoroutine(GetTotalProgress(RunManager.instance.currentState == RunManager.State.Boss));
    }

    private float totalSceneProgress;
    private float totalSpawnProgress;
    private IEnumerator GetSceneLoadProgress(List<AsyncOperation> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach (AsyncOperation operation in list)
                {
                    totalSceneProgress += operation.progress;
                }
                totalSceneProgress = (totalSceneProgress / list.Count) * 100f;
                yield return null;
            }
        }
    }
    private IEnumerator GetSceneUnloadProgress(List<AsyncOperation> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach (AsyncOperation operation in list)
                {
                    totalSceneProgress += operation.progress;
                }
                totalSceneProgress = (totalSceneProgress / list.Count) * 100f;
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.5f);
        sceneUnloaded?.Invoke();
        // loadPanel.SetActive(false);
    }
    private List<AsyncOperation> scenesUnloading = new List<AsyncOperation>();
    public void UnloadAnotherScenes(int[] scenesToIgnore)
    {
        int temp = SceneManager.sceneCount;
        for (int i = 0; i < temp; i++)
        {
            if (!scenesToIgnore.Contains(SceneManager.GetSceneAt(i).buildIndex))
            {
                scenesUnloading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));
            }
        }
        StartCoroutine(GetSceneUnloadProgress(scenesUnloading));
    }

    private IEnumerator GetTotalProgress(bool boss = false)
    {
        if (!boss)
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
        }
        yield return new WaitForSeconds(0.5f);
        sceneLoaded?.Invoke();
        loadPanel.SetActive(false);
    }
    #endregion
    
    #region Cameras
    public Camera menuCamera;
    public void SetMenuCameraActive(bool value)
    {
        menuCamera.enabled = value;
        if (SceneDataHolder.instanceExists)
        {
            SceneDataHolder.instance.ChangeSceneCamerasStatus(!value);
        }
    }

    #endregion

}

