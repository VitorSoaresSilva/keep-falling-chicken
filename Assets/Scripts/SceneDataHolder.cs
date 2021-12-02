using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SceneDataHolder : Singleton<SceneDataHolder>
{
    public Camera mainCamera;
    public Player player;
    public Transform playerTransform;
    public GameObject zequinha;
    public PlayerWinScript playerScriptWin;
    public GameObject parentOfConsumables;

    public void ChangeSceneCamerasStatus(bool value)
    {
        mainCamera.enabled = value;
    }
}
