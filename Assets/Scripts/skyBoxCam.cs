using UnityEngine;
using System.Collections;

public class skyBoxCam : MonoBehaviour
{

    public GameObject PlayerCamera;
    // public float FOV = 60.0f;
    public float scaleFloat = 0.02f;

    // LateUpdate is for camera functions - REMEMBER THIS!!
    // void LateUpdate()
    // {
    //     var transform1 = transform;
    //     transform1.rotation = PlayerCamera.transform.rotation;
    //     var position = PlayerCamera.transform.position;
    //     transform1.position = new Vector3(position.x * scaleFloat -100, position.y * scaleFloat -100, position.z * scaleFloat -100);
    // }

}