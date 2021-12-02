using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinScript : MonoBehaviour
{
    public float maxDistanceMoving;
    private bool isNear;
    private void Update()
    {
        if (!isNear)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, maxDistanceMoving * Time.deltaTime);
            if (Vector3.Distance(transform.position, Vector3.zero) < 0.01)
            {
                isNear = true;
                Invoke(nameof(Near),2);
            }
        }
    }

    private void Near()
    {
        RunManager.instance.ZequinhaCollected();
    }
}
