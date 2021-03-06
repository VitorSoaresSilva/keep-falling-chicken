using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    public int gold = 1;
    public Transform playerTransform;
    public float maxDistanceMagnetMove;

    public void MagnetActive(Transform player)
    {
        playerTransform = player;
        StartCoroutine(nameof(Magnet));
    }

    IEnumerator Magnet()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,maxDistanceMagnetMove);
            yield return null;
        }
    }

}
