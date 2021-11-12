using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableV2 : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position -= (Vector3.forward * EnemiesManager.Instance.CurrSpeedMovement * Time.deltaTime);
    }
}
