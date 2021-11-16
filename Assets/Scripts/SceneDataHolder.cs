using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SceneDataHolder : Singleton<SceneDataHolder>
{
    public Camera mainCamera;
    public Camera skyBoxCamera;
    public Player player;
    public Transform playerTransform;

    public Animator sceneAnimation;



    public void ChangeSceneCamerasStatus(bool value)
    {
        mainCamera.enabled = value;
        skyBoxCamera.enabled = value;
    }
}
